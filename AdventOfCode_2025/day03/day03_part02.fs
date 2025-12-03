module day03_part02

open AdventOfCode_2025.Modules

let parseContent (lines: string array) =
    lines
    |> Array.map(fun line ->
        line.ToCharArray()
        |> Array.map (string >> int)
    )

let findJoltage(bank: int array) =
    let rec findMaxValues i init acc =
        if i < 0 then acc
        else
            let windowEnd = bank.Length - i
            let slice = bank |> Array.take windowEnd |> Array.skip init
            let maxVal = Array.max slice
            let idx = Array.findIndex (fun x -> x = maxVal) (bank |> Array.skip init)
            let newInit = init + idx + 1
            findMaxValues (i - 1) newInit (maxVal :: acc)
    
    findMaxValues 11 0 [] 
    |> List.rev
    |> List.fold (fun acc digit -> acc * 10I + bigint digit) 0I

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