module day16_part02

open AdventOfCode_2023.Modules

type Direction =
    | Up
    | Down
    | Left
    | Right

let move (row: int) (col: int) dir =
    match dir with
    | Up -> row - 1, col
    | Down -> row + 1, col
    | Left -> row, col - 1
    | Right -> row, col + 1
    |> fun pos -> pos, dir

let validPosition (pos: int*int) (boundaries: int*int) =
    let (r, c) = pos
    let (height, width) = boundaries
    r >= 0 && c >= 0 && r < height && c < width

let step (input: char array array) (beams: ((int*int)*Direction) list) (maxRows: int) (maxCols: int) =
    beams
    |> List.collect (fun ((row, col), dir) ->
        match input[row][col], dir with
        | '.', _
        | '|', Down
        | '|', Up
        | '-', Right
        | '-', Left -> [ move row col dir ]
        | '-', Down
        | '-', Up -> [ move row col Left; move row col Right ]
        | '|', Left
        | '|', Right -> [ move row col Up; move row col Down ]
        | '\\', Left
        | '/', Right -> [ move row col Up ]
        | '\\', Down
        | '/', Up -> [ move row col Right ]
        | '\\', Up
        | '/', Down -> [ move row col Left ]
        | '\\', Right
        | '/', Left -> [ move row col Down ]
        | _ -> failwith "error")
    |> List.filter (fun (pos, _) -> validPosition pos (maxRows, maxCols))

let calculateEnergized (input: char array array) (pos: int*int) (dir: Direction) (maxRows: int) (maxCols: int) =
    Seq.unfold
        (fun (visited, beams) ->
            match beams with
            | [] -> None
            | _ ->
                let newBeams =
                    (step input beams maxRows maxCols) |> List.filter (fun beam -> not (Set.contains beam visited))

                let newVisited = Set.union visited (Set.ofList newBeams)
                Some(newVisited, (newVisited, newBeams)))
        ([ pos, dir ] |> Set.ofList, [ pos, dir ])
    |> Seq.last
    |> Set.map fst
    |> Set.count

let execute =
    let path = "day16/day16_input.txt"
    let lines = LocalHelper.ReadLines path |> Array.ofSeq |> Array.map (fun line -> line.ToCharArray())
    let height, width = lines.Length, lines[0].Length
    let parts = 
        [ for r in 0 .. height - 1 do
              (r, 0), Right
              (r, width - 1), Left
          for c in 0 .. width - 1 do
              (0, c), Down
              (height - 1, c), Up ]
    parts
    |> Seq.map (fun p -> calculateEnergized lines (fst p) (snd p) height width)
    |> Seq.max