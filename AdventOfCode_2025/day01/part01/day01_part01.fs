module day01_part01

open AdventOfCode_2025.Modules
open AdventOfCode_Utilities

type Movement =
    | Left of int
    | Right of int

let parseContent (lines: string array) =
    lines
    |> Array.map (fun line ->
        match line[0] with
        | 'L' -> Left(int(line[1..]))
        | 'R' -> Right(int(line[1..]))
        | _ -> failwith "Invalid input"
    )

let getPassword (diallectures: Movement array) =
    diallectures |> Array.fold (fun ((pos, count): int*int) (diallecture: Movement) ->
        let newPos =
            match diallecture with
            | Left steps -> safeModulo(pos - steps) 100
            | Right steps -> safeModulo(pos + steps) 100
        (newPos, if newPos = 0 then count + 1 else count)
    ) (50, 0) |> snd

let execute() =
    //let path = "day01/test_input_01.txt"
    let path = "day01/day01_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let dialectures = parseContent content
    getPassword dialectures