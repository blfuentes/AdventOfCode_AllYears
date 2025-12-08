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
    let timesPath = sprintf "leaderboard_%d.json" year
    let getFromNetwork =
        let sessionKey = LocalHelper.GetLinesFromFile "session.txt"
        let url = sprintf "https://adventofcode.com/%d/leaderboard/private/view/%s.json" year sessionKey[0]
        let client = new HttpClient()
        client.DefaultRequestHeaders.Add("Cookie", sprintf "session=%s" sessionKey[1])
        let mutable content = client.GetStringAsync(url).Result
        LocalHelper.WriteContentToFile(timesPath, content)
        System.Text.Json.JsonSerializer.Deserialize<Leaderboard>(content)
    let leaderboard =
        if LocalHelper.FileExists(timesPath) then
            let content = LocalHelper.GetContentFromFile timesPath
            if content.Length > 0 then
                System.Text.Json.JsonSerializer.Deserialize<Leaderboard>(content)
            else
                getFromNetwork
        else
            getFromNetwork
    leaderboard
            

let formatTimer (ts: int64) =
    let dt = System.DateTimeOffset.FromUnixTimeSeconds(ts).ToLocalTime()
    dt.ToString("yyyy-MM-dd HH:mm:ss")

let printLeaderboard (leaderboard: Leaderboard) =
    System.Console.OutputEncoding <- System.Text.Encoding.UTF8
    printfn "%s" (String.replicate 53 "=")
    let totalWidth = 53
    let innerWidth = totalWidth - 2  // Subtract the ‡ borders
    let title = sprintf "Advent of Code %s" leaderboard.event
    let centeredTitle = title.PadLeft((innerWidth + title.Length) / 2).PadRight(innerWidth)
 
    printfn "‡%s‡" centeredTitle
    printfn "%s" (String.replicate 53 "=")
    printfn "‡%3d %-46s ‡" leaderboard.num_days  (sprintf"days event %s" leaderboard.event)
    for kvp in leaderboard.members do
        let memberId = kvp.Key
        let participant = kvp.Value
        printfn "‡ Member: %-42s‡" (if participant.name <> null then participant.name else memberId.ToString())
        printfn "‡ Stars: %-42s ‡" (sprintf "%s (%d/%d) (%d%%)" (sprintf"%s%s" (String.replicate (participant.stars) "★")(String.replicate(leaderboard.num_days * 2 - participant.stars) "*")) (participant.stars) (leaderboard.num_days * 2) (participant.stars * 100 / (leaderboard.num_days * 2)))
        printfn "%s" (String.replicate 53 "=")
        printfn "‡ %3s ‡ %-20s ‡ %-20s ‡" "Day" "1st Star" "2nd Star"
        printfn "%s" (String.replicate 53 "=")
        let starDays = 
            kvp.Value.completion_day_level
            |> Seq.map (fun daystar -> (int)daystar.Key, daystar.Value)
            |> Seq.sortBy fst
        for day in 1 .. leaderboard.num_days do
            if starDays |> Seq.exists (fun (d, _) -> d = day) |> not then
                printfn "‡ %3d ‡ %-20s ‡ %-20s ‡" day "" ""
            else
                let infoDay = starDays |> Seq.tryFind (fun (k, _) -> k = day)
                let firstStarOutput =
                    match infoDay with
                    | Some (firstDay, firstDayInfo) ->
                        match firstDayInfo.``1`` with
                        | Some star1 ->
                            sprintf "%s" (formatTimer(star1.get_star_ts))
                        | None -> ""
                    | None -> ""
                let secondStarOutput =
                    match infoDay with
                    | Some (secondDay, secondDayInfo) ->
                        match secondDayInfo.``2`` with
                        | Some star2 ->
                            sprintf "%s" (formatTimer(star2.get_star_ts))
                        | None -> ""
                    | None -> ""
                printfn "‡ %3d ‡ %-20s ‡ %-20s ‡" (if infoDay.IsSome then fst infoDay.Value else 0) firstStarOutput secondStarOutput
        printfn "%s" (String.replicate 53 "=")
let leaderboard = getTimes 2025
printLeaderboard leaderboard

