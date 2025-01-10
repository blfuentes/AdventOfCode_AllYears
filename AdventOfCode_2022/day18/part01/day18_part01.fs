module day18_part01

open AdventOfCode_2022.Modules

let parseContent(lines: string array) =
    lines
    |> Array.map(fun l ->
        let arr =
            l.Split(",")
            |> Array.map int
        arr[0], arr[1], arr[2]
    )
    |> Set.ofArray

let neighbors (x, y, z) =
    [ (x - 1, y, z)
      (x + 1, y, z)
      (x, y - 1, z)
      (x, y + 1, z)
      (x, y, z - 1)
      (x, y, z + 1) ]

let part1 cubes =
    cubes
    |> Seq.sumBy (fun c ->
        neighbors c
        |> Seq.filter (fun n -> Set.contains n cubes |> not)
        |> Seq.length)


let execute =
    let path = "day18/day18_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let cubes = parseContent content
    part1 cubes