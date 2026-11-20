let validlines = ["a "; "b"; " "; "c"; " "; "d"; "e"]
let complete = 
    seq {
        for line in validlines do
            yield! line.Split(' ')
    } |> List.ofSeq

let getgroups (inputLines: 'a list) (separator: 'a) =
    let folder (a) (cur, acc) = 
        match a with
        | _ when a <> separator -> a::cur, acc
        | _ -> [], cur::acc 
    let result = List.foldBack folder (inputLines) ([], [])
    (fst result)::(snd result)

getgroups validlines " "

#r @"nuget: FSharpPlus"
open FSharpPlus

List.split [[" "]] ["abceadfapaq"; "asdqwedasca"; " "; "asdasqyhgahfgasdsadasda"]

let splitWhen predicate list =
    let folder state t =
        if predicate t then
            [] :: state
        else
            (t :: state.Head) :: state. Tail

    list 
    |> List.fold folder [ [] ] 
    |> List.map List.rev
    |> List.rev

["a"; "b"; " "; "c "; " "; "d"; "e"] |> splitWhen(fun x -> x = " ")


for i in [1..9] do
    printfn "%i" i