module day02_part02

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
//        let rec hasRepeated (current: string) (tomatch: string) =
//            if current.Length = tomatch.Length || current.Length = 0 then
//                false
//            else
//                let repeats = tomatch.Length / current.Length
//                let expected = String.replicate repeats current
//                if expected = tomatch then
//                    true
//                else
//                    hasRepeated (current.Substring(0, current.Length - 1)) tomatch

//        hasRepeated (stringyId.Substring(0, stringyId.Length / 2)) stringyId
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
        
        let rec hasRepeated (patternLen: int) =
            if patternLen > len / 2 then
                false
            elif len % patternLen <> 0 then
                hasRepeated (patternLen + 1)
            else
                let rec checkRepeats offset =
                    if offset >= len then true
                    elif offset + patternLen > len then false
                    else
                        let rec compareChars i =
                            if i >= patternLen then true
                            elif stringyId[i] <> stringyId[offset + i] then false
                            else compareChars (i + 1)
                        
                        if compareChars 0 then
                            checkRepeats (offset + patternLen)
                        else
                            false
                
                if checkRepeats patternLen then true
                else hasRepeated (patternLen + 1)
        
        hasRepeated 1
    
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
