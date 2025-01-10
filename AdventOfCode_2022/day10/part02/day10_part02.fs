module day10_part02

open AdventOfCode_2022.Modules.LocalHelper

let path = "day10/day10_input.txt"

let rec runOp (op: int * int) (currentValue: int) (listOfCycles: int list) =
    match fst op > 1 with
    | false -> 
        let lastValue = currentValue + snd op
        (lastValue, listOfCycles @ [lastValue])
    | true -> 
        let newListOfCycles = listOfCycles @ [currentValue]
        runOp ((fst op) - 1, snd op) currentValue newListOfCycles

let rec performInstructions (ins: (int * int) list) (currentValue: int) (listOfCycles: int list)=
    match ins with
    | head :: tail ->
        let (newCurrentValue, newListOfCycles) = runOp head currentValue listOfCycles
        performInstructions tail newCurrentValue newListOfCycles
    | [] -> listOfCycles

let getLitPixels (values: int list) =
    let chunks = values |> List.chunkBySize 40
    seq {
        for chunk in chunks do
           yield (chunk |> List.mapi(fun idx v -> if v = idx || v = (idx - 1) || v = (idx + 1) then "#" else "."))
    }  

let displayCRT (output: string list seq) =
    for line in output do
        printfn "%s" (System.String.Concat(line))

let execute =
    let inputLines = GetLinesFromFile(path)
    let instructions = inputLines |> Array.map(fun l -> if l.Split(' ').[0] = "noop" then (1, 0) else (2, (int)(l.Split(' ').[1]))) |> Array.toList
    let initValue = 1
    let initlistOfCycles = [1]
    let result = performInstructions instructions initValue initlistOfCycles
    displayCRT (getLitPixels result)
