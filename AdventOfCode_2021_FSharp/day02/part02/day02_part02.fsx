open System.IO
let path = "day02_input.txt"
//let path = "test_input.txt"
let inputLines = File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.map(fun line -> (line.Split(' ')[0], line.Split(' ')[1] |> int)) |> Seq.toList
let rec calculate (list, horizontal, depth, aim) =
    match list with
    | [] -> [|horizontal; depth; aim|]
    | _ -> match list.Head with
            | ("forward", v) -> calculate(list.Tail, horizontal + v, depth + (aim * v), aim)
            | ("down", v) -> calculate(list.Tail, horizontal, depth, aim + v)
            | ("up", v) -> calculate(list.Tail, horizontal, depth, aim - v)
            | _ -> [|0; 0; 0|]

let values = calculate(inputLines, 0, 0, 0)          
values.[0] * values.[1]
