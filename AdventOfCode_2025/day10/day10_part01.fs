module day10_part01

open AdventOfCode_2025.Modules
open System.Text.RegularExpressions

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

let rec combinationWithRepetition (num: int) (list: 'a list) : 'a list list = 
    match num, list with
    | 0, _ -> [[]]
    | _, [] -> []
    | k, (x::xs) -> 
        List.map ((@) [x]) (combinationWithRepetition (k-1) list) 
        @ (combinationWithRepetition k xs)

let findCombination (machine: Machine) =
    let applyButton (state: bool array) (buttonIndices: int array) =
        let newState = Array.copy state
        for idx in buttonIndices do
            if idx >= 0 && idx < newState.Length then
                newState[idx] <- not newState[idx]
        newState
    
    let buttonIndices = [0 .. (Seq.length machine.Buttons) - 1]
    let initialState = Array.zeroCreate<bool> machine.LightDiagram.Length
    
    Seq.initInfinite (fun pressCount ->
        combinationWithRepetition pressCount buttonIndices
    )
    |> Seq.concat 
    |> Seq.map (fun buttonSequence ->
        let finalState = 
            buttonSequence 
            |> List.fold (fun state btnIdx -> 
                applyButton state (machine.Buttons |> Seq.item btnIdx)
            ) initialState
        (buttonSequence.Length, finalState)
    )
    |> Seq.find (fun (count, state) -> state = machine.LightDiagram)
    |> fst

let getButtonPresses (machines: Machine array) =
    machines |> Array.sumBy findCombination

let execute() =
    //let path = "day10/test_input_10.txt"
    let path = "day10/day10_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let machines = parseContent content
    getButtonPresses machines
