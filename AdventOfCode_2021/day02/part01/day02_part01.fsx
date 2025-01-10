open System.IO

let path = "day02_input.txt"
//let path = "test_input.txt"
let inputLines = File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) 
                |> Seq.groupBy (fun line -> line.Split(' ')[0])
                |> Seq.map (fun (key, values) -> (key, values |> Seq.sumBy(fun v -> v.Split(' ')[1] |> int)))
let forward = snd (inputLines |> Seq.find((fun (k, v)-> k="forward")))
let down = snd (inputLines |> Seq.find((fun (k, v)-> k="down")))
let up = snd (inputLines |> Seq.find((fun (k, v)-> k="up")))

let result = forward * (down - up)
result