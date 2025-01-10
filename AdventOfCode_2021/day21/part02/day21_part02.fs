module day21_part02

open AdventOfCode_2021.Modules
open System.Collections.Generic

type Player = {
    Id: int
    Position: int
    Score: int
}

let parseContent(lines: string array) =
    lines
    |> Array.map(fun line ->
        let(i, p) = ((int)(line.Split(" ")[1]), (int)(line.Split(" ")[4]))
        { Id = i; Position = p - 1; Score = 0 }
    )

let diracDice =
    let r = [1; 2; 3;]
    r
    |> List.allPairs r
    |> List.allPairs r
    |> List.map (fun (a, (b, c)) -> a + b + c)
    |> List.countBy id

type Turn = int * int * int * int -> int64 * int64

let cache (turn : Turn) : Turn =
    let cache = Dictionary<int * int * int * int, int64 * int64>()
    fun input ->
        let exists, value = cache.TryGetValue input
        if exists then
            value
        else
            let value = turn input
            cache.Add(input, value)
            value

#nowarn "40"
let rec playWithDiracDice = cache(fun (p1, p2, s1, s2) ->
    let countWin (wins1 : int64, wins2: int64) (rollTotal : int, appears : int) =
        let p = (p1 + rollTotal) % 10
        let s = s1 + p + 1
        let (w2, w1) = playWithDiracDice (p2, p, s2, s)
        wins1 + w1 * (int64 appears), wins2 + w2 * (int64 appears)

    match s2 >= 21 with
    | true -> 0L, 1L
    | false ->
        diracDice
        |> List.fold countWin (0L, 0L))

let execute =
    let path = "day21/day21_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let players = parseContent content

    let (w1, w2) = playWithDiracDice ((players[0].Position), (players[1].Position), 0, 0)
    max w1 w2