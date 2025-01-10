open AdventOfCode_2023.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

//let path = "day06/test_input_01.txt"
let path = "day06/day06_input.txt"

let bigint = System.Numerics.BigInteger.Parse

type RaceTime = {
    time: float
    distance: float
}

// brute force
//let holdMakesYouWin (race: RaceTime) (pushTime: bigint) =
//    let remaining = race.time - pushTime
//    let distance = remaining * pushTime
//    race.distance < distance

//let getWinners (race: RaceTime) (timers: bigint array) =
//    timers |> Array.filter (holdMakesYouWin race)

let getWinners (race: RaceTime) =
    let min = ((-1.) * race.time + (Math.Sqrt(Math.Pow(race.time, 2.) - 4. * (race.distance)))) / 2. * (-1.)
    let max = ((-1.) * race.time - (Math.Sqrt(Math.Pow(race.time, 2.) - 4. * (race.distance)))) / 2. * (-1.)
    let min_int = Math.Floor(min + 1.)
    let max_int = Math.Ceiling(max - 1.)
    (int)(max_int - min_int + 1.)

let execute =
    let raceTimes = 
        let lines = GetLinesFromFile path
        let timeLine = lines.[0]
        let distanceTime = lines.[1]
        let times = Regex.Matches(timeLine, @"\d+") |> List.ofSeq |> List.map (fun m -> m.Value) |> String.concat ""
        let distances = Regex.Matches(distanceTime, @"\d+") |> List.ofSeq |> List.map (fun m -> m.Value) |> String.concat ""
        let races = 
            seq {
                yield { time = float (times); distance = float(distances) }
            } |> List.ofSeq
        races
    let results = raceTimes |> List.map(fun r -> getWinners r)
    results |> List.reduce (*)