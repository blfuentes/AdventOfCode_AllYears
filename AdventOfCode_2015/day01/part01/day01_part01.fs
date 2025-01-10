module day01_part01

open AdventOfCode_2015.Modules.LocalHelper

let path = "day01/day01_input.txt"

let execute = 
    let inputline: char[] = GetContentFromFile(path) |> Seq.toArray
    let elements2
        = ((inputline |> Array.filter (fun (x: char) -> x = '(')).Length , (inputline |> Array.filter (fun (x: char) -> x = ')')).Length)
    fst elements2 - snd elements2

