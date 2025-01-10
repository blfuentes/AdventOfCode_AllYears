module day25_part01

open AdventOfCode_2021.Modules

type Coord = (int * int)
type Herd = Coord []
type Navigator = Coord -> Coord
let eastMovement maxcols (r, c) : Coord = if c < maxcols then r, c + 1 else r, 0
let southMovement maxrows (r, c) : Coord = if r < maxrows then r + 1, c else 0, c

type HerdKind =
    | East
    | South
    | Empty

let ofChar c =
    match c with
    | '>' -> East
    | 'v' -> South
    | _ -> Empty

let findHerd ((east: Coord list), (south: Coord list), (sea: HerdKind [,])) (row: int, col: int, herd: HerdKind) =
    match herd with
    | East ->
        sea[row, col] <- herd
        (row, col) :: east, south, sea
    | South ->
        sea[row, col] <- herd
        east, (row, col) :: south, sea
    | Empty ->
        east, south, sea

let parseContent(lines: string array) =
    let data =
        lines
        |> Array.mapi (fun r l -> l.ToCharArray() |> Array.mapi (fun c o -> r, c, ofChar o))
        |> Array.concat

    let maxrows, maxcols, _ = data |> Array.last

    let seaMap =
        Array2D.init (maxrows + 1) (maxcols + 1) (fun row col -> Empty)

    let eastHerd, southHerd =
        data
        |> Array.fold findHerd (List.empty, List.empty, seaMap)
        |> (fun (eh, sh, _) -> List.toArray eh, List.toArray sh)

    ((eastHerd, southHerd), (maxrows, maxcols), seaMap)

let canMove (sea: HerdKind [,]) (nav: Navigator) i (sc: Coord) : Option<int * Coord * Coord> =
    let (nr, nc) = nav sc

    if sea[nr, nc].IsEmpty then
        Some(i, sc, (nr, nc))
    else
        None

let move (sea: HerdKind [,]) (herd: Herd) (i, (fr, fc): Coord, (tr, tc): Coord) =
    sea[tr, tc] <- sea.[fr, fc]
    sea[fr, fc] <- Empty
    herd[i] <- (tr, tc)

let step (sea: HerdKind [,]) (nav: Navigator) (herd: Herd) =
    let movable =
        herd
        |> Array.mapi (canMove sea nav)
        |> Array.choose id

    movable |> Array.iter (move sea herd)
    movable.Length > 0

let findNonStepsFurther ((eastHerd, southHerd), (maxRows, maxCols), sea) =
    let eastNav = eastMovement maxCols
    let southNav = southMovement maxRows

    let rec takeStep sc =
        let eastMoved = step sea eastNav eastHerd
        let southMoved = step sea southNav southHerd

        match (eastMoved || southMoved) with
        | true -> takeStep (sc + 1)
        | false -> sc

    takeStep 1

let execute =
    let path = "day25/day25_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let ((eastHerd, southHerd), (maxRows, maxCols), seaMap) = parseContent content
    findNonStepsFurther ((eastHerd, southHerd), (maxRows, maxCols), seaMap)