module day05_part02

open AdventOfCode_2025.Modules

let parseContent(lines: string) =
    let parts = lines.Split($"{System.Environment.NewLine}{System.Environment.NewLine}")
    let ranges =
        parts[0].Split(System.Environment.NewLine)
        |> Array.map(fun line ->
            (bigint.Parse(line.Split("-")[0]), bigint.Parse(line.Split("-")[1])))
        
    let ids = 
        parts[1].Split(System.Environment.NewLine)
        |> Array.map(fun line -> bigint.Parse(line))
    (ranges, ids)        

let countFreshRanges(ranges: (bigint * bigint)[]) =
    let sortedRanges = ranges |> Array.sortBy fst
    let rec mergeRanges (acc: (bigint * bigint) list) (toProcess: (bigint * bigint)[]) =
        match toProcess with
        | [||] -> acc |> List.rev |> List.toArray
        | [|single|] -> (single :: acc) |> List.rev |> List.toArray
        | _ ->
            let (min1, max1) = toProcess[0]
            let (min2, max2) = toProcess[1]
            if max1 >= min2 then
                let newRange = (min1, if max1 > max2 then max1 else max2)
                let newToProcess = Array.append [|newRange|] (toProcess.[2..])
                mergeRanges acc newToProcess
            else
                mergeRanges (toProcess[0] :: acc) (toProcess.[1..])
    let mergedRanges = mergeRanges [] sortedRanges
    mergedRanges 
    |> Array.sumBy(fun (min, max) -> (max - min)+1I)

let execute() =
    //let path = "day05/test_input_05.txt"
    let path = "day05/day05_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let (ranges, ids) = parseContent content
    countFreshRanges ranges