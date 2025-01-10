namespace AdventOfCode_2017.Modules

open System.IO

module LocalHelper =
    let GetLinesFromFile(path: string) =
        File.ReadAllLines(__SOURCE_DIRECTORY__ + @"../../" + path)

    let GetContentFromFile(path: string) =
        File.ReadAllText(__SOURCE_DIRECTORY__ + @"../../" + path)

    let ReadLines(path: string) =
        File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path)

    let knotHasher(text: string) =
        let rec processLengths (roundsleft: int) (state: int array) (initiallengths: int list) (lengths: int list) (currentskip: int) (currentindex: int) =
            match lengths with
            | [] -> 
                if roundsleft > 0 then
                    processLengths (roundsleft - 1) state initiallengths initiallengths currentskip currentindex
                else
                    state
            | totake :: rest ->
                let indexes = 
                    seq {
                        for idx in [currentindex..currentindex + totake - 1] do
                            yield (state[idx % state.Length], idx % state.Length)
                    }

                Seq.iter2(fun el rev -> state[snd el] <- fst rev) indexes (indexes |> Seq.rev)

                processLengths roundsleft state  initiallengths rest (currentskip + 1) ((currentindex + totake + currentskip) % state.Length) 

        let extra = "17, 31, 73, 47, 23".Split(", ") |> Array.map int |> List.ofArray
        let content = List.concat([text.ToCharArray() |> Array.map int |> List.ofArray; extra])
        let size = 256
        let rounds = 64
        let circular = [|0 .. size - 1|]
        processLengths (rounds - 1)  circular content content 0 0 |> ignore
        let hexadecimals = 
            circular 
                |> Array.chunkBySize(16)    
                |> Array.map(fun c -> sprintf "%02X" (c |> Array.reduce (^^^)))
        (String.concat "" hexadecimals).ToLowerInvariant()