module day13_part01

open System.IO

open AdventOfCode_Utilities

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

let rec fold(myboard: string[,], folds: (string * int) list) =
    match folds with
    | [] -> myboard
    | x::xs -> 
        let folded = foldBoard(myboard, x)
        fold(folded, xs)

let execute =
    let path = "day13_input.txt"

    let instructionsParts = 
        File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.toList
    let parts = instructionsParts |> List.splitAt(instructionsParts |> List.findIndex(fun l -> l = ""))
    let coordinates = (fst parts) |> List.map(fun c -> [|c.Split(',').[0] |> int; c.Split(',').[1] |> int|])
    let folding = (snd parts).Tail |> List.map(fun i -> (i.Split('=').[0].Substring(i.Split('=').[0].Length - 1, 1), i.Split('=').[1] |> int))

    let board = createBoard coordinates
    let firstFolding = foldBoard(board, folding.Item(0))
    let tBoard1 = Utilities.toJagged firstFolding
    (tBoard1 |> Array.map(fun e -> e |> Array.filter(fun ee -> ee = "#") |> Array.length)) |> Array.sum   