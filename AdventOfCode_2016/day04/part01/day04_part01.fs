module day04_part01

open System
open System.Text.RegularExpressions

open AdventOfCode_2016.Modules.LocalHelper

type CharCount = { Character: char; Count: int }
type Room = { Content: string; Name: string; SectorId: int; Checksum: string; Parts: CharCount array }

let getParts(input: string) =
    let pattern = @"^(.*?)-(\d+)\[(.*?)\]$"
    let regex = Regex(pattern)

    match regex.Match(input) with
    | matchResult when matchResult.Success ->
        let groups = matchResult.Groups
        let stringValueGroups = groups.[1].Value.Replace("-", "").ToCharArray() |> Array.sort |> Array.countBy (fun c -> c)
        let numberValue = groups.[2].Value
        let bracketsContent = groups.[3].Value
        { 
            Content = input;
            Name = groups.[1].Value; 
            SectorId = Int32.Parse(numberValue); 
            Checksum = bracketsContent; 
            Parts = stringValueGroups |> Array.map(fun p -> {Character = fst p; Count = snd p})
        }
    | _ ->
        raise (ArgumentException "Invalid input")

let sortByCountThenByChar(a: CharCount) (b: CharCount) =
    if a.Count = b.Count then 
        compare a.Character b.Character 
    else 
        compare b.Count a.Count

let isRealRoom (room: Room) =
    let sortedParts = (Array.sortWith sortByCountThenByChar room.Parts) |> Array.map (fun p -> p.Character) |> Array.take 5
    room.Checksum = (new String(sortedParts))


let execute =
    let path = "day04/day04_input.txt"
    let inputLines = GetLinesFromFile(path)
    let rooms = inputLines |> Array.map getParts
    let realRooms = rooms |> Array.filter isRealRoom
    realRooms |> Array.sumBy (fun room -> room.SectorId)