open AdventOfCode_2023.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

//let path = "day06/test_input_01.txt"
//let path = "day06/test_input_02.txt"
let path = "day06/day06_input.txt"

type RaceTime = {
    time: float
    distance: float
}

// brute force
//let holdMakesYouWin (race: RaceTime) (pushTime: int) =
//    let remaining = race.time - pushTime
//    let distance = remaining * pushTime
//    race.distance < distance

//let getWinners (race: RaceTime) (timers: int array) =
//    timers |> Array.filter (holdMakesYouWin race)

let getWinners (race: RaceTime) =
    let min = ((-1.) * race.time + (Math.Sqrt(Math.Pow(race.time, 2.) - 4. * (race.distance)))) / 2. * (-1.)
    let max = ((-1.) * race.time - (Math.Sqrt(Math.Pow(race.time, 2.) - 4. * (race.distance)))) / 2. * (-1.)
    printfn "min: %f, max: %f" min max
    let min_int = Math.Floor(min + 1.)
    let max_int = Math.Ceiling(max - 1.)
    (int)(max_int - min_int + 1.)

let raceTimes = 
    let lines = GetLinesFromFile path
    let timeLine = lines.[0]
    let distanceTime = lines.[1]
    let times = Regex.Matches(timeLine, @"\d+")
    let distances = Regex.Matches(distanceTime, @"\d+")
    let races = 
        seq {
            for idx in 0..times.Count-1 do
                yield { time = float (times.[idx].Value); distance = float (distances.[idx].Value) }
        } |> List.ofSeq
    races
let results = raceTimes |> List.map(fun r -> getWinners r)
results |> List.reduce (*)