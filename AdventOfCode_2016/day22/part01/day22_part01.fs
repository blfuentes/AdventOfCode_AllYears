module day22_part01

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
            Pos = { Row = (int)(m'.Groups[1].Value); Col = (int)(m'.Groups[2].Value) };
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

let execute =
    let path = "day22/day22_input.txt"
    let content = LocalHelper.GetLinesFromFile path

    let files = parseContent content
    findValidPairs files