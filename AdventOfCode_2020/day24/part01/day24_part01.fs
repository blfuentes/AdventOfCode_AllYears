module day24_part01

open AdventOfCode_2020.Modules
open AdventOfCode_Utilities

type Directions = NE | E | SE | SW | W | NW

type TileSide = White | Black

let parseContent (lines: string array) =
    let parseLine line =
        let initialDirection =
            match Seq.head line with
            | 'e' -> [E]
            | 'w' -> [W]
            | _ -> []
        let otherDirections =
            line
            |> Seq.pairwise
            |> Seq.choose (fun (lastChar, thisChar) ->
                            match (lastChar, thisChar) with
                            | ('n', 'e') -> Some NE
                            | ('s', 'e') -> Some SE
                            | (_, 'e') -> Some E
                            | ('n', 'w') -> Some NW
                            | ('s', 'w') -> Some SW
                            | (_, 'w') -> Some W
                            | (_, 'n') -> None
                            | (_, 's') -> None
                            | unrecognised -> failwithf "Unrecognised character pair %A" unrecognised
            )
            |> Seq.toList
        initialDirection @ otherDirections

    lines |> Seq.map parseLine

// Coordinates example:
// 0,0   2,0   4,0
//    1,1   3,1   5,1
// 0,2   2,2   4,2

let getVector direction =
    match direction with
    | NE  -> 1, -1
    | E   -> 2, 0
    | SE  -> 1, 1
    | SW  -> -1, 1
    | W   -> -2, 0
    | NW  -> -1, -1

let followDirections directions =
    directions
    |> List.fold (fun position direction -> direction |> getVector |> add2d position) (0,0)

let flipTile (tileMap: Map<int*int, TileSide>) position =
  tileMap
  |> match tileMap |> Map.tryFind position with
      | Some White -> Map.add position Black
      | Some Black -> Map.add position White
      | None -> Map.add position Black 

let getTileMapFromDirections directions =
    directions
    |> Seq.fold (fun tileMap tileDirections ->
                  tileDirections
                  |> followDirections 
                  |> flipTile tileMap
    ) Map.empty

let findBlackTiles directions =
    directions
    |> getTileMapFromDirections
    |> Map.toList
    |> List.filter (snd >> (=) Black)
    |> List.length

let execute =
    let path = "day24/day24_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let directions = parseContent content
    findBlackTiles directions