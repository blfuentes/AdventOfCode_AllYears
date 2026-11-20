module day10_part01

let rec lookAndSay (round: int) (max: int) (input: char array) : char array =
    let rec lookAndSay' (consumed: char array) (input: char array) : char array =
        if input.Length = 0 then consumed
        else
            let initial = input |> Array.takeWhile(fun x -> x = input.[0])
            let remaining = input |> Array.skipWhile(fun x -> x = input.[0])
            let consumed' = [|((char)(initial.Length.ToString())); input.[0] |]
            lookAndSay' (consumed' |> Array.append consumed) remaining
    if round = max then input
    else
        let newInput = lookAndSay' Array.empty input
        lookAndSay (round + 1) max newInput

let execute =
    (lookAndSay 0 40 ("1321131112".ToCharArray())).Length
