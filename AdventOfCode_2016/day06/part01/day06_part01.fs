module day06_part01


open AdventOfCode_2016.Modules.LocalHelper

let buildContainer (value: string) (listOfChars: string list array) =
    value.ToCharArray() |> Array.iteri(fun i c -> listOfChars.[i] <- (listOfChars.[i] @ [(c |> string)]))

let getErrorConnectedVersion (listOfChars: string list array) =
    listOfChars |> Array.map(fun l -> (l |> List.groupBy id) |> List.sortByDescending(fun (k, v) -> v.Length) |> List.head |> fst) 

let execute =
    let path = "day06/day06_input.txt"
    let input = GetLinesFromFile path
    let container = Array.create input.[0].Length [""]
    input |> Array.iter(fun s -> buildContainer s container)
    String.concat "" ((getErrorConnectedVersion container) |> Array.toSeq)