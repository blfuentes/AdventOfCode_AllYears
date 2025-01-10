module day05_part02

open System
open System.Text.RegularExpressions

open AdventOfCode_Utilities
open AdventOfCode_2023.Modules.LocalHelper

let path = "day05/day05_input.txt"

type RangeDefinition = { DestRangeStart: bigint; SourceRangeStart: bigint; Length: bigint }

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
    let range = destinationRanges |> List.filter (fun r -> r.DestRangeStart <= source && source < r.DestRangeStart + r.Length)
    match range with
    | [] -> source
    | _ -> range.Head.SourceRangeStart + (source - range.Head.DestRangeStart)

let rec getEnd(destinationRanges: RangeDefinition list list) (source: bigint) =
    match destinationRanges with
    | [] -> source
    | head :: tail -> 
        let destination = getDestination source head
        getEnd tail destination

let rec tryNumber (seedRanges: bigint list list) (currentLocation: bigint) (destinationRanges: RangeDefinition list list) (increment: bigint)=   
    let newSeed = getEnd destinationRanges currentLocation
    let range = seedRanges |> List.filter (fun r -> newSeed >= r.Item(0) && newSeed < (r.Item(0) + r.Item(1)))
    match range with
    | [] -> tryNumber seedRanges (currentLocation + increment) destinationRanges increment
    | head::_ ->
        currentLocation   

let execute =
    let lines = GetLinesFromFile path |> List.ofSeq

    let groups = Utilities.getGroupsOnSeparator lines ""
    let seedDefinition = groups.Head.Head
    let mappingDefinition = groups.Tail
    let seeds = getSeeds seedDefinition
    let mappings = mappingDefinition |> List.map getMappings
    let reverseMapping = mappings |> List.rev
    let pairs = seeds |> List.map bigint |> List.chunkBySize 2

    let increment = 10000I

    let found1 = tryNumber pairs 0I reverseMapping increment
    tryNumber pairs (found1 - increment) reverseMapping 1I