open System.IO

let filepath = __SOURCE_DIRECTORY__ + @"../../day01_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input.txt"
let lines = File.ReadLines(filepath)

let rec getFuel (massInput:int) =
    match (massInput / 3 - 2) with
    | validmass when validmass >= 0 -> validmass + getFuel (validmass)
    | _ -> 0

let displayValue =
    lines
        |> Seq.sumBy (fun mass -> getFuel(int mass))

displayValue
