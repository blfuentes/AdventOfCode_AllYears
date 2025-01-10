let rec lookAndSay (round: int) (max: int) (input: string) : string =
    printf "round: %A\n" round
    let rec lookAndSay' (consumed: string) (input: string list) : string =
        //printf "consumed: %A\n" consumed
        //printf "input: %A\n" input
        if input.IsEmpty then consumed
        else
            let initial = input |> List.takeWhile(fun x -> x = input.Head)
            printfn "initial: %A" initial
            let remaining = input |> List.skipWhile(fun x -> x = input.Head)
            printfn "remaining: %A" remaining
            let consumed' = initial.Length.ToString() + input.Head
            let newConsumed = consumed + consumed'
            printfn "consumed': %A" newConsumed
            lookAndSay' newConsumed remaining
    if round = max then input
    else
        lookAndSay (round + 1) max (lookAndSay' "" (input.ToCharArray() |> Array.map string |> List.ofArray))

let result = lookAndSay 0 3 "1"
printf "%i" result.Length


let rec lookAndSay2 (round: int) (max: int) (input: char array) : char array =
    printfn "Round: %i" round
    let rec lookAndSay' (consumed: char array) (input: char array) : char array =
        if input.Length = 0 then consumed
        else
            let initial = input |> Array.takeWhile(fun x -> x = input.[0])
            printfn "initial: %A" initial
            let remaining = input |> Array.skipWhile(fun x -> x = input.[0])
            printfn "remaining: %A" remaining
            let consumed' = [|((char)(initial.Length.ToString())); input.[0] |]
            let newConsumed = consumed' |> Array.append(consumed)
            printfn "consumed': %A" newConsumed
            lookAndSay' newConsumed remaining
    if round = max then input
    else
        let newInput = lookAndSay' Array.empty input
        lookAndSay2 (round + 1) max newInput

let result2 = lookAndSay2 0 3 ("1".ToCharArray())
printf "%i" result2.Length