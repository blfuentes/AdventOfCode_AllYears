module day19_part02

open AdventOfCode_2022.Modules
open System.Text.RegularExpressions
open System.Collections.Generic

let input =
    let path = "day19/day19_input.txt"
    let robotRe =
        Regex
            @"Blueprint (\d+): Each ore robot costs (\d+) ore. Each clay robot costs (\d+) ore. Each obsidian robot costs (\d+) ore and (\d+) clay. Each geode robot costs (\d+) ore and (\d+) obsidian."
   

    LocalHelper.GetLinesFromFile path
    |> Array.map(fun line ->
        let m = robotRe.Match line
    
        let values =  [ for g in (m.Groups |> Seq.tail) -> int g.Value ] |> Array.ofSeq
        values
    )

let addState (a1, a2, a3, a4, a5, a6, a7, a8, a9) (b1, b2, b3, b4, b5, b6, b7, b8, b9) =
    (a1 + b1, a2 + b2, a3 + b3, a4 + b4, a5 + b5, a6 + b6, a7 + b7, a8 + b8, a9 + b9)

let mine (time, oreR, clayR, obsR, geoR, ore, clay, obs, geo) =
    addState (time, oreR, clayR, obsR, geoR, ore, clay, obs, geo) (-1, 0, 0, 0, 0, oreR, clayR, obsR, geoR)

let makeGeode (bp: int []) state =
    addState (mine state) (0, 0, 0, 0, 1, -bp[5], 0, -bp[6], 0)

let makeObsidian (bp: int []) state =
    addState (mine state) (0, 0, 0, 1, 0, -bp[3], -bp[4], 0, 0)

let makeClay (bp: int []) state =
    addState (mine state) (0, 0, 1, 0, 0, -bp[2], 0, 0, 0)

let makeOre (bp: int []) state =
    addState (mine state) (0, 1, 0, 0, 0, -bp[1], 0, 0, 0)

let maxOres =
    input
    |> Array.map (fun bp -> bp[0], [ bp[1]; bp[2]; bp[3]; bp[5] ] |> List.max)
    |> Map.ofArray

let mineUntil predicate state =
    Seq.unfold
        (fun st ->
            let (time, _, _, _, _, _, _, _, _) = st

            if time = 0 || predicate st then
                None
            else
                let next = mine st
                Some(next, next))
        state
    |> Seq.last

let cache =
    Dictionary<(int * (int * int * int * int * int * int * int * int * int)), int>()

let best = Dictionary<int, int>()

// With infinite resources, this is the max amount of geodes in t time
let maxGeodesInTimeRemaining = Array.init 33 (fun t -> (t - 1) * t / 2)

// Discard resources that are impossible to be used to maximize cache hits
let discardExtraResources (bp: int []) (time, oreR, clayR, obsR, geoR, ore, clay, obs, geo) =
    let newOre = min ore (time * maxOres[bp[0]] - oreR * (time - 1))
    let newClay = min clay (time * bp[4] - clayR * (time - 1))
    let newObs = min obs (time * bp[6] - obsR * (time - 1))
    (time, oreR, clayR, obsR, geoR, newOre, newClay, newObs, geo)

let neighbors (bp: int []) state =
    let (time, oreR, clayR, obsR, geoR, ore, clay, obs, geo) = state

    [ if ore >= bp[5] && obs >= bp[6] then
          makeGeode bp state
      elif obsR > 0 then
          mineUntil (fun (_, _, _, _, _, o, _, ob, _) -> o >= bp[5] && ob >= bp[6]) state

      if obsR < bp[6] then
          if ore >= bp[3] && clay >= bp[4] then
              makeObsidian bp state
          elif clayR > 0 then
              mineUntil (fun (_, _, _, _, _, o, c, _, _) -> o >= bp[3] && c >= bp[4]) state

          if clayR < bp[4] then
              if ore >= bp[2] then
                  makeClay bp state
              else
                  mineUntil (fun (_, _, _, _, _, o, _, _, _) -> o >= bp[2]) state

      if oreR < maxOres[bp[0]] then
          if ore >= bp[1] then
              makeOre bp state
          else
              mineUntil (fun (_, _, _, _, _, o, _, _, _) -> o >= bp[1]) state ]
    |> List.filter (fun (t, _, _, _, gR, _, _, _, g) -> g + gR * t + maxGeodesInTimeRemaining[t] > best[bp[0]])
    |> List.map (discardExtraResources bp)

let tryMax seq =
    if Seq.isEmpty seq then
        0
    else
        Seq.max seq

let rec bestGeo (bp: int []) state =
    if not (best.ContainsKey bp[0]) then
        best[bp[0]] <- 0

    match cache.TryGetValue((bp[0], state)) with
    | true, value -> value
    | false, _ ->
        let value =
            neighbors bp state
            |> List.map (fun (time, oreR, clayR, obsR, geoR, ore, clay, obs, geo) ->
                if time = 0 then
                    best[bp[0]] <- max best[bp[0]] geo
                    geo
                else
                    bestGeo bp (time, oreR, clayR, obsR, geoR, ore, clay, obs, geo))
            |> tryMax

        cache.Add((bp[0], state), value)
        value

let part2 =
    input
    |> Array.truncate 3
    |> Array.map (fun bp -> bestGeo bp (32, 1, 0, 0, 0, 0, 0, 0, 0))
    |> Array.reduce (*)

let execute =
    let path = "day19/day19_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    part2