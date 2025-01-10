#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_2017.Modules
open AdventOfCode_Utilities

//let path = "day07/test_input_01.txt"
//let path = "day07/day07_input.txt"

let path = "day07/day07_input - Copy.txt"

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

let getIncorrectValue (group: int * (Tower * int) array) (target: int) (towers: Tower array) =
    printfn "target value: %i" target
    let toBeFixed = towers |> Array.find(fun t -> t.Name = (fst((snd group)[0])).Name)
    Math.Abs(toBeFixed.Weight - Math.Abs((target - (fst group))))

let rec calculateTowerWeight (initTower: Tower) (towers: Tower array) (incorrectValue: int array) : int =
    let t = towers |> Array.find(fun t1 -> t1.Name = initTower.Name)
    if t.Children.Length = 0 then
        t.Weight
    else
        let parentAndChildrenWeights = (t.Children |> Array.map(fun c -> (c, calculateTowerWeight c towers incorrectValue)))
        let wrong = 
            parentAndChildrenWeights 
            |> Array.groupBy (fun p -> (snd p))
            |> Array.sortBy(fun (c, v) -> v.Length)
        if wrong.Length > 1 && incorrectValue[0] = 0 then
            printfn "Found incorrect value!: %A" wrong[0]
            incorrectValue[0] <- getIncorrectValue wrong[0] (snd ((snd wrong[1])[0])) towers
        //printfn "found distinct for %A" (parentAndChildrenWeights |> Array.map(fun c -> sprintf "%s-%i" (fst c).Name (snd c)))
        let childrenweights = parentAndChildrenWeights |> Array.map(fun e -> snd e)
        t.Weight + (Array.sum childrenweights)

let parent = 
    towers 
    |> Array.find(fun t1 -> 
        not (containsTower t1.Name allChildren))

let correctedValue = [|0|]
let groups = 
    parent.Children
    |> Array.map (fun c-> towers |> Array.find (fun t -> t.Name = c.Name))
    |> Array.map (fun c -> (c, calculateTowerWeight c towers correctedValue))
correctedValue