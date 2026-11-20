module day10_part02

let nextElement (input : string) =
    input
    |> Seq.fold (fun acc checker ->
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

let execute =
    {1..50}
    |> Seq.fold (fun acc _ -> nextElement acc) "1321131112"
    |> Seq.length