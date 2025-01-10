module day24_part01

open AdventOfCode_2021.Modules

let parseContent (lines: string list) =
    lines
    |> List.map (fun line -> line.Split() |> List.ofArray)

let getRelevantAdds (puzzle: string list list) =
    let div1 = ResizeArray<int option>()
    let div26 = ResizeArray<int option>()
    
    for i in 0 .. 18 .. (puzzle.Length - 1) do
        if puzzle[i + 4].[2] = "1" then
            div1.Add(Some (int puzzle[i + 15].[2]))
            div26.Add(None)
        else
            div1.Add(None)
            div26.Add(Some (int puzzle[i + 5].[2]))

    List.ofSeq div1, List.ofSeq div26

let getModelNo (div1: int option list) (div26: int option list) =
    let modelNo = Array.create 14 0
    let stack = ResizeArray<(int * int)>()
    let startDigit = 9

    for i in 0 .. div1.Length - 1 do
        let a, b = div1[i], div26[i]
        match a with
        | Some value ->
            stack.Add((i, value))
        | None ->
            let (ia, a) =  stack.Item(stack.Count - 1)
            stack.RemoveAt(stack.Count - 1)
            let diff = a + b.Value
            modelNo[ia] <- min startDigit (startDigit - diff)
            modelNo[i] <- min startDigit (startDigit + diff)
    modelNo

let solve (puzzle: string list list) =
    let div1, div26 = getRelevantAdds puzzle
    let modelNo = getModelNo div1 div26
    modelNo |> Array.map string |> String.concat ""

let execute =
    let path = "day24/day24_input.txt"
    let content = LocalHelper.GetLinesFromFile path |> List.ofArray
    solve (parseContent content)