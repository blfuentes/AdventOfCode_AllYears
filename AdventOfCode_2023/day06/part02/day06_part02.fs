module day06_part02

open System
open System.Text.RegularExpressions

open AdventOfCode_2023.Modules.LocalHelper

let path = "day06/day06_input.txt"

type RaceTime = {
    time: float
    distance: float
}

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