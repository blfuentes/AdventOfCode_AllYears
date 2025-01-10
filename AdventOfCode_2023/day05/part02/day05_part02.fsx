open AdventOfCode_2023.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

type RangeDefinition = { DestRangeStart: bigint; SourceRangeStart: bigint; Length: bigint }

let path = "day05/test_input_01.txt"
//let path = "day05/day05_input.txt"
let lines = GetLinesFromFile path |> List.ofSeq

let groups = Utilities.getGroupsOnSeparator lines ""
let seedDefinition = groups.Head.Head
let mappingDefinition = groups.Tail

let bigint = System.Numerics.BigInteger.Parse

let getSeeds (input: string) =
    Regex.Matches(input, "\d+") |> Seq.cast<Match> |> Seq.map (fun m -> m.Value) |> Seq.toList

let getMappings (input: string list) =
    let mappings =
        seq {
            for line in input.Tail do
                let parts = line.Split(" ")

                let range = { DestRangeStart = bigint (parts.[0]); SourceRangeStart = bigint parts.[1]; Length = bigint parts.[2] }
                yield range
        } |> Seq.toList
    mappings



let getDestination (source: bigint) (destinationRanges: RangeDefinition list) =
    let range = destinationRanges |> List.filter (fun r -> r.SourceRangeStart <= source && source < r.SourceRangeStart + r.Length)
    match range with
    | [] -> source
    | _ -> range.Head.DestRangeStart + (source - range.Head.SourceRangeStart)

let rec getEnd(destinationRanges: RangeDefinition list list) (source: bigint) =
    match destinationRanges with
    | [] -> source
    | head :: tail -> 
        let destination = getDestination source head
        getEnd tail destination

let rec tryNumber (seedRanges: bigint list list) (currentLocation: bigint) (destinationRanges: RangeDefinition list list) =
    let newLocation = getEnd (destinationRanges |> List.rev) currentLocation
    let range = seedRanges |> List.filter(fun r -> r |> List.contains newLocation)
    match range with
    | [] -> tryNumber seedRanges (currentLocation + 1I) destinationRanges
    | head::_ ->
        if newLocation >= currentLocation then
            currentLocation
        else
            tryNumber seedRanges (currentLocation + 1I) destinationRanges

let seeds = getSeeds seedDefinition
let mappings = mappingDefinition |> List.map getMappings

let pairs = seeds |> List.map bigint |> List.chunkBySize 2
let allSeedRanges = pairs |> List.map (fun p -> [p.Item(0)..p.Item(0)+p.Item(1)])


tryNumber allSeedRanges 0I mappings

//let results = allnumbers |> List.map (fun s -> getEnd mappings s)
//results |> List.min