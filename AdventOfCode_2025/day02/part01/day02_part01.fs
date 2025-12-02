module day02_part01

open AdventOfCode_2025.Modules
open System

let parseContent (lines: string) =
    let parts = lines.Split(",")
    parts
    |> Array.map(fun part ->
        (bigint.Parse(part.Split("-")[0]), bigint.Parse(part.Split("-")[1]))    
    )

// slower...
//let coundInvalidIds(ranges: (bigint * bigint) array) =
//    let notValid(id: bigint) =
//        let stringyId = id.ToString()
//        let firstPart = stringyId.Substring(0, stringyId.Length / 2)
//        let secondPart = stringyId.Substring(stringyId.Length / 2)
//        firstPart = secondPart
//    ranges
//    |> Seq.collect(fun (start, end') ->
//        [start..end']
//        |> Seq.filter(fun id -> notValid(id))
//    )
//    |> Seq.sum

let coundInvalidIds(ranges: (bigint * bigint) array) =
    let notValid(id: bigint) =
        let stringyId = id.ToString()
        let len = stringyId.Length
        
        if len % 2 <> 0 then false
        else
            let halfLen = len / 2
            let span = stringyId.AsSpan()
            let firstHalf = span.Slice(0, halfLen)
            let secondHalf = span.Slice(halfLen, halfLen)
            firstHalf.SequenceEqual(secondHalf)
    
    ranges
    |> Seq.collect(fun (start, end') ->
        seq { start..end' }
        |> Seq.filter(fun id -> notValid(id))
    )
    |> Seq.sum

let execute() =
    //let path = "day02/test_input_02.txt"
    let path = "day02/day02_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let ranges = parseContent content
    coundInvalidIds ranges