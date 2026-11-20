open System.IO

let (height, width) = (100, 100)
let __SOLUTION__ = 19690720

let solutionMatrix = 
    seq {
        for row in 0 .. width - 1 do
            for col in 0 .. height - 1 ->
                (row, col)
        }

let performOperation (values: int[]) =
    let mutable continueLooping = true
    let mutable idx = 0
    while continueLooping && idx < values.Length do
        match values.[idx] with
            | 1 -> Array.set values values.[idx + 3] (values.[values.[idx + 1]] + values.[values.[idx + 2]])
            | 2 -> Array.set values values.[idx + 3] (values.[values.[idx + 1]] * values.[values.[idx + 2]])
            | 99 -> continueLooping <- false
            | _ -> ()
        idx <- idx + 4
    values.[0]

let printOutput (noum: int, verb: int) =
    printfn "What is 100 * %d + %d? %d" noum verb ((100 * noum) + verb)

let displaySolutionMatrix =
    solutionMatrix
        |> Seq.skipWhile (fun (noum, verb) -> 
            let filepath = __SOURCE_DIRECTORY__ + @"../../day02_input.txt"
            let values = File.ReadAllText(filepath).Split(',')
                            |> Array.map int
            
            // prepare the tranche input 
            Array.set values 1 noum
            Array.set values 2 verb

            performOperation values <> __SOLUTION__
        )
        |> Seq.head 

let execute =   
    match displaySolutionMatrix with
        | (noum, verb) -> noum * 100 + verb

execute
