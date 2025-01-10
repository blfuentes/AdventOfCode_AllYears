module day20_part01

open AdventOfCode_2016.Modules

let parseContent(lines: string array) =
    lines
    |> Array.map(fun l ->
        ((int64)(l.Split("-")[0]), (int64)(l.Split("-")[1]))
    )

let findFirstAvailable(blockedRanges: (int64*int64) array) =
    let mutable lowest = 0L
    let sorted = blockedRanges |> Array.sortBy(fun (f,t) -> f)
    
    for idx in [0..sorted.Length-1] do
        let (bottom, top) = sorted[idx]
        if lowest >= bottom && lowest <= top then
            lowest <- top + 1L    
    lowest

let execute =
    let path = "day20/day20_input.txt"
    let content = LocalHelper.GetLinesFromFile path

    let blockedRanges = parseContent content
    findFirstAvailable blockedRanges