module day25_part01

open AdventOfCode_Utilities
open AdventOfCode_2018.Modules

let manhattan p0 p1 = Array.zip p0 p1 |> Array.sumBy (fun (c0, c1) -> abs (c0 - c1))

let asIntArray : string [] -> int [] = Array.map int

let solve points =
    let rec countComponents count unseen =
        let rec findComponent queue unseen =
            match queue with
            | p0 :: ps ->
                let toAdd, toKeep = List.partition (fun p1 -> manhattan p0 p1 <= 3) unseen
                let newQueue = List.foldBack (fun t q -> t :: q) toAdd ps
                findComponent newQueue toKeep
            | [] -> unseen
        match unseen with
        | p :: _ -> countComponents (count + 1) (findComponent [p] unseen)
        | [] -> count
    countComponents 0 (Seq.toList points)

let execute =
    let path = "day25/day25_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let points = content |> Seq.map (splitByFn "," asIntArray)
    solve points
