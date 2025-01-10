module day10_part01

open AdventOfCode_2017.Modules

let rec processLengths (state: int array) (lengths: int list) (currentskip: int) (currentindex: int) =
    match lengths with
    | [] -> state
    | totake :: rest ->
        let indexes = 
            seq {
                for idx in [currentindex..currentindex + totake - 1] do
                    yield (state[idx % state.Length], idx % state.Length)
            }

        Seq.iter2(fun el rev -> state[snd el] <- fst rev) indexes (indexes |> Seq.rev)

        processLengths state rest (currentskip + 1) ((currentindex + totake + currentskip) % state.Length)

let execute() =
    let path = "day10/day10_input.txt"
    let content = (LocalHelper.GetContentFromFile path).Split(",") |> Array.map int |> List.ofArray
    let size = 256
    let circular = [|0 .. size - 1|]
    processLengths circular content 0 0 |> ignore
    circular |> Array.take(2) |> Array.reduce (*)