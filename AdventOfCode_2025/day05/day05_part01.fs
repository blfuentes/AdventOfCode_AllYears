module day05_part01

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

let countFreshIds(ranges: (bigint * bigint)[], ids: bigint[]) =
    ids
    |> Array.filter(fun id ->
        (ranges |> Array.exists(fun (min, max) -> id >= min && id <= max)))
    |> Array.length

let execute() =
    //let path = "day05/test_input_05.txt"
    let path = "day05/day05_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let (ranges, ids) = parseContent content
    countFreshIds(ranges, ids)