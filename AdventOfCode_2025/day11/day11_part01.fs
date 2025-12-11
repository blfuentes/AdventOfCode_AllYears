module day11_part01

open AdventOfCode_2025.Modules

type Device = {
    From: string
    Outputs: string list
}

let parseContent (lines: string array) =
    lines
    |> Array.map (fun line ->
        let parts = line.Split(":")
        {
            From = parts[0].Trim();
            Outputs = parts[1].Trim().Split(" ") |> List.ofArray
        }
    )
    |> Array.map(fun device -> (device.From, device)
    ) |> Map.ofArray

let countPaths (devices: Map<string, Device>) =
    let start = devices["you"]
    let goal = "out"

    let results = ResizeArray<Device list>()

    let rec dfs visited path current =
        if List.contains goal current.Outputs then
            results.Add(List.rev (current :: path))
        else
            for neighbor in current.Outputs do
                if not (Set.contains neighbor visited) then
                    dfs (Set.add neighbor visited) (current :: path) (devices[neighbor])

    dfs (Set.singleton start.From) [] start
    results.Count

let execute() =
    //let path = "day11/test_input_11.txt"
    let path = "day11/day11_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let devices = parseContent content
    countPaths devices