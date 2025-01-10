open System.Collections.Generic
open CustomDataTypes
open AdventOfCode_2020.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Model/CustomDataTypes.fs"
#load @"../../Modules/Helpers.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities
open AdventOfCode_2020.Modules.Helpers
open AdventOfCode_Utilities

//let file = "test_input.txt"
let file = "day11_input.txt"
let path = "day11/" + file
let inputLines = GetLinesFromFile(path)

let maxX = inputLines.[0].Length
let maxY = inputLines.Length

let placesDict = new Dictionary<int*int, SeatInfo>()

let plane =
    seq {
        for jdx in [0..maxY - 1] do
            for idx in [0..maxX - 1] do
                let seat =
                    match inputLines.[jdx].[idx] with
                    | '.' -> SeatStatus.FLOOR
                    | 'L' -> SeatStatus.EMPTY
                    | '#' -> SeatStatus.OCCUPIED
                    | _ -> failwith("wrong site type")
                let seatToAdd = { Seat = seat; Location = (jdx, idx); Content = inputLines.[jdx].[idx] }
                if seatToAdd.Seat <> SeatStatus.FLOOR then
                    placesDict.Add((jdx, idx), seatToAdd)
                yield seatToAdd
    } |> List.ofSeq

let printPlane (dictPlane: Dictionary<(int*int), SeatInfo>)= 
    for idx in [0 .. maxY - 1] do
        for jdx in [0 .. maxX - 1] do
            if dictPlane.ContainsKey(idx, jdx) then
                printf "%c" dictPlane.[(idx, jdx)].Content
            else
                printf "."
        printfn ""

let getNewPlaces (places: Dictionary<(int*int), SeatInfo>) =
    let newplacesDict = new Dictionary<int*int, SeatInfo>()
    for place in places do      
        let seatsToCheck =
            seq {
                for idx in [(fst place.Value.Location) - 1 .. (fst place.Value.Location) + 1] do
                    for jdx in [(snd place.Value.Location) - 1 .. (snd place.Value.Location) + 1] do
                        if places.ContainsKey(idx, jdx) && (fst place.Value.Location <> idx || snd place.Value.Location <> jdx) then
                            yield places.[(idx, jdx)]
            } |> List.ofSeq
        let adjOccupied = seatsToCheck |> List.filter(fun s -> s.Seat = SeatStatus.OCCUPIED) |> List.length
        let newSeat = 
            match place.Value.Seat with
            | SeatStatus.EMPTY -> 
                if adjOccupied = 0 then
                    { Seat= SeatStatus.OCCUPIED; Location= (place.Value.Location); Content= '#'}
                else
                    { Seat= place.Value.Seat; Location= (place.Value.Location); Content= place.Value.Content } 
            | SeatStatus.OCCUPIED -> 
                if adjOccupied > 3 then
                    { Seat= SeatStatus.EMPTY; Location= (place.Value.Location); Content= 'L'}
                else
                    { Seat= place.Value.Seat; Location= (place.Value.Location); Content= place.Value.Content } 
            | _ -> place.Value
        newplacesDict.Add((fst place.Value.Location, snd place.Value.Location), newSeat)
    newplacesDict

let rec round (oldplaces: Dictionary<(int*int), SeatInfo>) (rounds: int) =
    let newplacesDict = getNewPlaces oldplaces
    printfn "Round %i" rounds
    //printPlane newplacesDict
    let planesAreIdentical = oldplaces.Values |> List.ofSeq |> List.forall(fun op -> newplacesDict.[(fst op.Location, snd op.Location)].Seat = op.Seat)
    match planesAreIdentical with
    | true -> newplacesDict |> List.ofSeq |> List.filter(fun s -> s.Value.Seat = SeatStatus.OCCUPIED) |> List.length
    | false -> round newplacesDict (rounds + 1)

round placesDict 0