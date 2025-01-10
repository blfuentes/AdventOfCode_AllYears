module day18_part02

open AdventOfCode_2023.Modules
open System.Text.RegularExpressions
open System

type Direction = UP | RIGHT | DOWN | LEFT

type Instruction = { Direction: Direction; Steps: bigint; Color: string }

let calculateFromHex(input: string) =
    (Int32.Parse(input.Substring(0, input.Length - 1), System.Globalization.NumberStyles.HexNumber)) |> bigint

let dirRows = [| -1I; 0I; 1I; 0I |]
let dirCols = [| 0I; 1I; 0I; -1I |]

let digHoleFold (ans: Instruction list) (dirMap: Map<string, Direction>) (directions: Direction[])=
    let calcArea (acc: bigint*bigint*bigint*bigint) (input: Instruction) =
        let steps = calculateFromHex(input.Color)
        let direction = dirMap.[input.Color.Substring(input.Color.Length - 1)]
        let dIdx = Array.findIndex ((=) direction) directions
        let row, col, border, area = acc
        let newRow = row + dirRows[dIdx] * steps
        let newCol = col + dirCols[dIdx] * steps
        let newArea = area + (row - newRow) * (col + newCol) * 1I
        let newBorder = border + steps
        (newRow, newCol, newBorder, newArea)

    let _, _, border, area = List.fold calcArea (0I, 0I, 0I, 0I) ans
    bigint ((float)border / 2. + Math.Abs((float)area) / 2. + 1. )

let parseInput (input: string list) =
    input 
        |> List.map (fun l ->
            let parts = Regex.Match(l, @"([U|D|L|R])\s([\d]+)\s\(#([0-9a-fA-F]+)\)")
            { 
                Direction = 
                    match parts.Groups.[1].Value with
                    | "U" -> UP
                    | "D" -> DOWN
                    | "L" -> LEFT
                    | "R" -> RIGHT
                    | _ -> failwith "Unknown direction"
                ; Steps = bigint.Parse(parts.Groups.[2].Value)
                ; Color = parts.Groups.[3].Value
            }
    )

let execute =
    let path = "day18/day18_input.txt"
    let instructions = LocalHelper.ReadLines(path) |> List.ofSeq |> parseInput
    let mapDirection = Map.empty
                        .Add("0", RIGHT)
                        .Add("1", DOWN)
                        .Add("2", LEFT)
                        .Add("3", UP)
    digHoleFold instructions mapDirection [| UP; RIGHT; DOWN; LEFT |]