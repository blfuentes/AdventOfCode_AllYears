open System.IO

let filepath = __SOURCE_DIRECTORY__ + @"../../day08_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input.txt"

let width = 25
let height = 6

let values = File.ReadAllText(filepath).ToCharArray() |> Array.map string |> Array.map int |> Array.toList
let layers = values |> List.chunkBySize(width * height)

let fewestZerosList = layers |> List.maxBy(fun _list -> _list |> List.countBy(fun x -> x <> 0))
let numberOfOnes = fewestZerosList |> List.filter(fun _x -> _x = 1) |> List.length
let numberOfTwos = fewestZerosList |> List.filter(fun _x -> _x = 2) |> List.length

let result = numberOfOnes * numberOfTwos