module day01_part02

open AdventOfCode_2015.Modules.LocalHelper

let path = "day01/day01_input.txt"
let inputline = GetContentFromFile(path).ToCharArray()
let numOfMovs = inputline.Length

let calculateFloor (checkFloor:int) = 
    let floor = inputline |> Array.take(checkFloor)
    let elements2 = ((floor |> Array.filter (fun x -> x = '(')).Length , (floor |> Array.filter (fun x -> x = ')')).Length)
    fst elements2 - snd elements2

let execute =
    1 + ([|0..numOfMovs|] |> Array.takeWhile(fun x -> (calculateFloor x) >= 0) |> Array.last)
