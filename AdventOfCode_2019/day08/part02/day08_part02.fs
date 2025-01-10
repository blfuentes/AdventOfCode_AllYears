module day08_part02
open System.IO

let filepath = __SOURCE_DIRECTORY__ + @"../../day08_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input.txt"

let width = 25
let height = 6

let values = File.ReadAllText(filepath).ToCharArray() |> Array.map string |> Array.map int
let chunkSize = width * height
let layers = values |> Array.chunkBySize(chunkSize)

let findFirstColorValue(position: int) =
    let layersValues = layers |> Array.map (fun x -> x.[position])
    layersValues |> Array.find (fun v -> v <> 2)

let result = 
    let colors = seq {
        for idx in [|0..chunkSize - 1|] do
            match (findFirstColorValue idx) with
            | 0 -> yield ' '
            | _ -> yield char 35 
    }
    colors |> Seq.toList

let execute =
    for jdx in [|0..result.Length|] do
        match jdx % width with
        | 0 -> printf "%s" System.Environment.NewLine
        | _ -> printf "%c" result.[jdx]