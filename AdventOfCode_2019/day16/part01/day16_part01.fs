module day16_part01

open System
open System.IO

let filepath = __SOURCE_DIRECTORY__ + @"../../day16_input.txt"

let input = File.ReadAllText(filepath).ToCharArray() |> Array.map (string >> int)
let basePattern = [| 0; 1; 0; -1 |]

let generatePattern2(length: int, position: int, basePattern: int[]) =
    let tmpPattern = basePattern |> Array.map (fun x -> Array.create (position + 1) x) |> Array.fold Array.append Array.empty<int>
    let pattern = seq { for x in [| 0 .. length |] do yield tmpPattern.[x % tmpPattern.Length] } |> Seq.toArray
    Seq.take length (pattern |> Seq.skip(1)) |> Seq.toArray

let calculateInput(input:int[], pattern:int[]) =
    let result = seq {
        for x in [ 0 .. input.Length - 1 ] do
            let compPattern = generatePattern2(input.Length, x, pattern)
            let tmpResult = Array.map2 (*) input compPattern |> Array.sum |> fun x -> Math.Abs(x % 10)
            yield tmpResult
        }
    result |> Seq.toArray

let rec convertInput(input:int[], basePattern: int[], numberOfPhases: int, currentPhase: int) =
    match currentPhase = numberOfPhases with
    | true -> input |> Array.take(8) |> Array.map string |> String.concat "" |> int
    | false -> convertInput(calculateInput(input, basePattern), basePattern, numberOfPhases, currentPhase + 1)


let execute =
    convertInput(input, basePattern, 100, 0)