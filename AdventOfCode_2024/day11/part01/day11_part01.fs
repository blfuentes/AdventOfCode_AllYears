module day11_part01

open AdventOfCode_2024.Modules

let parseContent(lines: string) =
    lines.Split(" ") |> Array.map int64

let mutateStones(stone: int64) =
    match stone with
    | 0L -> [1L]
    | s when s.ToString().Length % 2 = 0 -> 
        let mid = s.ToString().Length / 2
        let left, right = (s.ToString()[0..mid-1], s.ToString()[mid..])
        [System.Int64.Parse(left); System.Int64.Parse(right)]
    | _ -> [(stone * 2024L)]

let rec runMutations (stones: int64 array) (step: int) =
    if step = 0 then
        stones
    else
        let newmutations = 
            seq {
                for stone in stones do
                    yield mutateStones stone
            } |> Seq.concat |> Array.ofSeq
        runMutations newmutations (step - 1)

let execute() =
    let path = "day11/day11_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let stones = parseContent content
    let mutated = runMutations stones 25
    mutated.Length