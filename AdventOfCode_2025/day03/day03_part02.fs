module day03_part02

open AdventOfCode_2025.Modules
let calculateLine(part: (int*int) list) =
    part
    |> List.map(fun (a, b) -> a*b)
    |> List.reduce (+)

let parseContent (lines: string array) =
    lines
    |> Array.map(fun line ->
        line.ToCharArray()
        |> Array.map (string >> int)
    )

let findJoltage(bank: int array) =
    let joltageParts = ResizeArray<string>()
    let mutable init = 0
    for i in 11 .. -1 .. 0 do
        let part = Array.max (bank |> Array.take(bank.Length - i) |> Array.skip(init))
        joltageParts.Add(part.ToString())
        let idx = Array.findIndex (fun x -> x = part) (bank |> Array.skip(init))
        init <- init + idx + 1
    let value = (String.concat"" joltageParts)
    //printfn "%s" value
    bigint.Parse(value)

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
    