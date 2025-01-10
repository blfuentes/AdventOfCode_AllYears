module day01_part02

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
    
let countTimes (element: int*int) (searchlist: (int*int) List) =
    match searchlist |> List.tryFind(fun a -> fst element = fst a) with
    | Some(result) -> (fst element) * (snd element)  * (snd result)
    | None -> 0

let execute() =
    let path = "day01/day01_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let (lefts, rights) = parseContent content
    let groupLeft = lefts |> List.countBy id
    let groupRight = rights |> List.countBy id
    groupLeft |> List.sumBy(fun l -> countTimes l groupRight)
    