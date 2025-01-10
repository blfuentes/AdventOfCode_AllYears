module day16_part01

open System.Text.RegularExpressions
open AdventOfCode_2020.Modules

type RangeKind =
    | Class
    | Row
    | Seat

type Range = {
    Start: int
    End: int
}

type Notes = {
    Ranges: Range list
    MyTickets: int array
    OtherTickets: int array
}

let parseContent(input: string) =
    let parts = input.Split($"{System.Environment.NewLine}{System.Environment.NewLine}")
    let rangepattern = @"(\d+)-(\d+) or (\d+)-(\d+)"
    let numberspattern = @"(\d+)"
    let ranges =
        [
            for p in parts[0].Split(System.Environment.NewLine) do
                let m' = Regex.Match(p, rangepattern)
                let (f1, t1, f2, t2) =
                    ((int)(m'.Groups[1].Value), (int)(m'.Groups[2].Value), (int)(m'.Groups[3].Value), (int)(m'.Groups[4].Value))
                yield! [{ Start = f1; End = t1 }; { Start = f2; End = t2 }]
        ]
    let myticket = (parts[1].Split(System.Environment.NewLine)[1]).Split(",") |> Array.map int
    let o' = Regex.Matches((parts[2]), numberspattern)
    let othertickets = o' |> Seq.map(fun v -> int(v.Value))

    { Ranges = ranges; MyTickets = myticket; OtherTickets = othertickets |> Array.ofSeq }

let errorRate(notes: Notes) =
    notes.OtherTickets
    |> Array.filter(fun t ->
        not (notes.Ranges |> List.exists(fun r -> r.Start <= t && t <= r.End))
    )
    |> Array.sum

let execute =
    let path = "day16/day16_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let notes = parseContent content

    errorRate notes