open System
open System.IO

let input = File.ReadAllText(__SOURCE_DIRECTORY__ + "/day00_input.txt")
let input2= File.ReadAllLines(__SOURCE_DIRECTORY__ + "/day00_input.txt") |> Array.toList