module day06_part01

open AdventOfCode_2025.Modules

type Worksheet = {
    Numbers: bigint list
    Operation: string
}

let parseContent(lines: string array) =
    let numOfWorksheets = lines[0].Split([|' '|], System.StringSplitOptions.RemoveEmptyEntries).Length
    let worksheets = Array.create<Worksheet> numOfWorksheets { Numbers = []; Operation = "" }
    for lIdx, line in lines |> Array.indexed do
        let parts = line.Split([|' '|], System.StringSplitOptions.RemoveEmptyEntries)
        for idx, part in parts |> Array.indexed do
            if lIdx < lines.Length - 1 then
                worksheets[idx] <- { worksheets[idx] with Numbers = worksheets[idx].Numbers @ [bigint.Parse(part.Trim())] }
            else
                worksheets[idx] <- { worksheets[idx] with Operation = part.Trim() }
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
