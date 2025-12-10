module day10_part02

open AdventOfCode_2025.Modules
open System.Text.RegularExpressions
open Microsoft.Z3

type Machine = {
    Id: int;
    LightDiagram: bool array;
    Buttons: (int array) seq
    Joltages: int array
}

let parseContent (lines: string array) =
    lines
    |> Array.mapi (fun idx line ->
        let id = idx
        let parts = line.Split(" ")
        let lightsRegex = Regex.Match(parts[0], @"\[([^\]]*)\]")
        let lightDiagram = 
            lightsRegex.Groups[1].Value.ToCharArray() 
            |> Array.map (fun c -> c = '#')
        let buttons=
            parts[1..parts.Length-2]
            |> Array.map (fun btnStr ->
                Regex.Match(btnStr, @"\(([^)]*)\)").Groups[1].Value.Split(",")
                |> Array.map int
            )
        let joltajes = 
            Regex.Match(parts[parts.Length-1], @"{([^>]*)}").Groups[1].Value.Split(",")
            |> Array.map int
        {
            Id = id;
            LightDiagram = lightDiagram;
            Buttons = buttons;
            Joltages = joltajes
        }
    )

let minimumPresses (machine: Machine) =
    let target = machine.Joltages
    let buttons = 
        machine.Buttons 
        |> Seq.map (fun btnIndices ->
            let effects = Array.zeroCreate target.Length
            for idx in btnIndices do
                if idx >= 0 && idx < target.Length then
                    effects[idx] <- effects[idx] + 1
            effects
        )
        |> Seq.toArray
    
    if target |> Array.forall ((=) 0) then
        0
    else
        let cfg = System.Collections.Generic.Dictionary()
        cfg.Add("model", "true")
        use ctx = new Context(cfg)
        use solver = ctx.MkOptimize()

        // convert buttons to variables
        let buttonVars = 
            buttons 
            |> Array.mapi (fun idx _ -> 
                let var = ctx.MkIntConst($"button_{idx}")
                solver.Add(ctx.MkGe(var, ctx.MkInt(0)))
                var
            )

        for posIdx in 0 .. target.Length - 1 do
            let contributions = 
                buttonVars 
                |> Array.mapi (fun btnIdx btnVar ->
                    let effect = buttons[btnIdx][posIdx]
                    if effect > 0 then
                        Some(ctx.MkMul(btnVar, ctx.MkInt(effect)))
                    else
                        None
                )
                |> Array.choose id

            if contributions.Length > 0 then
                let sumExpr = ctx.MkAdd(contributions |> Array.map (fun x -> x :> ArithExpr))
                let targetExpr = ctx.MkInt(target[posIdx])
                solver.Add(ctx.MkEq(sumExpr, targetExpr))
            elif target[posIdx] <> 0 then
                // ?? it shouldn't be possible to reach here.
                // at least one button should affect each position with non-zero target
                failwith $"machine {machine.Id} - position {posIdx} cannot be modified"

        let totalPresses = ctx.MkAdd(buttonVars |> Array.map (fun x -> x :> ArithExpr))
        solver.MkMinimize(totalPresses) |> ignore

        match solver.Check() with
        | Status.SATISFIABLE ->
            let model = solver.Model
            let totalValue = 
                buttonVars 
                |> Array.sumBy (fun var -> 
                    model.Eval(var, true).ToString() |> int
                )
            //printfn $"machine {machine.Id} requires {totalValue}"
            totalValue        
        |_ ->
            failwith $"Unexpected solver status for machine {machine.Id}"

let execute() =
    //let path = "day10/test_input_10.txt"
    let path = "day10/day10_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let machines = parseContent content
    
    machines 
    |> Array.Parallel.mapi (fun idx machine ->
        minimumPresses machine
    )
    |> Array.sum