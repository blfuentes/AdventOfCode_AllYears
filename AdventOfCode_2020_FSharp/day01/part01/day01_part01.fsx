open System.IO

//let path = "test_input.txt"
let path = "day01_input.txt"

let inputLines = File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.map int |> Seq.toList

let rec combination num list = 
    match num, list with
    | 0, _ -> [[]]
    | _, [] -> []
    | k, (x::xs) -> List.map ((@) [x]) (combination (k-1) xs) @ combination k xs

let pairs = combination 3 inputLines

let pair2020 =  pairs |> List.find (fun ele -> List.sum ele = 2020)
let result = pair2020 |> List.fold (*) 1