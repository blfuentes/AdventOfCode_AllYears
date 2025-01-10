module day16_part02

open System.Text.RegularExpressions
open AdventOfCode_2020.Modules

type Range = {
    Start: int64
    End: int64
    IsDeparture: bool
}

type Notes = {
    Ranges: Range list array
    MyTickets: int64 array
    OtherTickets: int64 array array
}

let parseContent(input: string) =
    let parts = input.Split($"{System.Environment.NewLine}{System.Environment.NewLine}")
    let rangepattern = @"(\d+)-(\d+) or (\d+)-(\d+)"
    let numberspattern = @"(\d+)"
    let ranges =
        [
            for p in parts[0].Split(System.Environment.NewLine) do
                let m' = Regex.Match(p, rangepattern)
                let departure = p.StartsWith("departure")
                let (f1, t1, f2, t2) =
                    ((int64)(m'.Groups[1].Value), (int64)(m'.Groups[2].Value), (int64)(m'.Groups[3].Value), (int64)(m'.Groups[4].Value))
                yield [{ Start = f1; End = t1; IsDeparture = departure }; { Start = f2; End = t2; IsDeparture = departure }]
        ]
    let myticket = (parts[1].Split(System.Environment.NewLine)[1]).Split(",") |> Array.map int64
    let otickets = parts[2].Split(System.Environment.NewLine) |> Array.skip(1)
    let nearby =
        [
            for n in otickets do
                yield n.Split(",") |> Array.map int64
        ]

    { Ranges = ranges |> Array.ofList; MyTickets = myticket; OtherTickets = nearby |> Array.ofList }

let errorRate(notes: Notes) =
    let otherfiltered =
        notes.OtherTickets
        |> Array.filter(fun ta ->
            ta
            |> Array.forall(fun t ->
                (notes.Ranges |> List.concat) |> List.exists(fun r -> r.Start <= t && t <= r.End)
            )
        )
    { notes with OtherTickets = otherfiltered }

let calculatedeparture(notes: Notes) =
    let maxnumber = 19
    let cleanednotes = errorRate notes
    let alltickets = Array.append [| cleanednotes.MyTickets |] cleanednotes.OtherTickets
    let validindexes =
        [|0..maxnumber|]
        |> Array.map(fun idx ->
            notes.Ranges
            |> Array.indexed
            |> Array.filter(fun (_, ranges) ->
                alltickets
                |> Array.forall(fun t ->
                    ranges |> List.exists(fun r -> r.Start <= t[idx] && t[idx] <= r.End)
                )
            )
            |> Array.map fst
            |> Set.ofArray
        )

    let solution = Array.create (maxnumber+1) -1

    let rec cleanIndexes (indexes: Set<int> array) (current: Set<int>) (index: int) =
        if current.Count > maxnumber then
            [0..5]
            |> List.map(fun idx -> 
                notes.MyTickets[solution[idx]]
            )
            |> List.reduce (*) 
        else
            let idx = validindexes |> Array.findIndex(fun v -> v.Count = index + 1)
            let tobecleaned = indexes[idx]
            let sindex = (Set.difference tobecleaned current).MinimumElement
            solution[sindex] <- idx 
            cleanIndexes indexes (Set.add sindex current) (index + 1)

    let index = validindexes |> Array.findIndex(fun v -> v.Count = 1)
    let sindex = validindexes[index].MinimumElement
    solution[sindex] <- index

    cleanIndexes validindexes (Set.ofList([solution[index]]))  1

let execute =
    let path = "day16/day16_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let notes = parseContent content

    calculatedeparture notes