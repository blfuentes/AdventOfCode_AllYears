module day01_part01

open System.IO

let filepath = __SOURCE_DIRECTORY__ + @"../../day01_input.txt"
let lines = File.ReadLines(filepath)

let displayValue =
    lines
        |> Seq.map (fun mass -> int mass / 3 - 2)
        |> Seq.sum 
