open System.Net.Http
open AdventOfCode_Api.Modules
open System.Collections.Generic

type StarInfo = {
    star_index: int
    get_star_ts: int64
}

type DayLevel = {
    ``1``: StarInfo option
    ``2``: StarInfo option
}

type Member = {
    last_star_ts: int64
    local_score: int
    completion_day_level: Dictionary<string, DayLevel>
    stars: int
    id: int
    name: string
}

type Leaderboard = {
    num_days: int
    members: Dictionary<string, Member>
    owner_id: int
    event: string
    day1_ts: int64
}

// https://adventofcode.com/{year}/leaderboard/private/view/{XXXXX}.json
let getTimes year =    
    let sessionKey = LocalHelper.GetLinesFromFile "session.txt"
    let url = sprintf "https://adventofcode.com/%d/leaderboard/private/view/%s.json" year sessionKey[0]
    let client = new HttpClient()
    client.DefaultRequestHeaders.Add("Cookie", sprintf "session=%s" sessionKey[1])
    let mutable content = client.GetStringAsync(url).Result
    System.Text.Json.JsonSerializer.Deserialize<Leaderboard>(content)

let formatTimer (ts: int64) =
    let dt = System.DateTimeOffset.FromUnixTimeSeconds(ts).ToLocalTime()
    dt.ToString("yyyy-MM-dd HH:mm:ss")

let printLeaderboard (leaderboard: Leaderboard) =
    printfn "%d days event %s" leaderboard.num_days leaderboard.event
    for kvp in leaderboard.members do
        let memberId = kvp.Key
        let participant = kvp.Value
        printfn "Member %s (ID: %d) has %d stars, last star at %s" (if participant.name <> null then participant.name else "Anonymous") participant.id participant.stars (formatTimer(participant.last_star_ts))
        let starDays = 
            kvp.Value.completion_day_level
            |> Seq.map (fun daystar -> (int)daystar.Key, daystar.Value)
            |> Seq.sortBy fst
        for (day, dayInfo) in starDays do
            match dayInfo.``1`` with
            | Some star1 ->
                printfn "  Day %d Star 1 at %s" day (formatTimer(star1.get_star_ts))
            | None -> ()
            match dayInfo.``2`` with
            | Some star2 ->
                printfn "  Day %d Star 2 at %s" day (formatTimer(star2.get_star_ts))
            | None -> ()
let leaderboard = getTimes 2025
printLeaderboard leaderboard

