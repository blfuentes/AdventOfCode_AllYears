open System
open System.IO
open System.Net

let input = File.ReadAllText(__SOURCE_DIRECTORY__ + "/day00_input.txt")
let input2= File.ReadAllLines(__SOURCE_DIRECTORY__ + "/day00_input.txt") |> Array.toList

let download day =
    async {
        let folder = __SOURCE_DIRECTORY__ + day
        let filename = $"{day}.txt"
        let httpClient = new Http.HttpClient()
        let! response = httpClient.GetAsync($"https://adventofcode.com/2022/day/{day}/input") |> Async.AwaitTask
        response.EnsureSuccessStatusCode() |> ignore
        let! fileContent = response.Content.ReadAsByteArrayAsync() |> Async.AwaitTask
        use out = File.Create(Path.Combine(folder, filename))
        out.Write(fileContent)
        out.Close()
    }

download "10"

let getLinesGroupBySeparatorWithEmptySpaceNoReplace (inputLines: string list) (separator: string) =
        let complete = 
            seq {
                for line in inputLines do
                    yield! line.Split(' ')
            } |> List.ofSeq
        let folder (a) (cur, acc) = 
            match a with
            | _ when a <> separator -> a::cur, acc
            | _ -> [], cur::acc 
        let result = List.foldBack folder (complete) ([], [])
        (fst result)::(snd result)

let getLinesGroupBySeparatorWithEmptySpaceFixReplace (inputLines: string list) (separator: string) =
        let validlines = inputLines |> List.map(fun e -> if e = " " then "§" else e)
        let validsplitter =
            match separator = " " with
            | true -> "§"
            | false -> separator
        let complete = 
            seq {
                for line in validlines do
                    yield! line.Split(' ')
            } |> List.ofSeq
        let folder (a) (cur, acc) = 
            match a with
            | _ when a <> validsplitter -> a::cur, acc
            | _ -> [], cur::acc
                
        let result = List.foldBack folder (complete) ([], [])
        (fst result)::(snd result)

let testgetLinesGroupBySeparator0 = getLinesGroupBySeparatorWithEmptySpaceNoReplace ["abceadfapaq"; "asdqwedasca"; " "; "asdasqyhgahfgasdsadasda"] " "
let testgetLinesGroupBySeparator00 = getLinesGroupBySeparatorWithEmptySpaceNoReplace ["abceadfapaq"; "asdqwedasca"; "="; "asdasqyhgahfgasdsadasda"] "="

let testgetLinesGroupBySeparator1 = getLinesGroupBySeparatorWithEmptySpaceFixReplace ["abceadfapaq"; "asdqwedasca"; " "; "asdasqyhgahfgasdsadasda"] " "
let testgetLinesGroupBySeparator11 = getLinesGroupBySeparatorWithEmptySpaceFixReplace ["abceadfapaq"; "asdqwedasca"; "="; "asdasqyhgahfgasdsadasda"] "="

let getLinesGroupBySeparator (inputLines: string list) (separator: string) =
    let validlines = inputLines |> List.map(fun e -> if e = " " then "§" else e)
    let validsplitter =
        match separator = " " with
        | true -> "§"
        | false -> separator
    let complete = 
        seq {
            for line in validlines do
                yield! line.Split(' ')
        } |> List.ofSeq
    let folder (a) (cur, acc) = 
        match a with
        | _ when a <> validsplitter -> a::cur, acc
        | _ -> [], cur::acc
            
    let result = List.foldBack folder (complete) ([], [])
    (fst result)::(snd result)

let groups =  groupByEmptyLines input2 " "
let groups0 =  getLinesGroupBySeparator0 input2 " "
let groups2 =  getLinesGroupBySeparator input2 " "

let testgetLinesGroupBySeparator2 = groupByEmptyLines ["abceadfapaq"; "asdqwedasca"; " "; "asdasqyhgahfgasdsadasda"] " "

let groups = input.Split("\r\n\r\n") |> Array.map(fun a -> a.Split("\r\n"))