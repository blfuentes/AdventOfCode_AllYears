List.iter (printf "%d") [0 .. 9]
List.iter 
    (printf "%d") 
        [0 .. 9]

let calculateRate(input: list<string[]>, criteria: string, noncriteria: string, op: (int -> int -> bool)) =
    let rate = [0 .. input.Head.Length - 1] 
            |> List.map(fun idx -> input 
                                |> List.filter (fun e -> e.[idx] = criteria) 
                                |> List.length)

    rate |> List.map(fun r -> if op r (input.Length - r) then criteria else noncriteria) |> List.toArray

let fakeinput = [[|"0";"0";"0";"1"|];[|"0";"0";"1";"0"|];[|"0";"0";"1";"1"|];[|"0";"1";"0";"0"|];]
let rate = calculateRate(fakeinput, "1", "0", (>=))
rate

open System
open System.IO

let path = "day04_input.txt"
let inputLines = File.ReadAllText(__SOURCE_DIRECTORY__ + @"/day04/" + path)

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

let rec roundFirstWinner (values:int list, listOfBoards: int[][] list) =
    let newBoards = listOfBoards |> List.map (fun b -> playBoard(values.Head, b))
    match newBoards |> List.filter(fun b -> IsWinningBoard(b)) with
    | [x] -> (x, values.Head)
    | _ -> roundFirstWinner(values.Tail, newBoards)

let rec roundLastWinner (values:int list, listOfBoards: int[][] list) =
    let newBoards = listOfBoards |> List.map (fun b -> playBoard(values.Head, b))
    match newBoards |> List.filter(fun b -> not(IsWinningBoard(b))) with
    | [] -> (newBoards.Head, values.Head)
    | x -> roundLastWinner(values.Tail, x)

let execute =
    let inputParts = splitStringBySeparator inputLines "\r\n\r\n" |> Array.toList
    let playingnumbers = inputParts.Head.Split([|','|]) |> Array.map int |> Array.toList
    let boardsFirstWinner = inputParts.Tail |> List.map (fun p -> buildBoard p)
    let boardsLastWinner = inputParts.Tail |> List.map (fun p -> buildBoard p)
    let firstwinningBoard = roundFirstWinner(playingnumbers, boardsFirstWinner)
    let lastwinningBoard = roundLastWinner(playingnumbers, boardsLastWinner)
    let firstwinningscore = getBoardScore(fst firstwinningBoard)
    let lastwinningscore = getBoardScore(fst lastwinningBoard)

    ((firstwinningscore*snd firstwinningBoard), (lastwinningscore*snd lastwinningBoard))