module day21_part02

open AdventOfCode_2022.Modules
open AdventOfCode_Utilities

type Expression =
    | Specific of int64
    | Operation of string array
type EMap = Map<string, Expression>

let operation a b = function
    | "+" -> a + b
    | "-" -> a - b
    | "*" -> a * b
    | _ -> a / b

let rec eval (d: EMap) n t =
    match d[n] with
    | Specific v -> v, t, n = "humn"
    | Operation ps ->
        let va, t, ca = eval d ps[0] t
        let vb, t, cb = eval d ps[2] t
        let r = operation va vb ps[1]
        match ca, cb with
        | false, false -> r, t, false
        | false, true -> r, (va, ps[1], false) :: t, true
        | true, false -> r, (vb, ps[1], true) :: t, true
        | _, _ -> failwith "This should never happen."

let parse (line: string) =
    let name = line[0..3]
    let parts = (line[6..]).Split(' ')
    match parts.Length with
    | 1 -> name, Specific (int64 parts[0])
    | _ -> name, Operation (parts)

let reverseEval r (v, o, rs) =
    match o, rs with
    | "/", true -> r * v
    | "/", false -> v / r
    | "-", true -> r + v
    | "-", false -> v - r
    | "*", _ -> r / v
    | _, _ -> r - v

let solve lines =
    let m =
        lines
        |> Seq.map parse
        |> Map.ofSeq
    let _, t, _ = eval m "root" List.empty
    let r, _, _ = t.Head
    let p2 =
        t.Tail
        |> Seq.fold reverseEval r
    p2

let execute =
    let path = "day21/day21_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    solve content