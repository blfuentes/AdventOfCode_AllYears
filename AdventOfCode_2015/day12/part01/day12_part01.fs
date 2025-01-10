module day12_part01

open AdventOfCode_2015.Modules
open System.Text.RegularExpressions

let execute =
    let path = "day12/day12_input.txt"

    let content = LocalHelper.GetContentFromFile path
    let regex = Regex.Matches(content, "-?\d+")
    regex |> Seq.map(fun e ->  int e.Value) |> Seq.reduce (+)