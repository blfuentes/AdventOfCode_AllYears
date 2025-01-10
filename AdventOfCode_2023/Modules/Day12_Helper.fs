namespace Day12_Helper

module Day12_Helper =
    let splitClean (delim: char) (s: string) =
        s.Split(
            delim,
            System.StringSplitOptions.RemoveEmptyEntries
            ||| System.StringSplitOptions.TrimEntries
        )

    let uncons xs =
        Seq.tryHead xs |> Option.map (fun x -> (x, Seq.tail xs))

    let intersperse x xs =
        match uncons xs with
        | Some(y, zs) ->
            seq {
                yield y

                for z in zs do
                    yield x
                    yield z
            }
        | None -> Seq.empty


