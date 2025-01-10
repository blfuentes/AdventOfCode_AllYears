open System
open System.IO
open System.Text.RegularExpressions

let path = "day09_input.txt"
//let path = "test_input.txt"
//let path = "test_input_00.txt"

let inputPartsCollection = 
    File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.map(fun l -> l.ToCharArray() |> Array.map string |> Array.map int) |> Seq.toArray

let columns = inputPartsCollection.[0].Length
let rows = inputPartsCollection.Length

let topbottomArray = [|Array.init(columns + 2)(fun i -> 9)|]
let adaptedinput = inputPartsCollection |> Array.map(fun r -> Array.concat[[|9|]; r; [|9|]])
let paddedArray = Array.concat([topbottomArray; adaptedinput; topbottomArray])

let rec findBasisSize(map: int[][], pos:int[]) =
    let element = map.[pos.[0]][pos.[1]]
    match element = 9 with
    |true -> 0
    |false ->
        map.[pos[0]][pos.[1]] <- 9
        1 + 
            findBasisSize(map, [|pos.[0] - 1; pos.[1]|]) + 
            findBasisSize(map, [|pos.[0]; pos.[1] - 1|]) + 
            findBasisSize(map, [|pos.[0]; pos.[1] + 1|]) + 
            findBasisSize(map, [|pos.[0] + 1; pos.[1]|])

let findBasis(map: int[][]) =
    let basins = seq {
        for r in [1 .. map.Length - 1] do
            for c in [ 1 .. map.[0].Length - 1] do
                if map.[r].[c] <> 9 then 
                    yield! [findBasisSize(map, [|r; c|])]
    }
    basins

let final = findBasis(paddedArray) |> Seq.sortByDescending(fun i -> i) |> Seq.take(3) |> Seq.fold (*) 1
    
