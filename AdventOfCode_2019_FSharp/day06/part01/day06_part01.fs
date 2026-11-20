module day06_part01

open System.IO

let filepath = __SOURCE_DIRECTORY__ + @"../../day06_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input.txt"
let values = File.ReadAllLines(filepath)
                |> Array.map (fun x -> (x.Split(')').[0], x.Split(')').[1])) |> Array.toSeq

let endElements = values |> Seq.groupBy (fun x -> snd x) |> Seq.map fst

let rec numberOfParents(elem: string) =
    let result = 
        values 
        |> Seq.tryFind(fun x -> (snd x) = elem)
        |> function 
            |Some(parent) -> 1 + numberOfParents (fst parent)
            |None -> 0
    result

let execute =
    endElements |> Seq.sumBy (fun x -> numberOfParents x)