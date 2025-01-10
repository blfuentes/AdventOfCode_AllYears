module day06_part02

open System.IO
open System.Linq

let filepath = __SOURCE_DIRECTORY__ + @"../../day06_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_02.txt"
let values = File.ReadAllLines(filepath)
                |> Array.map (fun x -> (x.Split(')').[0], x.Split(')').[1])) |> Array.toSeq

let endElements = values |> Seq.groupBy (fun x -> snd x) |> Seq.map fst

let rec distanceToParent(elem: string, check:string) = 
    let result = 
        values 
        |> Seq.tryFind(fun x -> (snd x) = elem && elem <> check)
        |> function 
            |Some (parent) -> 1 + distanceToParent (fst parent, check)
            |None -> 0
    result

let rec listOfParents(elem: string) =
    values 
    |> Seq.tryFind(fun x -> (snd x) = elem)
    |> function
        |Some (parent) -> seq { yield (fst parent); yield! listOfParents(fst parent)}
        |None -> Seq.empty

let findParent(elem: string) =
    values 
    |> Seq.tryFind(fun x -> (snd x) = elem) |> Option.get


let intersect (xs:'a seq) (ys: 'a seq) = xs.Intersect(ys)

let execute =
    let youOrbitParent = findParent "YOU"
    let sanOrbitParent = findParent "SAN"

    let parentsYou = listOfParents(fst youOrbitParent) |> Seq.toList
    let parentsSan = listOfParents(fst sanOrbitParent) |> Seq.toList

    let firstParent = (intersect parentsYou parentsSan).First()
    let distancefromYou = distanceToParent(fst youOrbitParent, firstParent)
    let distancefromSan = distanceToParent(fst sanOrbitParent, firstParent)
    distancefromYou + distancefromSan