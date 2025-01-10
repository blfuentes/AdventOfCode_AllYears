open System.IO

// let path = "test_input_02.txt"
let path = "day01_input.txt"
let inputLine = File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.head |> Seq.toArray

let elements = 
    inputLine 
    |> Seq.groupBy id 
    |> Seq.map (fun (key, el) -> (key, Seq.length el))

let elements2 = 
    (inputLine |> Array.filter (fun x -> x = '(') |> Array.length , inputLine |> Array.filter (fun x -> x = ')') |> Array.length)

printfn "result %d" (fst elements2 - snd elements2)
