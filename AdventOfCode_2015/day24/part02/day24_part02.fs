module day24_part02

open AdventOfCode_2015.Modules

let parseContent(lines: string array) =
    lines |> Array.map int |> List.ofArray

let rec comb n l = 
    match n, l with
    | 0, _ -> [[]]
    | _, [] -> []
    | k, (x::xs) -> List.map ((@) [x]) (comb (k-1) xs) @ comb k xs

let minQE (nums: int list) grps =
    [2..(nums.Length/grps - 1)]
    |> List.map (fun n -> comb n nums) |> List.concat
    |> List.filter (fun cmb -> cmb |> List.sum = (nums |> List.sum) / grps)
    |> Seq.groupBy (fun cmb -> cmb.Length)
    |> Seq.minBy (fun (len,_) -> len) |> snd
    |> Seq.map (fun cmb -> cmb |> List.map int64 |> List.reduce (*)) |> Seq.min

let execute =
    let input = "./day24/day24_input.txt"
    let content = LocalHelper.GetLinesFromFile input
    let weights = parseContent content
    minQE weights 4