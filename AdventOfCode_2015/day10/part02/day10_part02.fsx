let rec lookAndSay (round: int) (max: int) (input: string) : string =
    printf "round: %A\n" round
    let rec lookAndSay' (consumed: string) (input: string list) : string =
        //printf "consumed: %A\n" consumed
        //printf "input: %A\n" input
        if input.IsEmpty then consumed
        else
            let initial = input |> List.takeWhile(fun x -> x = input.Head)
            //printfn "initial: %A" initial
            let remaining = input |> List.skipWhile(fun x -> x = input.Head)
            //printfn "remaining: %A" remaining
            let consumed' = initial.Length.ToString() + input.Head
            lookAndSay' (consumed + consumed') remaining
    if round = max then input
    else
        lookAndSay (round + 1) max (lookAndSay' "" (input.ToCharArray() |> Array.map string |> List.ofArray))

let result = lookAndSay 0 50 "1321131112"
printf "%i" result.Length


let nextElement (input : string) =
    printf "input: %s\n" input
    input
    |> Seq.fold (fun acc checker ->
        printf "acc: %A tocompare: %c \n" acc checker
        match acc with
        | (numOfRepeats, currentElement)::tl when checker = currentElement -> 
            // repeated element
            (numOfRepeats+1, currentElement)::tl
        | _ -> 
            // no more repeated elements
            (1, checker)::acc) []
    |> List.rev
    |> Seq.collect (fun (numOfRepeats, element) -> sprintf "%d%c" numOfRepeats element)
    |> fun elements -> System.String.Join("", elements)

//read "1321131112" |> printfn "result %s"º
{1..3}
|> Seq.fold (fun acc _ -> nextElement acc) "1321131112"
|> Seq.length