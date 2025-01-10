module day07_part02

open AdventOfCode_2024.Modules

let parseContent(lines: string array) =
    lines
    |> Array.map(fun line ->
        (
           System.Int64.Parse(line.Split(":")[0]),
           (line.Split(":")[1]).Trim().Split(" ") |> Array.map int64)
        )

let compute((expected, eqmembers): int64*int64 array) (ops: (int64->int64->int64) array)=
    let rec calculate(expected': int64) (tocalculate: int64 list) (currentResult: int64)=
        if currentResult > expected' then false
        else
            match tocalculate with
            | [] -> expected' = currentResult
            | newvalue :: tocompute ->
                ops
                |> Array.exists(fun op -> (calculate (expected') (tocompute) (op currentResult newvalue)))

    calculate expected (eqmembers |> List.ofSeq) 0      

let (||) a b =
    System.Int64.Parse(a.ToString() + b.ToString())

let execute() =
    let path = "day07/day07_input.txt"
    let content = LocalHelper.GetLinesFromFile path

    parseContent content
    |> Array.sumBy (fun (expected, eqmembers) -> 
        if compute (expected, eqmembers) [| (+); (*); (||) |] then expected else 0
    )