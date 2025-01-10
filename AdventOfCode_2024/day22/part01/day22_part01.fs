module day22_part01

open AdventOfCode_2024.Modules

let parseContent(lines: string array) =
    lines |> Array.map int64

let mix value cSecret = value ^^^ cSecret
let prune value = value % 16777216L
let mixPrune value secret = value |> mix secret |> prune

let mul64 value = mixPrune (value * 64L) value
let div32 value = mixPrune (int64 (floor (float (value / 32L)))) value
let mul2048 value = mixPrune (value * 2048L) value

let secret = mul64 >> div32 >> mul2048

let calculateSecretNumber(number: int64) (th: int) =
    let rec getSecret(currentth: int) (current: int64) =
        match currentth = th with
        | true -> 
            current
        | false ->
            getSecret (currentth+1) (secret current)
    
    getSecret 0 number

let execute() =
    let path = "day22/day22_input.txt"

    let content = LocalHelper.GetLinesFromFile path
    let numbers = parseContent content
    let secretnumbers =
        numbers
        |> Array.map(fun s -> calculateSecretNumber s 2000)
    secretnumbers |> Array.sum