module day01_part01

open AdventOfCode_2024.Modules
open System

let parseContent (lines: string array) =
    let (lefties, righties) =
        (([], []), lines)
        ||> Array.fold (fun (lefties, righties) line ->
            match line.Split(" ", StringSplitOptions.RemoveEmptyEntries) with
            | [|left; right|] -> (int left) :: lefties, (int right) :: righties
            | _ -> failwith "error"
        )
    (lefties |> List.rev, righties |> List.rev)

let execute() =
    let path = "day01/day01_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let (lefts, rights) = parseContent content
    List.zip(lefts |> List.sort) (rights |> List.sort) |>  List.sumBy (fun (a, b) -> abs (a - b))