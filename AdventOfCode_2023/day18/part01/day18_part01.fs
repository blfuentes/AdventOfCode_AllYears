module day18_part01

open AdventOfCode_2023.Modules
open System.Text.RegularExpressions
open System

type Direction = UP | RIGHT | DOWN | LEFT

type Instruction = { Direction: Direction; Steps: int; Color: string }

let dirRows = [| -1; 0; 1; 0 |]
let dirCols = [| 0; 1; 0; -1 |]

let digHoleFold (ans: Instruction list) (directions: Direction[]) =
    let calcArea (acc: int*int*int*int) (input: Instruction) =
        let dIdx = Array.findIndex (fun d -> d = input.Direction) directions
        let row, col, border, area = acc
        let newRow = row + dirRows[dIdx] * input.Steps
        let newCol = col + dirCols[dIdx] * input.Steps
        let newArea = area + (row - newRow) * (col + newCol) * 1
        let newBorder = border + input.Steps
        (newRow, newCol, newBorder, newArea)

    let _, _, border, area = List.fold calcArea (0, 0, 0, 0) ans
    int ((float)border / 2. + Math.Abs((float)area) / 2. + 1. )

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
                ; Steps = int parts.Groups.[2].Value
                ; Color = parts.Groups.[3].Value
            }
    )

let execute =
    let path = "day18/day18_input.txt"
    let instructions = LocalHelper.ReadLines(path) |> List.ofSeq |> parseInput
    digHoleFold instructions [| UP; RIGHT; DOWN; LEFT |]