module day15_part02

open System.Text.RegularExpressions

open AdventOfCode_2016.Modules

type Disc = {
    Number: int
    Slots: int
    Time: int
    Position: int
}

let parseContent(lines: string array) =
    lines
    |> Array.map(fun disc ->
        let regexpattern = @"(\d+).*?(\d+).*?=(\d+).*?(\d+)"
        let m' = Regex.Match(disc, regexpattern)
        { 
            Number = (int)(m'.Groups[1].Value);
            Slots = (int)(m'.Groups[2].Value);
            Time = (int)(m'.Groups[3].Value);
            Position = (int)(m'.Groups[4].Value);
        }
    )

let slotPosition (disc: Disc) (second: int) =
    let pos = (disc.Position + second + disc.Number) % disc.Slots
    (pos, pos = 0)

let firstDrop (discs: Disc array) = 
    let mutable second = 0
    let mutable passesAll = false

    while not passesAll do
        passesAll <-
            discs
            |> Array.mapi(fun i d -> slotPosition d (second))
            |> Array.forall(fun (pos, p) -> p)
        if not passesAll then
            second <- second + 1
    second

let execute =
    let path = "day15/day15_input.txt"
    let content = LocalHelper.GetLinesFromFile path

    let discs = Array.append (parseContent content) [|{ Number = 7; Slots = 11; Time = 0; Position = 0 }|]
    firstDrop discs