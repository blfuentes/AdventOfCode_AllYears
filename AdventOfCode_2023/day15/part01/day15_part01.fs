module day15_part01

open AdventOfCode_2023.Modules

let calculateHash (input: char array)  =
    let calcHash (acc: int) (input: char) =
        (acc + (int input)) * 17 % 256
    Array.fold calcHash 0 input

let execute =
    let path = "day15/day15_input.txt"
    let lines = LocalHelper.GetContentFromFile path
    let allparts = lines.Split(',') |> Array.map (fun x -> x.ToCharArray())
    Array.map calculateHash allparts |> Array.sum
