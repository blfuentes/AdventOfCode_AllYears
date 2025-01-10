module day06_part02

open AdventOfCode_2022.Modules.LocalHelper

let path = "day06/day06_input.txt"

let rec findIdxOfUnique (size: int) (pos: int) (prev: string) (input: string) = 
    match pos < size with
    | true -> findIdxOfUnique size (pos + 1) (prev + input.Substring(pos, 1)) input
    | false ->
        let lastfour = prev.ToCharArray() |> Array.rev |> Array.take size        
        if lastfour |> Array.countBy(fun c -> c) |> Array.forall(fun (e, c) -> c = 1) then pos 
        else findIdxOfUnique size (pos + 1) (prev + input.Substring(pos, 1)) input
        
let execute =
    let content = GetContentFromFile(path)
    findIdxOfUnique 14 0 "" content