module day02_part01

open AdventOfCode_2024.Modules

let parseContent (lines: string array) =
    lines
    |> Array.map(fun line -> line.Split(" ") |> Array.map int)

let validDiff a b =
    b - a >= 1 && b - a <= 3

let areSafeInc(i: int array) should =
    i |> Array.pairwise |> Array.forall (fun (a,b) -> should a b)

let areSafeDec(i: int array) should =
    i |> Array.pairwise |> Array.forall (fun (a, b) -> should b a)

let isSafe l = areSafeInc l validDiff || areSafeDec l validDiff

let execute() =
    let path = "day02/day02_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let values = parseContent content
    values
    |> Array.filter isSafe
    |> Array.length