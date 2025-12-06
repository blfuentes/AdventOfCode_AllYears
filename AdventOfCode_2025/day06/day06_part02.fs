module day06_part02

open AdventOfCode_2025.Modules

type Worksheet = {
    Numbers: bigint list
    Operation: string
}

let parseContent(lines: string array) =
    let numOfWorksheets = lines[0].Split([|' '|], System.StringSplitOptions.RemoveEmptyEntries).Length
    let worksheets = Array.create<Worksheet> numOfWorksheets { Numbers = []; Operation = "" }

    let worksheetMap = Array2D.init lines.Length lines[0].Length (fun r c -> 
        (lines[r][c]).ToString()
    )
    let mutable currentWorksheet = 0
    for column in 0 .. worksheetMap.GetLength(1) - 1 do
        let columnValues = worksheetMap[*, column] |> Array.take (worksheetMap.GetLength(0) - 1)
        if columnValues |> Array.forall(fun v -> v = " ") then
            currentWorksheet <- currentWorksheet + 1
        else
            let newValue = String.concat "" columnValues
            worksheets[currentWorksheet] <- { worksheets[currentWorksheet] with Numbers = worksheets[currentWorksheet].Numbers @ [bigint.Parse(newValue)] }

    let operations = lines[lines.Length - 1].Split([|' '|], System.StringSplitOptions.RemoveEmptyEntries)
    for idx, operation in operations |> Array.indexed do
        worksheets[idx] <- { worksheets[idx] with Operation = operation.Trim() }

    worksheets

let operate (worksheet: Worksheet) =
    match worksheet.Operation with
    | "+" -> worksheet.Numbers |> Seq.sum
    | "*" -> worksheet.Numbers |> Seq.reduce (fun acc x -> acc * x)
    | _ -> bigint.Zero

let execute() =
    //let path = "day06/test_input_06.txt"
    let path = "day06/day06_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let worksheets = parseContent content
    worksheets
    |> Array.sumBy operate