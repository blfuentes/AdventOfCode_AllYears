module day05_part01

open System.IO
open System.Text.RegularExpressions

let path = "day05_input.txt"
//let path = "test_input.txt"
let regexPattern = "([0-9]+),([0-9]+) -> ([0-9]+),([0-9]+)"

let getPoints input pattern =
    let m = Regex.Match(input, pattern)
    match m.Success with
    | true-> ([|m.Groups[1].Value |> int; m.Groups[2].Value |> int|], [|m.Groups[3].Value |> int; m.Groups[4].Value |> int|])
    | false -> ([|-1; -1|], [|-1; -1|])

let board _minRow _maxRow _minCol _maxCol : int[,]=
    Array2D.zeroCreate (_maxCol + 1) (_maxRow + 1)

let listOfPoints (cloud:int[]*int[]) =
    let diffX = (snd cloud).[0] - (fst cloud).[0]
    let diffY = (snd cloud).[1] - (fst cloud).[1]
    let numberOfPoints = if (fst cloud).[0] = (snd cloud).[0] then abs((fst cloud).[1] - (snd cloud).[1]) else abs((fst cloud).[0] - (snd cloud).[0])
        
    let intervalX = diffX / numberOfPoints
    let intervalY = diffY / numberOfPoints
    let result = [|0 .. numberOfPoints |] |> Array.map (fun p -> [|((fst cloud).[0] + (intervalX * p));((fst cloud).[1] + (intervalY * p))|])
    result

let markCloud (cloud:int[]*int[]) (field:int[,]) =    
    let points = listOfPoints cloud
    points |> Array.iter (fun p -> field[p.[1], p.[0]] <- field[p.[1], p.[0]] + 1)

let toListOfLists (arr2d: 'a [,]) = [ yield! [arr2d.GetLowerBound(0)..arr2d.GetUpperBound(0)] |> List.map (fun i -> arr2d.[i,*] |> List.ofArray) ]

let execute =
    let inputLines = File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.map (fun line -> getPoints line regexPattern) |> Seq.toList
    let minCol = inputLines |> List.find(fun l -> inputLines 
                                                |> List.forall(fun l2 -> ((fst l).[1] <= (fst l2).[1] && (fst l).[1] <= (snd l2).[1]) ||
                                                                         ((snd l).[1] <= (snd l2).[1] && (snd l).[1] <= (fst l2).[1])))
                |> (fun x -> if (fst x).[1] <= (snd x).[1] then (fst x).[1] else (snd x).[1])
    let maxCol = inputLines |> List.find(fun l -> inputLines
                                                |> List.forall(fun l2 -> ((fst l).[1] >= (fst l2).[1] && (fst l).[1] >= (snd l2).[1]) ||
                                                                         ((snd l).[1] >= (snd l2).[1] && (snd l).[1] >= (fst l2).[1])))
                |> (fun x -> if (fst x).[1] >= (snd x).[1] then (fst x).[1] else (snd x).[1])
    let minRow = inputLines |> List.find(fun l -> inputLines
                                                |> List.forall(fun l2 -> ((fst l).[0] <= (fst l2).[0] && (fst l).[0] <= (snd l2).[0]) ||
                                                                         ((snd l).[0] <= (snd l2).[0] && (snd l).[0] <= (fst l2).[0])))
                |> (fun x -> if (fst x).[0] <= (snd x).[0] then (fst x).[0] else (snd x).[0])
    let maxRow = inputLines |> List.find(fun l -> inputLines
                                                |> List.forall(fun l2 -> ((fst l).[0] >= (fst l2).[0] && (fst l).[0] >= (snd l2).[0]) ||
                                                                         ((snd l).[0] >= (snd l2).[0] && (snd l).[0] >= (fst l2).[0])))
                |> (fun x -> if (fst x).[0] >= (snd x).[0] then (fst x).[0] else (snd x).[0])

    let myfield = board minRow maxRow minCol maxCol
    inputLines |> List.filter(fun l -> (fst l).[0] = (snd l).[0] || (fst l).[1] = (snd l).[1]) |> List.iter (fun c -> markCloud c myfield)
    toListOfLists myfield |> List.sumBy(fun x -> x |> List.sumBy(fun c -> if c > 1 then 1 else 0))