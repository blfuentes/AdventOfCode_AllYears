module day03_part01

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

let processSchematic (schematic: string[,]) =
    let regex = Regex("(\d+)")
    let numbers =
        seq {
            for i in 0..schematic.GetLength(0) - 1 do
                let matches = regex.Matches(String.Join("",schematic.[i,*]))
                let numbers = matches |> Seq.map(fun m -> buildRegion schematic i m.Index m.Value)
                yield! numbers
        } |> Seq.toList
    numbers

let execute =
    let path = "day03/day03_input.txt"
    let lines = GetLinesFromFile path
    let engineSchematic = Array2D.create lines.Length lines.[0].Length ""
    buildSchematicEngine engineSchematic lines
    let linkedNumbers = processSchematic engineSchematic
    linkedNumbers |> List.filter(fun n -> n.neighbours |> List.exists(fun c -> c.isSymbol)) |> List.sumBy(fun n -> (int)n.id)