module day25_part01

open System.Collections.Generic
open AdventOfCode_Utilities
open AdventOfCode_2024.Modules

let parseContent(lines: string) =
    let schematics = Dictionary<bool, int array array>()

    let parts = lines.Split("\r\n\r\n")
    parts
    |> Array.map(fun p ->
        let lines = p.Split("\r\n")
        let (maxrows, maxcols) = (lines.Length, lines[0].Length)
        let isLock = lines[0] = "#####" && lines[maxrows-1] = "....."
        let isKey = lines[0] = "....." && lines[maxrows-1] = "#####"
        let currentHeight = Array.init maxcols (fun _ -> if isLock then 0 else maxrows - 1)

        for row in 0..maxrows-1 do
            for col in 0..maxcols-1 do
                let value = lines[row][col]
                match value with
                | '#' when isLock -> currentHeight[col] <- currentHeight[col] + if row = 0 then 0 else 1
                | '.' when isKey  -> currentHeight[col] <- currentHeight[col] - 1
                | _ -> ()

        (isLock, currentHeight)
    )
    |> Array.groupBy fst
    |> Array.iter(fun (k, content) -> 
        schematics.Add(k, content |> Array.map snd)
    )
    schematics

let calculateFits ((locks, keys): (int array array*int array array)) =
    let isFit((lock, key): int array * int array) =
        Array.zip lock key
        |> Array.forall(fun (l, k) -> l + k < 6)

    let combinations = Utilities.generateCombinations (locks |> List.ofArray) (keys |> List.ofArray)
    combinations
    |> List.sumBy(fun c ->
        if isFit c then 1 else 0
    )


let execute() =
    let path = "day25/day25_input.txt"

    let content = LocalHelper.GetContentFromFile path
    let currentHeights = parseContent content
    let (locks, keys) = (currentHeights[true], currentHeights[false])

    calculateFits (locks, keys)