module day13_part02

open AdventOfCode_2015.Modules
open AdventOfCode_Utilities

type HappyDelta = {
    From: string;
    To: string;
    Value: int;
}

let parseContent(lines: string array) : (string seq*Map<string*string, int>) =
    let result = seq {
        for line in lines do
            let parts = line.Split(" ")
            let from = parts[0]
            let to' = parts[parts.Length - 1].Replace(".", "")
            let value = if parts[2] = "gain" then (int)parts[3] else -(int)parts[3]
            yield { From = from; To = to'; Value = value }
    }
    let mapping =
        result
        |> Seq.map(fun r -> (r.From, r.To), r.Value) |> Map.ofSeq
    ((result |> Seq.map _.From |> Seq.distinct), mapping)

let calculateHappiness(elements: string list) (mapping: Map<string*string, int>) =
    (elements
    |> List.pairwise
    |> List.sumBy(fun (from,to') -> mapping.Item(from, to') + mapping.Item(to', from)))

let execute =
    let input = "./day13/day13_input.txt"
    let content = LocalHelper.GetLinesFromFile input
    let (elements, mapping) = parseContent content
    let myself = "myself"
    let newmappings = 
        seq {
            for el in elements do
                yield ((el, myself), 0)
                yield ((myself, el), 0)
        } |> List.ofSeq
    let fullmapping = List.fold (fun (map: Map<string*string, int>) (key, value) -> map.Add(key, value)) mapping newmappings

    let elementsWithMe = (elements |> List.ofSeq) @ [myself]

    let possibleComb = 
        Utilities.perms elementsWithMe
        |> Seq.map(fun l -> l @ [l.Head])
    possibleComb
    |> Seq.map(fun c -> calculateHappiness c fullmapping)
    |> Seq.max