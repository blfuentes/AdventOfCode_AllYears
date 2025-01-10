module day04_part02


open AdventOfCode_2017.Modules.LocalHelper

let isAnagram (s1:string) (s2:string) =
    let s1 = s1 |> Seq.sort |> Seq.toArray
    let s2 = s2 |> Seq.sort |> Seq.toArray
    s1 = s2

let rec checkAnagram (values: string list) =
    match values with
    | [] -> false
    | head::tail ->
        let foundAnagram = tail |> List.exists (fun v -> isAnagram head v)
        match foundAnagram with
        | true -> true
        | false -> checkAnagram tail


let execute() =
    let path = "day04/day04_input.txt"
    let input = GetLinesFromFile path     
    input |> Array.map (fun l -> l.Split(' ') |> Array.toList) |> Array.filter(fun g -> checkAnagram g |> not) |> Array.length