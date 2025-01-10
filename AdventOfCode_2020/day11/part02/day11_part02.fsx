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

let rec findFirstSeat (direction: int[]) (seat: int[]) (originalSeat: int[]) (places: Dictionary<(int*int), SeatInfo>) (limits: int[]) =
    let newSeat = [| seat.[0] + direction.[0]; seat.[1] + direction.[1] |]
    if newSeat.[0] > (limits.[0] - 1) || newSeat.[1] > (limits.[1] - 1) then
        places.[(originalSeat.[0], originalSeat.[1])]
    else       
        match places.ContainsKey(newSeat.[0], newSeat.[1]) with
        | true -> places.[(newSeat.[0], newSeat.[1])]
        | false -> 
            if (newSeat.[0] > -1 && newSeat.[1] > -1) then
                findFirstSeat direction newSeat originalSeat places limits
            else
                match places.ContainsKey((seat.[0], seat.[1])) with
                | true -> places.[(seat.[0], seat.[1])]
                | false -> places.[(originalSeat.[0], originalSeat.[1])]
            

let getNewPlaces (places: Dictionary<(int*int), SeatInfo>) =
    let directions = [ [|-1; -1|]; [|-1; 0|]; [|-1; 1|]; [|0; -1|]; [|0; 1|]; [|1; -1|]; [|1; 0|]; [|1; 1|] ]
    let newPlaces = new Dictionary<(int*int), SeatInfo>()
    for place in places do      
        let seatsToCheck =
            seq{
                for dir in directions do
                    let originalSeat = [| fst place.Value.Location; snd place.Value.Location |]
                    let newSeatPlace = findFirstSeat dir originalSeat originalSeat places [|maxY; maxX|]
                    if newSeatPlace.Location <> place.Value.Location then
                        yield newSeatPlace
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
                if adjOccupied > 4 then
                    { Seat= SeatStatus.EMPTY; Location= (place.Value.Location); Content= 'L'}
                else
                    { Seat= place.Value.Seat; Location= (place.Value.Location); Content= place.Value.Content } 
            | _ -> place.Value
        newPlaces.Add((fst place.Value.Location, snd place.Value.Location), newSeat)
        //places.[(fst place.Value.Location, snd place.Value.Location)] <- newSeat
    newPlaces

let rec round (oldplaces: Dictionary<(int*int), SeatInfo>) (rounds: int) =
    let newplacesDict = getNewPlaces oldplaces
    printfn "Round %i" rounds
    //printPlane newplacesDict
    let planesAreIdentical = oldplaces.Values |> List.ofSeq |> List.forall(fun op -> newplacesDict.[(fst op.Location, snd op.Location)].Seat = op.Seat)
    match planesAreIdentical with
    | true -> newplacesDict |> List.ofSeq |> List.filter(fun s -> s.Value.Seat = SeatStatus.OCCUPIED) |> List.length
    | false -> round newplacesDict (rounds + 1)

round placesDict 0