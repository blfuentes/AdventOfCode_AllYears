open AdventOfCode_2016.Modules

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

//let path = "day04/test_input_01.txt"
let path = "day04/day04_input.txt"

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
    //printfn "Sorted parts: %s - checksum: %s" (new String(sortedParts)) room.Checksum
    //let outputString = sprintf "Room: %s - Sorted parts: %A - ToBeChecked: %A - Checksum: %s \r\n \r\n" 
    //                    room.Content (room.Parts |> Array.sortByDescending (fun p -> p.Count) |> Array.map(fun p -> $"{p.Character}, {p.Count}")) sortedParts room.Checksum
    let isRealRoom = room.Checksum = (new String(sortedParts))
    //if isRealRoom then
    //    System.IO.File.AppendAllText("./../output_rooms.txt", outputString)
    //else
    //    System.IO.File.AppendAllText("./../output_decoys.txt", outputString)
    isRealRoom

let decrypherText (input: string) (shifter: int) =
    let numberOfShifts = shifter % 26
    new String((input.ToCharArray() |> Array.map (fun c -> 
    if c = '-' then ' ' 
    else 
        let newValue = (int)c + numberOfShifts
        let newChar = (char)newValue
        if newValue > 122 then 
            (char)(96 + (numberOfShifts - (122 - (int)c)))
        else
            newChar
        )))

//let test = "qzmt-zixmtkozy-ivhz-"
//let shift = 343

//decrypherText test shift

let decypherRoom (room: Room) =
    decrypherText room.Name room.SectorId

let inputLines = LocalHelper.GetLinesFromFile(path)
let rooms = inputLines |> Array.map getParts
let realRooms = rooms |> Array.filter isRealRoom
//realRooms |> Array.iter(fun r -> System.IO.File.AppendAllText("./../output_decypher_rooms.txt", sprintf "%s \r\n" (decypherRoom r)))
let northpolestorage = (realRooms |> Array.find(fun r -> (decypherRoom r).StartsWith("northpole"))).SectorId
//let sumSectorsId = realRooms |> Array.sumBy (fun room -> room.SectorId)
//printfn "Sum of sector IDs: %d" sumSectorsId