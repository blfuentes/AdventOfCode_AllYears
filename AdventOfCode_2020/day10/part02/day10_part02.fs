module day10_part02


open AdventOfCode_2020.Modules

let path = "day10/day10_input.txt"

let inputLines = LocalHelper.GetLinesFromFile(path) |> Array.map int |> Array.sort |> List.ofArray

let diff(a, b) =
   b - a

let execute =
    let maxValue = inputLines.[inputLines.Length - 1] + 3
    let compList = [0] @ inputLines @ [maxValue] |> List.pairwise |> List.map (diff) |> List.map string |> String.concat ""
    let groups = compList.Split('3')
    let groupsOf2 = groups |> Array.filter(fun c -> c.Length = 2) |> Array.length |> float
    let groupsOf3 = groups |> Array.filter(fun c -> c.Length = 3) |> Array.length |> float
    let groupsOf4 = groups |> Array.filter(fun c -> c.Length = 4) |> Array.length |> float
    (2.0**groupsOf2 * 4.0**groupsOf3 * 7.0**groupsOf4) |> int64