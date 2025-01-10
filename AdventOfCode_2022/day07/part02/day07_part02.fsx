open AdventOfCode_2022.Modules.LocalHelper
open AoC_2022.Modules

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/DataModels.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities
open AdventOfCode_Utilities

//let path = "day07/test_input_01.txt"
let path = "day07/day07_input.txt"

let inputLines = GetLinesFromFile(path) |> Seq.toList

let createElement (parent: option<FileSystemItem>) (input: string) =
    let newElement =
        match input.Split(' ') with
        | parts when parts.[0] = "dir" -> 
            let directory = { 
                Name = parts.[1]; 
                NodeType = NodeEnum.Directory;
                Size = 0;
                Parent = if parent.IsSome then parent.Value.Path else "";
                Path = if parent.IsSome then parent.Value.Path + "§" + parts.[1] else ""
            }
            directory
        | parts -> 
            let file = { 
                Name = parts.[1]; 
                NodeType = NodeEnum.File;
                Size= (int)(parts.[0]);
                Parent = if parent.IsSome then parent.Value.Path else "";
                Path = if parent.IsSome then parent.Value.Path + "§" + parts.[1] else ""
            }
            file
    newElement

let rec updateParent (elements: FileSystemItem list) (current: FileSystemItem) (size: int) =
    let newCurrent = {
        Name = current.Name;
        NodeType = current.NodeType;
        Size = current.Size + size;
        Parent = current.Parent;
        Path = current.Path
    }
    let newElements =
        match (elements |> List.tryFindIndex(fun e -> e.Path = current.Path)) with
        | Some currenIdx -> 
            Utilities.updateElement currenIdx newCurrent elements
        | None -> elements
    match (newElements |> List.tryFindIndex(fun e -> e.Path = newCurrent.Parent)) with
    | Some currenIdx -> 
        updateParent newElements (newElements.Item(currenIdx)) size
    | None -> newElements

let rec parseInstruction 
    (elements: FileSystemItem list) (current: FileSystemItem) (instructions: string list) =
    match instructions with 
    | head :: tail -> 
        match head.StartsWith("$") with
        | true -> 
            match head.Split(' ') with
            | opParts when opParts.Length = 3 && opParts.[2] = ".." -> // "$ cd .."
                let newCurrent = elements |> List.find(fun e -> e.Path = current.Parent)
                parseInstruction elements newCurrent tail                
            | opParts when opParts.Length = 3 -> // "$ cd /" or "$ cd <dir>"
                match opParts.[2] with
                | "/" -> // "$ cd /"
                    parseInstruction elements elements.Head tail
                | _ -> // "$ cd <dir>"
                    let newCurrent = elements |> List.find(fun e -> e.Path = current.Path + "§" + opParts.[2])
                    parseInstruction elements newCurrent tail
            | _ -> // $ ls
                parseInstruction elements current tail // $ ls
        | false -> 
            let newElement = createElement (Some(current)) head
            let tmpElements = 
                if elements |> List.exists(fun e -> e.Path = newElement.Path) then
                    elements
                else
                    elements @ [newElement]   
            let newElements =
                match newElement.Size with
                | 0 -> tmpElements
                | _ -> updateParent tmpElements current newElement.Size           
            let newCurrent = newElements |> List.find(fun e -> e.Path = current.Path)
            parseInstruction newElements newCurrent tail
    | [] -> elements

let startElement = { 
    Name = "/"; 
    NodeType = NodeEnum.Directory;
    Size = 0;
    Parent = "";
    Path = $"/"
}

let structure = parseInstruction [startElement] startElement inputLines
structure |> List.filter(fun f -> f.Size <=100000 && f.NodeType = NodeEnum.Directory) |> List.sumBy(fun e-> e.Size)

let totalSpace = 70000000
let requiredSpace = 30000000
let usedSpace = structure.Head.Size
let availableSpace = totalSpace - usedSpace
let requiredAdditionalSpace = requiredSpace - availableSpace
let folderToDelete = structure |> List.filter(fun f -> f.NodeType = NodeEnum.Directory) |> List.sortBy(fun f -> f.Size) |> List.find(fun e -> e.Size >= requiredAdditionalSpace)
folderToDelete.Size