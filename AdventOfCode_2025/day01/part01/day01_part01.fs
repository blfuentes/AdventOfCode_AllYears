module day01_part01

open AdventOfCode_2025.Modules

type Orientation =
    | Left
    | Right

type Movement = {
        Orientation: Orientation
        Steps: int
    }

let parseContent (lines: string array) =
    lines
    |> Array.map (fun line ->
        match line[0] with
        | 'L' -> { Orientation = Left; Steps = int(line[1..]) }
        | 'R' -> { Orientation = Right; Steps = int(line[1..]) }
        | _ -> failwith "Invalid input"
    )

let getPassword(diallectures: Movement array) =
    let mutable initial = 50
    let mutable count = 0
    for diallecture in diallectures do
        match diallecture.Orientation with
        | Left -> initial <- (initial - diallecture.Steps) % 100
        | Right -> initial <- (initial + diallecture.Steps) % 100
        if initial = 0 then
            count <- count + 1
    count

let execute() =
    //let path = "day01/test_input_01.txt"
    let path = "day01/day01_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let dialectures = parseContent content
    getPassword dialectures