module day02_part01

open System.IO

let filepath = __SOURCE_DIRECTORY__ + @"../../day02_input.txt"
let lines = File.ReadLines(filepath)

let getTupleComposition(input: string) =  
    let counted = input |> Seq.countBy id 
    let existsTwo = counted |> Seq.exists (fun (x, y) -> y = 2)
    let existsThree = counted |> Seq.exists (fun (x, y) -> y = 3)
    let tupleresult = (existsTwo, existsThree)
    tupleresult

let finalValue =
    let tmplist = lines |> Seq.map (fun line -> getTupleComposition line) |> List.ofSeq
    let numberOfTwos = tmplist |> List.filter (fun (two, three) -> two = true) |> List.length
    let numberOfThrees = tmplist |> List.filter (fun (two, three) -> three = true) |> List.length
    numberOfTwos * numberOfThrees