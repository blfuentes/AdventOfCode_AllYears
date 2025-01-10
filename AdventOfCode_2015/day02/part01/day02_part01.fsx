open System.IO

// let path = "test_input_01.txt"
// let path = "test_input_02.txt"
let path = "day02_input.txt"
let inputLine = File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.map(fun x -> x.Split('x') |> Array.map int)

let calculatePaper(d: int[]) =
    // 2*l*w  2*w*h + 2*h*l (l - w - h)
    let smallest = d |> Array.sort |> Array.take(2) |> Array.fold (*) 1
    (2 * d.[0] * d.[1] + 2 * d.[1] * d.[2] + 2 * d.[2] * d.[0]) + smallest

inputLine |> Seq.sumBy(fun p -> calculatePaper(p))