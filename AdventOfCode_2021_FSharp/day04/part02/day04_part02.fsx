open System
open System.IO

//let path = "day04_input.txt"
let path = "test_input.txt"
let inputLines = File.ReadAllText(__SOURCE_DIRECTORY__ + @"../../" + path)

let splitStringBySeparator (content: string) (separator: string) =
    content.Split([|separator|], StringSplitOptions.None)

let buildBoard (content: string) =
    let rows = splitStringBySeparator content "\r\n"
    rows |> Array.map (fun r -> r.Split(' ') |> Array.filter(fun r -> r <> "") |> Array.map int)

let playBoard(value:int, board: int[][]) =
    board |> Array.map(fun b -> b |> Array.map(fun bb -> if bb = value then -1 else bb))

let IsWinningBoard(board: int[][]) =
    (board |> Array.exists(fun r -> r |> Array.forall(fun a -> a = -1))) || 
    [0..board.[0].Length - 1] |> List.exists(fun i -> (board |> Array.forall(fun b -> b.[i] = -1)))

let getBoardScore(board: int[][]) =
    board |> Array.sumBy(fun x -> x |> Array.filter(fun v -> v <> -1) |> Array.sum)

let rec round (values:int list, listOfBoards: int[][] list) =
    let newBoards = listOfBoards |> List.map (fun b -> playBoard(values.Head, b))
    match newBoards |> List.filter(fun b -> not(IsWinningBoard(b))) with
    | [] -> (newBoards.Head, values.Head)
    | x -> round(values.Tail, x)

let inputParts = splitStringBySeparator inputLines "\r\n\r\n" |> Array.toList
let playingnumbers = inputParts.Head.Split([|','|]) |> Array.map int |> Array.toList
let boards = inputParts.Tail |> List.map (fun p -> buildBoard p)

let winningBoard = round(playingnumbers, boards)
let score = getBoardScore(fst winningBoard)

printfn "Found winning board with score %i in value %i. Result %i:" score (snd winningBoard) (score*snd winningBoard)
