open System.IO

let filepath = __SOURCE_DIRECTORY__ + @"../../day02_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input.txt"
let values = File.ReadAllText(filepath).Split(',')
                |> Array.map int

// prepare the tranche input 
Array.set values 1 12
Array.set values 2 2

let execute =
    let mutable continueLooping = true
    let mutable idx = 0
    while continueLooping && idx < values.Length do
        match values.[idx] with
            | 1 -> Array.set values values.[idx + 3] (values.[values.[idx + 1]] + values.[values.[idx + 2]])
            | 2 -> Array.set values values.[idx + 3] (values.[values.[idx + 1]] * values.[values.[idx + 2]])
            | 99 -> continueLooping <- false
            | _ -> ()
        idx <- idx + 4
        //printfn "first position in loop %d = %d" idx values.[0]
    //printfn "first position %d" values.[0]
    values.[0]
execute
