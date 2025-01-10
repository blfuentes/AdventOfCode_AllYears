module day02_part02

open AoC_2019.Modules

let (height, width) = (100, 100)
let __SOLUTION__ = 19690720I

let solutionMatrix = 
    seq {
        for row in 0 .. width - 1 do
            for col in 0 .. height - 1 ->
                (row, col)
        }

let displaySolutionMatrix =
    solutionMatrix
        |> Seq.skipWhile (fun (noum, verb) -> 
            let filepath = __SOURCE_DIRECTORY__ + @"../../day02_input.txt"
            //let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_yavuz.txt"
            let values = IntCodeModule.getInput filepath

            // prepare the tranche input 
            values.[1I] <- noum
            values.[2I] <- verb

            let result = IntCodeModule.getOutput values 0I 0I [0I] false 0I
            values.[0] <> __SOLUTION__
        )
        |> Seq.head 

let execute =   
    match displaySolutionMatrix with
        | (noum, verb) -> noum * 100 + verb