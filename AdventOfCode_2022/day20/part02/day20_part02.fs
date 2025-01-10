module day20_part02

open AdventOfCode_2022.Modules
open AdventOfCode_Utilities

type Number = { OrigPos: int64; Value: int64 }

let printNumbers n = n |> List.map (fun n -> n.Value) |> printfn "%A"

let rec mix numbers toMix =
    let len = List.length numbers - 1 |> int64

    match toMix with
    | [] -> numbers
    | l ->
        let h = List.head l
        let pos = numbers |> List.findIndex (fun n -> n = h) |> int64
        let front, back = numbers |> List.splitAt (int pos)
        let numbers' = front @ (List.tail back)
        let insertAt = (pos + h.Value) %! len

        mix (numbers' |> List.insertAt (int insertAt) h) (List.tail l)

let rec run numbers rounds =
    let mixOrder = numbers |> List.sortBy (fun n -> n.OrigPos)
    match rounds with
    | 0 -> numbers
    | _ ->
        let numbers' = mix numbers mixOrder
        run numbers' (rounds - 1)


let solve content key rounds =
    let numbers =
        content
        |> List.ofSeq
        |> List.mapi (fun i s -> { OrigPos = i; Value = int64 s * key })

    let mixed =
        run numbers rounds |> Seq.repeatForever |> Seq.skipWhile (fun n -> n.Value <> 0)

    [ 1000; 2000; 3000 ]
    |> Seq.map (fun v -> mixed |> Seq.skip v |> Seq.head |> (fun n -> n.Value))
    |> Seq.sum
    |> int64


let execute =
    let path = "day20/day20_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    solve content 811589153L 10