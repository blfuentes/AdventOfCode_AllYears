open System.IO

let path = "day01_input.txt"
//let path = "test_input.txt"
let inputLines = File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.map int |> Seq.toList

let rec getSubListBySize (num, list: 'a list) : 'a list list= 
    match num <= list.Length with
    | true -> [(list |> List.take num)] @ (getSubListBySize(num, (list.Tail)))
    | false -> []

let groupedByThree = getSubListBySize(3, inputLines) |> List.map (List.sum)
groupedByThree |> List.pairwise |> List.filter (fun (x,y) -> y > x) |> List.length