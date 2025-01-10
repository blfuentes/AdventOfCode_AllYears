module day13_part02

open AdventOfCode_Utilities
open AdventOfCode_2017.Modules

type Layer = {
    Depth: int
    Range: int
}

let parseContent(lines: string array) =
    lines
    |> Array.map(fun l ->
        { Depth = (int)(l.Split(": ")[0]); Range = (int)(l.Split(": ")[1]) }
    )

let position(layer: Layer) (second: int) =
    zigzagPosition second (layer.Range - 1)

let caugthAt(layers: Layer array) (delay: int)=
    layers
    |> Array.sumBy(fun l ->
        let pos = position l (l.Depth + delay)
        if pos = 0 then
            (l.Depth + delay) * l.Range
        else
            0
    )

let findDelay(layers: Layer array) =
    let rec tryDelay(second: int) =
        match caugthAt layers second with
        | 0 -> second
        | _ -> tryDelay (second + 1)

    tryDelay 0

let execute() =
    let path = "day13/day13_input.txt"
    let content = LocalHelper.GetLinesFromFile path

    let layers = parseContent content
    findDelay layers