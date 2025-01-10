module day19_part01

open AdventOfCode_2018.Modules
open AdventOfCode_Utilities

let getUniqueVals lines =
    let lines = lines |> Seq.toArray
    // after comparing many inputs, the only lines that were different were 
    // the constants on line 23 and 25
    let unique1 = lines.[22] |> splitByFn " " (fun p -> int p.[2])
    let unique2 = lines.[24] |> splitByFn " " (fun p -> int p.[2])
    unique1, unique2

let getPart1Target (unique1, unique2) =
    let target = 
        2 // addi target 2 target
        |> (fun t -> t * t) // mulr target target target
        |> (*) 19 // mulr ip target target
        |> (*) 11 // muli target 11 target
    let temp =
        unique1 // addi temp unique1 temp
        |> (*) 22 // mulr temp ip temp
        |> (+) unique2 // addi temp unique2 temp
    temp + target // addr target temp target

let getFactors target =
    seq { for i = 1 to (target |> double |> sqrt |> int) do
            if target % i = 0 then
              yield i
              if i * i <> target then
                yield target / i }

let solve f = f >> getFactors >> Seq.sum

let execute =
    let path = "day19/day19_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let uniqueVals = getUniqueVals content
    solve getPart1Target uniqueVals
