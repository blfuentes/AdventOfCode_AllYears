module day03_part01

open AdventOfCode_2025.Modules

let parseContent (lines: string array) =
    lines
    |> Array.map(fun line ->
        line.ToCharArray()
        |> Array.map (string >> int)
    )

let findJoltage(bank: int array) =
    let firstJoltage = Array.max (bank |> Array.take(bank.Length - 1))
    let idx = Array.findIndex (fun x -> x = firstJoltage) bank
    let secnodJoltage = Array.max (bank |> Array.skip(idx+1))
    (sprintf "%d%d" firstJoltage secnodJoltage) |> int

let sumJoltages (banks: int array array) =
    banks
    |> Array.map findJoltage
    |> Array.sum

let execute() =
    //let path = "day03/test_input_03.txt"
    let path = "day03/day03_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let banks = parseContent content
    sumJoltages banks