module day13_part02

open AdventOfCode_2024.Modules
open System.Text.RegularExpressions
open AdventOfCode_Utilities
open Microsoft.Z3

type Push = {
    Name: string;
    X : int64;
    Y : int64;
}

type Combination = {
    ButtonA : Push;
    ButtonB : Push;
    ResultX : int64;
    ResultY : int64;
}

let getValues(line: string) =
    let regexp = @"(\d+)[^\d]+(\d+)"
    let regex = Regex(regexp)
    match regex.Match(line) with
    | m -> (System.Int64.Parse(m.Groups[1].Value), System.Int64.Parse(m.Groups[2].Value))
    | _ -> failwith "no matching"

let parseContent(lines: string array) =
    let extra = 10000000000000L
    let groups = Utilities.getGroupsOnSeparator (lines |> List.ofArray) ""
    groups
    |> List.map(fun g ->
        
        let (pushAX, pushAY) = getValues(g.Item(0))
        let (pushBX, pushBY) = getValues(g.Item(1))
        let (resultX, resultY) = getValues (g.Item(2))
        { ButtonA = { Name = "A"; X = pushAX; Y = pushAY }; ButtonB = { Name = "B"; X = pushBX; Y = pushBY }; ResultX = extra + resultX; ResultY = extra + resultY }
    )

// https://en.wikipedia.org/wiki/Cramer's_rule
let solveEcuation (ecuation: Combination) =
    let partA1 = (ecuation.ButtonB.Y * ecuation.ResultX - ecuation.ButtonB.X * ecuation.ResultY)
    let partA2 = (ecuation.ButtonA.X * ecuation.ButtonB.Y - ecuation.ButtonA.Y * ecuation.ButtonB.X)
    let mulA = partA1 / partA2
    let partB1 = (ecuation.ButtonA.Y * ecuation.ResultX - ecuation.ButtonA.X * ecuation.ResultY)
    let partB2 = (ecuation.ButtonA.Y * ecuation.ButtonB.X - ecuation.ButtonA.X * ecuation.ButtonB.Y)
    let mulB = partB1 / partB2
    
    let newPointA = { ecuation.ButtonA with X = ecuation.ButtonA.X * mulA; Y = ecuation.ButtonA.Y * mulA }
    let newPointB = { ecuation.ButtonB with X = ecuation.ButtonB.X * mulB; Y = ecuation.ButtonB.Y * mulB }
    let newPoint = { Name = "-"; X = newPointA.X + newPointB.X; Y = newPointA.Y + newPointB.Y }
    if newPoint.X = ecuation.ResultX && newPoint.Y = ecuation.ResultY then
        Some(mulA, mulB)
    else
        None

// Just for curiosity...
let solveZ3 (ecuation: Combination) =
    let cfg = System.Collections.Generic.Dictionary()
    cfg.Add("model", "true")
    use ctx = new Context(cfg)

    let x = ctx.MkIntConst("X")
    let y = ctx.MkIntConst("Y")

    let ax, ay = ecuation.ButtonA.X, ecuation.ButtonA.Y
    let bx, by = ecuation.ButtonB.X, ecuation.ButtonB.Y
    let rx, ry = ecuation.ResultX, ecuation.ResultY

    let eq1 = ctx.MkEq(ctx.MkAdd(ctx.MkMul(ctx.MkInt(ax), x), ctx.MkMul(ctx.MkInt(bx), y)), ctx.MkInt(rx))
    let eq2 = ctx.MkEq(ctx.MkAdd(ctx.MkMul(ctx.MkInt(ay), x), ctx.MkMul(ctx.MkInt(by), y)), ctx.MkInt(ry))

    use solver = ctx.MkSolver()
    solver.Add(eq1)
    solver.Add(eq2)

    match solver.Check() with
    | Status.SATISFIABLE ->
        let model = solver.Model
        let valueA = model.Eval(x, true).ToString() |> int64
        let valueB = model.Eval(y, true).ToString() |> int64
        Some(valueA, valueB)
    | _ -> None

let execute() =
    let path = "day13/day13_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let ecuations = parseContent content
    ecuations
    |> List.sumBy(fun e ->
        match solveEcuation e with
        | Some((aValue, bValue)) -> aValue*3L + bValue*1L
        | _ -> 0
    )