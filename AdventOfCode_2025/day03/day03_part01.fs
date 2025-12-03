module day03_part01

open AdventOfCode_2025.Modules

let parseContent (lines: string array) =
    lines
    |> Array.map(fun line ->
        line.ToCharArray()
        |> Array.map (string >> int)
    )

let findJoltage(bank: int array) =
    let firstIdx, firstMax = 
        bank 
        |> Array.take(bank.Length - 1)
        |> Array.indexed
        |> Array.maxBy snd
    
    let secondMax = 
        bank 
        |> Array.skip(firstIdx + 1)
        |> Array.max
    
    firstMax * 10 + secondMax

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