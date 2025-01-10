module day09_part01

open AdventOfCode_2024.Modules

let parseContent(line: string) =
    line.ToCharArray() |> Array.map (fun v -> System.Int32.Parse((string)v))

let buildblocks (input: int array) =
    let builder = new System.Text.StringBuilder()
    let mutable currentVIdx = 0
    input
    |> Array.mapi(fun idx v ->
        let replica =
            if idx = 0 then
                "0"
            else
                if idx % 2 = 0 then 
                    currentVIdx.ToString()                     
                else "."
        if replica <> "." then currentVIdx <- currentVIdx + 1
        Array.create v replica
    ) |> Array.concat

let processblocks (input: string array) =
    let mutable revIdx = input.Length - 1
    let mutable added = 0
    for bIdx in [1..input.Length-1] do
        let value = input[bIdx]
        if input |> Array.skip(bIdx) |> Array.exists(fun v -> v <> ".") then
            if value = "." then
                while input[revIdx] = "." do
                    revIdx <- revIdx - 1
                added <- added + 1
                input[bIdx] <- input[revIdx]
                input[revIdx] <- "."
            
    input |> Array.filter(fun v -> v <> ".")

let execute() =
    let path = "day09/day09_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let block = buildblocks (parseContent content)
    let processed = processblocks block
    processed
    |> Array.mapi(fun idx v -> bigint(idx) * bigint.Parse(v))
    |> Array.sum

