module day11_part02

open AdventOfCode_2025.Modules
open System.Collections.Generic

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

let countPathsMemoized (devices: Map<string, Device>) =
    let start = "svr"
    let goal = "out"
    let required = Set.ofList ["dac"; "fft"]
    
    let cache = Dictionary<string * Set<string>, int64>()
    
    let rec dfs visited requiredVisited current =
        if current = goal then
            if Set.isEmpty (Set.difference required requiredVisited) then 1L else 0L
        else
            let key = (current, requiredVisited)
            match cache.TryGetValue(key) with
            | true, count -> count
            | false, _ ->
                let mutable totalCount = 0L
                
                match devices.TryFind current with
                | None -> ()
                | Some device ->
                    for neighbor in device.Outputs do
                        if not (Set.contains neighbor visited) then
                            let newRequiredVisited = 
                                if required.Contains(neighbor) then
                                    Set.add neighbor requiredVisited
                                else
                                    requiredVisited
                            
                            totalCount <- totalCount + 
                                dfs (Set.add neighbor visited) 
                                    newRequiredVisited 
                                    neighbor
                
                cache[key] <- totalCount
                totalCount
    
    dfs (Set.singleton start) Set.empty start

let execute() =
    //let path = "day11/test_input_11b.txt"
    let path = "day11/day11_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let devices = parseContent content
    countPathsMemoized devices
