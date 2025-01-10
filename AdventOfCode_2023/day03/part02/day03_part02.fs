module day03_part02

open System
open System.Text.RegularExpressions

open AdventOfCode_2023.Modules.LocalHelper

type Coord = {
    element: string
    row: int
    col: int
    isSymbol: bool
}

type Region = {
    id: string
    col: int
    row: int
    width: int
    neighbours: Coord list
}

let (|Int|_|) (str:string) =
    match System.Int32.TryParse str with
    | true,int -> Some int
    | _ -> None

let isNumber (input: string): bool =
    match input with
    |Int i -> true 
    |_ -> false

let isSymbol (input: string): bool =
    match input with
    |Int i -> false 
    |_ -> if input = "." then false else true

let buildSchematicEngine (schematic: string[,]) (lines: string[]) =
    for i in 0..lines.Length-1 do
        for j in 0..lines.[0].Length-1 do
            schematic.[i,j] <- lines.[i].[j].ToString()

let buildRegion (schematic: string[,]) (row: int) (col: int) (number: string) =
    let directions = [(-1,-1);(-1,0);(-1,1);(0,-1);(0,1);(1,-1);(1,0);(1,1)]
    let tmpNeighbours =
        seq {
            for j in (col)..(col + number.Length - 1) do
                for dir in directions do
                    let newRow = row + (fst dir)
                    let newCol = j + (snd dir)
                    if (newRow >= 0 && newCol >= 0 && newRow < schematic.GetLength(0) && newCol < schematic.GetLength(1)) then
                        let element = schematic.[newRow, newCol]
                        if not (isNumber element) then
                            yield { element = element; row = newRow; col = newCol; isSymbol = isSymbol element }
                    
        } |> Seq.toList
    let region = { id = number; col = col; row = row; width = number.Length; neighbours = tmpNeighbours }
    region

let findNumberRegions (schematic: string[,]) =
    let regex = Regex("(\d+)")
    let numbers =
        seq {
            for i in 0..schematic.GetLength(0) - 1 do
                let matches = regex.Matches(String.Join("",schematic.[i,*]))
                let numbers = matches |> Seq.map(fun m -> buildRegion schematic i m.Index m.Value)
                yield! numbers
        } |> Seq.toList
    numbers

let findGearRegions (schematic: string[,]) =
    let regex = Regex("(\*)")
    let gears =
        seq {
            for i in 0..schematic.GetLength(0) - 1 do
                let matches = regex.Matches(String.Join("",schematic.[i,*]))
                let gears = matches |> Seq.map(fun m -> buildRegion schematic i m.Index m.Value)
                yield! gears
        } |> Seq.toList
    gears

let isNeighbour (coord: Region) (region: Region) =
    let neighbours = region.neighbours
    let result = neighbours |> Seq.filter(fun n -> n.col = coord.col && n.row = coord.row)
    not (Seq.isEmpty result)

let findCollision (numbers: Region list) (gears: Region list) =
    let collisions =
        seq {
            for ger in gears do
                let numbers = numbers |> Seq.filter(fun n -> (isNeighbour ger n)) |> Seq.toList
                if numbers.Length = 2 then
                    yield numbers |> List.map (fun n ->  (int)n.id) |> List.reduce (*)
        } |> Seq.toList
    collisions

let execute =
    let path = "day03/day03_input.txt"
    let lines = GetLinesFromFile path
    let engineSchematic = Array2D.create lines.Length lines.[0].Length ""
    buildSchematicEngine engineSchematic lines
    let numbers = findNumberRegions engineSchematic
    let gears = findGearRegions engineSchematic
    findCollision numbers gears |> List.sum
    