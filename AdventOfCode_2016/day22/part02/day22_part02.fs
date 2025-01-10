module day22_part02

open System.Text.RegularExpressions

open AdventOfCode_2016.Modules
open AdventOfCode_Utilities

type FilePos = {
    Row: int
    Col: int
}

type FileType = {
    Pos: FilePos
    Size: int
    Used: int
    Avail: int
    Use: int
}

let parseContent(files: string array) =
    files
    |> Array.skip(2)
    |> Array.map(fun f ->
        let regexpattern = @"(\d+).*?(\d+).*?(\d+).*?(\d+).*?(\d+).*?(\d+)"
        let m' = Regex.Match(f, regexpattern)
        {
            Pos = { Row = (int)(m'.Groups[2].Value); Col = (int)(m'.Groups[1].Value) };
            Size = (int)(m'.Groups[3].Value);
            Used = (int)(m'.Groups[4].Value);
            Avail = (int)(m'.Groups[5].Value);
            Use = (int)(m'.Groups[6].Value)
        }
    )

let findValidPairs(files: FileType array) =
    let possibleComb = 
        Utilities.comb 2 (files |> List.ofArray)
        |> List.filter(fun f ->
            (f.Item(0).Used > 0 &&
            f.Item(1).Avail >= f.Item(0).Used) 
            ||
            (f.Item(1).Used > 0 &&
            f.Item(0).Avail >= f.Item(1).Used) 
        )
    possibleComb.Length


let buildFileMap(files: FileType array) =
    let emptyFile = files |> Array.find(fun f -> f.Used = 0)
    let walls = files |> Array.filter(fun f -> f.Use > 90)
    let maxCol = files |> Array.maxBy(fun f -> f.Pos.Col)
    let maxRow = files |> Array.maxBy(fun f -> f.Pos.Row)

    let filemap = Array2D.create (maxRow.Pos.Row+1) (maxCol.Pos.Col+1) "."
    //let maxRows = filemap.GetLength(0)
    //let maxCols = filemap.GetLength(1)

    files
    |> Array.iter(fun f ->
        if f.Use > 90 then
            filemap[f.Pos.Row, f.Pos.Col] <- "#"
        elif f.Used = 0 then
            filemap[f.Pos.Row, f.Pos.Col] <- "_"
    )
    //for row in 0..maxRows-1 do
    //    for col in 0..maxCols-1 do
    //        printf "%s" filemap[row, col]
    //    printfn ""

    // resolved with observation of the map
    let mostleftwall = walls |> Array.minBy(fun w -> w.Pos.Col)
    let movesfromempty = (emptyFile.Pos.Col -  mostleftwall.Pos.Col) + 1 // required to surpass the wall
    let movestotop = emptyFile.Pos.Row
    let movestogoal = maxCol.Pos.Col - mostleftwall.Pos.Col
    // https://en.wikipedia.org/wiki/15_puzzle 
    // 6 movements to replace one position with another
    // repeat X times. where X = topright col - 1
    let reps = maxCol.Pos.Col - 1 
    reps * 5 + movesfromempty + movestotop + movestogoal + 1 // extra for (0,0)

let execute =
    let path = "day22/day22_input.txt"
    let content = LocalHelper.GetLinesFromFile path

    let files = parseContent content
    buildFileMap files