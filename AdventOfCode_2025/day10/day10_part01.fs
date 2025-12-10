module day10_part01

open AdventOfCode_2025.Modules
open System.Text.RegularExpressions

type Machine = {
    Id: int;
    LightDiagram: uint64
    Buttons: uint64 array
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
            |> Array.mapi (fun i c -> if c = '#' then 1UL <<< i else 0UL)
            |> Array.fold (|||) 0UL
        
        let buttons =
            parts[1..parts.Length-2]
            |> Array.map (fun btnStr ->
                Regex.Match(btnStr, @"\(([^)]*)\)").Groups[1].Value.Split(",")
                |> Array.map int
                |> Array.map (fun idx -> 1UL <<< idx)
                |> Array.fold (|||) 0UL
            )
        
        let joltages = 
            Regex.Match(parts[parts.Length-1], @"{([^>]*)}").Groups[1].Value.Split(",")
            |> Array.map int
        
        {
            Id = id;
            LightDiagram = lightDiagram;
            Buttons = buttons;
            Joltages = joltages
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
    let applyButton (state: uint64) (buttonMask: uint64) =
        state ^^^ buttonMask  // apply XOR
    
    let buttonIndices = [0 .. machine.Buttons.Length - 1]
    let initialState = 0UL
    
    Seq.initInfinite (fun pressCount ->
        combinationWithRepetition pressCount buttonIndices
    )
    |> Seq.concat 
    |> Seq.map (fun buttonSequence ->
        let finalState = 
            buttonSequence 
            |> List.fold (fun state btnIdx -> 
                applyButton state machine.Buttons.[btnIdx]
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
