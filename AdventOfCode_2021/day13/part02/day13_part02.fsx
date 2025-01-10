open System
open System.IO
open System.Text.RegularExpressions
open System.Collections.Generic

let path = "day13_input.txt"
//let path = "day13_input_test.txt"
//let path = "test_input.txt"
//let path = "test_input_00.txt"
//let path = "test_input_01.txt"

let instructionsParts = 
    File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.toList

let parts = instructionsParts |> List.splitAt(instructionsParts |> List.findIndex(fun l -> l = ""))
let coordinates = (fst parts) |> List.map(fun c -> [|c.Split(',').[0] |> int; c.Split(',').[1] |> int|])
let folding = (snd parts).Tail |> List.map(fun i -> (i.Split('=').[0].Substring(i.Split('=').[0].Length - 1, 1), i.Split('=').[1] |> int))

let createBoard(coords: int[] list) =
    let maxY = 1500//((coords |> List.sortByDescending(fun c -> c.[1])) |> List.head).[1] + 1
    let maxX = 1500//((coords |> List.sortByDescending(fun c -> c.[0])) |> List.head).[0] + 1
    let myboard = Array2D.create maxY maxX "."
    coords |> List.iter(fun c -> myboard[c.[1], c.[0]] <- "#")
    myboard

let foldBoard(myboard: string[,], fold: string * int) =
    let newBoard = 
        match (fst fold) with
        | "y" -> 
            Array2D.create (snd fold) (myboard.GetLength 1) ""
        | "x" -> 
            Array2D.create (myboard.GetLength 0) (snd fold) ""
        | _ -> Array2D.create 0 0 ""
    for row in [0..(newBoard.GetLength(0) - 1)] do
        for col in [0..(newBoard.GetLength(1) - 1)] do
            //myboard[row, col] <-
            //    match (fst fold) with
            //    | "y" -> if row = (snd fold) then "-" else myboard[row, col]
            //    | "x" -> if col = (snd fold) then "-" else myboard[row, col]
            //    | _ -> ""
            let mirrored = 
                match (fst fold) with
                | "y" -> 
                    let mRow = (snd fold) * 2  - row
                    let mCol = col
                    myboard[mRow, mCol]
                | "x" -> 
                    let mRow = row
                    let mCol = (snd fold) * 2 - col
                    myboard[mRow, mCol]
                | _ -> ""
            newBoard[row, col] <- 
                if mirrored = "#" then 
                    mirrored 
                else 
                    myboard[row, col]
    newBoard

let toJagged<'a> (arr: 'a[,]) : 'a[][] =
    [|for x in 0..Array2D.length1 arr - 1 do
        yield [| for y in 0 ..Array2D.length2 arr - 1 -> arr.[x, y] |]|]

let rec fold(myboard: string[,], folds: (string * int) list) =
    match folds with
    | [] -> myboard
    | x::xs -> 
        let folded = foldBoard(myboard, x)
        fold(folded, xs)

let board = createBoard coordinates

//let firstFolding = foldBoard(board, folding.Item(0))
//let tBoard1 = toJagged firstFolding
//let numberOfDots1 = (tBoard1 |> Array.map(fun e -> e |> Array.filter(fun ee -> ee = "#") |> Array.length)) |> Array.sum   
//numberOfDots1
//for row in [0..firstFolding.GetLength(0) - 1] do
//    for col in [0..firstFolding.GetLength(1) - 1] do
//        printf "%s" firstFolding[row, col]
//    printfn ""


//let secondFolding = foldBoard(firstFolding, folding.Item(1))
//let tBoard2 = toJagged secondFolding
//let numberOfDots2 = (tBoard2 |> Array.map(fun e -> e |> Array.filter(fun ee -> ee = "#") |> Array.length)) |> Array.sum   
//numberOfDots2
//for row in [0..secondFolding.GetLength(0) - 1] do
//    for col in [0..secondFolding.GetLength(1) - 1] do
//        printf "%s" secondFolding[row, col]
//    printfn ""

let finalBoard = fold(board, folding)
//let tBoardf = toJagged finalBoard
//let tBoard = toJagged secondFolding
//let numberOfDotsf = (tBoardf |> Array.map(fun e -> e |> Array.filter(fun ee -> ee = "#") |> Array.length)) |> Array.sum   
//numberOfDotsf
for row in [0..finalBoard.GetLength(0) - 1] do
    for col in [0..finalBoard.GetLength(1) - 1] do
        printf "%s" (if finalBoard[row, col] = "#" then finalBoard[row, col] else " ")
    printfn ""