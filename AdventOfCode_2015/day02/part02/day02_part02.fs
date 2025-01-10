module day02_part02

open AdventOfCode_2015.Modules.LocalHelper

let path = "day02/day02_input.txt"
let inputLine = GetLinesFromFile(path) |> Seq.map(fun x -> x.Split('x') |> Array.map int)

let calculateRibbonPaper(d: int[]) =
    let smallest = d |> Array.sort |> Array.take(2)
    let wrap = 2 * (smallest |> Array.sum)
    let bow = d |> Array.fold (*) 1
    wrap + bow
    

let execute =
    inputLine |> Seq.sumBy(fun p -> calculateRibbonPaper(p))