module day03_part01

open AdventOfCode_BASE.Modules
  
let calculateLine(part: (int*int) list) =
    part
    |> List.map(fun (a, b) -> a*b)
    |> List.reduce (+)

let execute() =
    let path = "day03/day03_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    0