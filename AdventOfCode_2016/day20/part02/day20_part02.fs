module day20_part02

open AdventOfCode_2016.Modules

let parseContent(lines: string array) =
    lines
    |> Array.map(fun l ->
        ((int64)(l.Split("-")[0]), (int64)(l.Split("-")[1]))
    )

let findAllValidIps(maxvalue: int64) (blockedRanges: (int64*int64) array) =
    let isfound (value: int64) =
        blockedRanges
        |> Array.exists(fun (f, t) -> value >= f && value <= t || value > maxvalue) 

    let possible = blockedRanges |> Array.map(fun (f, t) -> t + 1L)
    let expected = possible |> Array.filter(fun v -> not (isfound v))
    let mutable counter = 0L
    for ip in expected do
        let mutable ip' = ip
        while not(isfound ip') do
            counter <- counter + 1L
            ip' <- ip' + 1L
    counter


let execute =
    let path = "day20/day20_input.txt"
    let content = LocalHelper.GetLinesFromFile path

    let blockedRanges = parseContent content
    let maxvalue = 4294967295L
    findAllValidIps maxvalue blockedRanges
