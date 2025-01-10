open System.IO
open System.Linq

let filepath = __SOURCE_DIRECTORY__ + @"../../day06_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_02.txt"
let values = File.ReadAllLines(filepath)
                |> Array.map (fun x -> (x.Split(')').[0], x.Split(')').[1])) |> Array.toSeq

let endElements = values |> Seq.groupBy (fun x -> snd x) |> Seq.map fst

let rec numberOfParents(elem: string) =
    let result = 
        values 
        |> Seq.tryFind(fun x -> (snd x) = elem)
        |> function 
            |Some (parent) -> 1 + numberOfParents (fst parent)
            |None -> 0
    result

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
    |> Seq.tryFind(fun x -> (snd x) = elem)

let youOrbitParent = findParent "YOU"
let sanOrbitParent = findParent "SAN"

let parentsYou = listOfParents("BSW") |> Seq.toList
let parentsSan = listOfParents("H7F") |> Seq.toList

let intersect (xs:'a seq) (ys: 'a seq) = xs.Intersect(ys)

let firstParent = (intersect parentsYou parentsSan).First()
let distancefromYou = distanceToParent("BSW", "P2L")
let distancefromSan = distanceToParent("H7F", "P2L")

endElements |> Seq.sumBy (fun x -> numberOfParents x)

Seq.iter 
    (fun x -> printfn "element %s has %d orbits" x (numberOfParents x))

printfn "%A" endElements
