open System
open System.IO

// let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_00.txt"
// let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_04.txt"
// let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_05.txt"
// let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_06.txt"
let filepath = __SOURCE_DIRECTORY__ + @"../../day16_input.txt"

let input = File.ReadAllText(filepath).ToCharArray() |> Array.map (string >> int)
let basePattern = [| 0; 1; 0; -1 |]

let calculateInput(input:int[]) =
    let reversed = input |> Array.rev
    for x in [ 1 .. input.Length - 1 ] do
        reversed.[x] <- Math.Abs((reversed.[x - 1] + reversed.[x]) % 10)
    reversed |> Array.rev

let rec convertInput(input:int[], offSet: int, numberOfPhases: int, currentPhase: int) =
    match currentPhase = numberOfPhases with
    | true -> input |> Array.take(8) |> Array.map string |> String.concat ""
    | false -> convertInput(calculateInput(input), offSet, numberOfPhases, currentPhase + 1)

let solution =     
    let realInput = input |> Array.replicate 10000 |> Array.concat
    let offSet = realInput |> Array.take(7) |> Array.map string |> String.concat "" |> int    
    // (realInput, realInput.Length, offSet, (realInput |> Array.skip(offSet)).Length)
    convertInput((realInput |> Array.skip(offSet)), offSet,  100, 0)