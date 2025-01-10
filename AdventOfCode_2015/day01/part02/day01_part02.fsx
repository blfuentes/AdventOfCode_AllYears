open System.IO

// let path = "test_input_03.txt"
let path = "day01_input.txt"
let inputline = File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.head |> Seq.toArray

let calculateFloor (checkFloor:int) = 
    let elements2 = (inputline |> Seq.take(checkFloor) |> Seq.filter (fun x -> x = '(') |> Seq.length , inputline |> Seq.take(checkFloor) |> Seq.filter (fun x -> x = ')') |> Seq.length)
    fst elements2 - snd elements2

let numOfMovs = inputline |> Array.length
// calculateFloor 7000

let execute =
    1 + ([0..numOfMovs] |> List.takeWhile(fun x -> (calculateFloor x) >= 0) |> List.rev |> List.head)