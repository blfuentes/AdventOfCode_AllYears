module day22_part02

open AdventOfCode_2024.Modules
open System.Collections.Generic

let parseContent(lines: string array) =
    lines |> Array.map int64

let mix value cSecret = value ^^^ cSecret
let prune value = value % 16777216L
let mixPrune value secret = value |> mix secret |> prune

let mul64 value = mixPrune (value * 64L) value
let div32 value = mixPrune (int64 (floor (float (value / 32L)))) value
let mul2048 value = mixPrune (value * 2048L) value

let secret = mul64 >> div32 >> mul2048

let calculateSecretNumber(number: int64) (th: int) (index: int) (mapOfBananaPrices: (int64*int64)[,])=   
    let rec getSecret(currentth: int) (prev: int64) (current: int64) =
        match currentth = th with
        | true -> 
            current
        | false ->
            let newSecret = secret current
            let price = newSecret % 10L
            mapOfBananaPrices[index, currentth] <- (price, (price - prev))
            getSecret (currentth+1) price newSecret
    
    getSecret 0 0 number

let getMostBananasOverall(mapOfBananaPrices: (int64 * int64)[,]) =
    let mapOfSeqs = Dictionary<struct (int64 * int64 * int64 * int64), int64[]>()
    let (maxrows, maxcols) = (mapOfBananaPrices.GetLength(0), mapOfBananaPrices.GetLength(1))
    
    for row in 0..(maxrows - 1) do
        let mutable seqBuffer = 
            [| snd mapOfBananaPrices[row, 0]; snd mapOfBananaPrices[row, 1]; snd mapOfBananaPrices[row, 2] |]
        
        for col in 3..(maxcols - 1) do
            let fourth = snd mapOfBananaPrices[row, col]
            let key = struct (seqBuffer[0], seqBuffer[1], seqBuffer[2], fourth)
            
            if not (mapOfSeqs.ContainsKey(key)) then
                mapOfSeqs.[key] <- Array.zeroCreate maxrows
            
            if mapOfSeqs.[key].[row] = 0L then
                mapOfSeqs.[key].[row] <- fst mapOfBananaPrices[row, col]

            seqBuffer <- [| seqBuffer[1]; seqBuffer[2]; fourth |]
    
    mapOfSeqs.Values
    |> Seq.map Array.sum
    |> Seq.max

let execute() =
    let path = "day22/day22_input.txt"

    let content = LocalHelper.GetLinesFromFile path
    let numbers = parseContent content
    
    let mapOfBananaPrices = Array2D.create content.Length 2000 (0L, 0L)
    
    let _ =
        numbers
        |> Array.mapi(fun idx s -> calculateSecretNumber s 2000 idx mapOfBananaPrices)
    getMostBananasOverall mapOfBananaPrices