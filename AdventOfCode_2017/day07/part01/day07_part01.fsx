#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_2017.Modules
open AdventOfCode_Utilities

//let path = "day07/test_input_01.txt"
let path = "day07/day07_input.txt"

let input = LocalHelper.GetLinesFromFile path

type Tower = {
    Name: string
    Weight: int
    Children: Tower array
}

let createTower(name: string) (weight: int) (children: Tower array) : Tower =
    { Name = name; Weight = weight; Children = children }

let extractTower(line: string) : Tower =
    let parts = line.Split("->")
    let towerpattern = @"(\w+) \((\d+)\)"
    let towermatch = Regex.Match(parts[0], towerpattern)
    let defTower = createTower(towermatch.Groups[1].Value.Trim()) ((int)towermatch.Groups[2].Value) Array.empty
    let children = 
        if parts.Length > 1 then
            parts[1].Split(", ")
            |> Array.map(fun t -> createTower (t.Trim()) 0 Array.empty)
        else
            Array.empty

    { defTower with  Children = children }
    

let parseInput(lines: string array) : Tower array =
    lines |> Array.map extractTower

let containsTower (name: string) (towers: Tower array): bool =
  towers |> Array.map _.Name |> Array.contains name  

let towers = parseInput input

let allChildren = 
    Array.concat (towers |> Array.map _.Children)

towers 
|> Array.find(fun t1 -> 
    not (containsTower t1.Name allChildren))