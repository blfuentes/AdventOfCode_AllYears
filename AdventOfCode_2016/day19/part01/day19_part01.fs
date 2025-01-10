module day19_part01


open AdventOfCode_2016.Modules

open System


let findIndex(numOfElves: int) =
    let mutable factor = 1.0
    while Math.Pow(2, factor) < numOfElves do
        factor <- factor + 1.0
    let current = (int)(Math.Pow(2, factor - 1.0))
    2 * (numOfElves - current) + 1

let execute =
    let path = "day19/day19_input.txt"
    let content = LocalHelper.GetContentFromFile path |> int
    findIndex content
    