open System.IO

let filepath = __SOURCE_DIRECTORY__ + @"../../day01_part01_input.txt"
let lines = File.ReadLines(filepath)

let displayValue =
    lines
        |> Seq.map int
        |> Seq.sum 

displayValue