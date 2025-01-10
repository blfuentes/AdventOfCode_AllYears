module day17_part01

open Intcode
open AdventOfCode_2019.Modules
open AdventOfCode_Utilities

let progOutputToGrid = List.map char >> charsToStr >> splitByFn "\n" (Array.map Seq.toArray)

let getGrid intcode =
    match run (Computer.create intcode) with
    | Output (output, _) -> progOutputToGrid output
    | _ -> failwith "Expected an output"

let neighbours (x, y) = [| (x + 1, y); (x - 1, y); (x, y + 1); (x, y - 1) |]

let sumAligments (intcode) =
    let g = getGrid intcode
    
    let getAt (x, y) = Array.tryItem y g |> Option.bind (Array.tryItem x) |> Option.defaultValue '.'
    let isScaffold (x, y) = getAt (x, y) = '#'
    let isIntersection pos =
        isScaffold pos && (neighbours pos |> Array.exists (isScaffold >> not) |> not)

    seq {
        for y = 1 to g.Length - 2 do
            for x = 1 to g.[0].Length - 2 do
                if isIntersection (x, y) then
                    x * y } |> Seq.sum

let execute =
    let path = "day17/day17_input.txt"
    let content = (LocalHelper.GetContentFromFile path).Split(",") |> Array.map int64
    sumAligments content