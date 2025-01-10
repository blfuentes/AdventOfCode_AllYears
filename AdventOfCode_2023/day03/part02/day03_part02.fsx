open AdventOfCode_2023.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

//let path = "day03/test_input_01.txt"
let path = "day03/day03_input.txt"

type FoundNum = {
    number: string
    row: int
    col: int
}

let (|Int|_|) (str:string) =
    match System.Int32.TryParse str with
    | true,int -> Some int
    | _ -> None

let isNumber (input: string): bool =
    match input with
    |Int i -> true 
    |_ -> false

let isSymbol (input: string): bool =
    match input with
    |Int i -> false 
    |_ -> if input = "." then false else true

let addToSymbols (symbols: string[]) (input: string) =
    if Array.contains input symbols then symbols
    else Array.append symbols [|input|]

let buildSchematicEngine (schematic: string[,]) (lines: string[]) (symbols: string[])=
    for i in 0..lines.Length-1 do
        for j in 0..lines.[0].Length-1 do
            schematic.[i,j] <- lines.[i].[j].ToString()

let printSchematic (schematic: string[,]) =
    for i in 0..schematic.GetLength(0)-1 do
        for j in 0..schematic.GetLength(1)-1 do
            printf "%s" schematic.[i,j]
        printfn ""

let checkSymbol(schematic: string[,]) (row: int) (col: int) =
    //printf "Check element %i %i " row col
    if (row >= 0 && col >= 0 && row < schematic.GetLength(0) && col < schematic.GetLength(1)) then
        let element = schematic.[row, col]
        //printf "Element %s " element
        if isNumber element then 
            //printfn "is symbol"

            true 
        else 
            //printfn "is not symbol"
            false
    else
        //printfn "Out of bounds"
        false

let transformPart (input: string array) =
    let value = String.Join("", input)
    let number = Regex.Match(value, "\d+")
    match number with
    | m when m.Success -> Int32.Parse(m.Value)
    | _ -> 0

let getCheckers (input:string array) =
    let check =
        match input |> Array.map isNumber with
        // 1 1 1 X 1 1 1 (3 - 3)
        | [|  true;   true;  true;  false;  true;  true;   true |] -> [| Array.sub input 0 3; Array.sub input 4 3 |]
        // 1 1 1 X 1 1 X (3 - 2)
        | [|  true;   true;  true;  false;  true;  true;  false |] -> [| Array.sub input 0 3; Array.sub input 4 2 |]
        // 1 1 1 X 1 X - (3 - 1)
        | [|  true;   true;  true;  false;  true; false;      _ |] -> [| Array.sub input 0 3; Array.sub input 4 1 |]
        // 1 1 1 X X - - (3 - 0)
        | [|  true;   true;  true;  false; false;     _;      _ |] -> [| Array.sub input 0 3 |]

        // X 1 1 X 1 1 1 (2 - 3)
        | [| false;   true;  true;  false;  true;  true;   true |] -> [| Array.sub input 1 2; Array.sub input 4 3 |]
        // - 1 1 X 1 1 - (2 - 2)
        | [|     _;   true;  true;  false;  true;  true;      _ |] -> [| Array.sub input 1 2; Array.sub input 4 2 |]
        // - 1 1 X 1 X - (2 - 1)
        | [|     _;   true;  true;  false;  true; false;      _ |] -> [| Array.sub input 1 2; Array.sub input 4 1 |]
        // - 1 1 X X - - (2 - 0)
        | [|     _;   true;  true;  false; false;     _;      _ |] -> [| Array.sub input 1 2 |]

        // - X 1 X 1 1 1 (1 - 3)
        | [|     _;  false;  true;  false;  true;  true;   true |] -> [| Array.sub input 2 1; Array.sub input 4 3 |]
        // - X 1 X 1 1 X (1 - 2)
        | [|     _;  false;  true;  false;  true;  true;  false |] -> [| Array.sub input 2 1; Array.sub input 4 2 |]
        // - X 1 X 1 X - (1 - 1)
        | [|     _;  false;  true;  false;  true; false;      _ |] -> [| Array.sub input 2 1; Array.sub input 4 1 |]
        // - X 1 X X - - (1 - 0)
        | [|     _;  false;  true;  false; false;     _;      _ |] -> [| Array.sub input 2 1 |]

        // - - X X 1 1 1 (0 - 3)
        | [|     _;      _; false;  false;  true;  true;   true |] -> [| Array.sub input 4 3 |]
        // - - X X 1 1 X (0 - 2)
        | [|     _;      _; false;  false;  true;  true;  false |] -> [| Array.sub input 4 2 |]
        // - - X X 1 X X (0 - 1)
        | [|     _;      _; false;  false;  true; false;  false |] -> [| Array.sub input 4 1 |]
           
        // - X 1 1 1 X - (1)
        | [|     _;  false;  true;   true;  true; false;      _ |] -> [| Array.sub input 2 3 |]
        // - 1 1 1 X - - (1)
        | [|     _;   true;  true;   true; false;     _;      _ |] -> [| Array.sub input 1 3 |]
        // - X 1 1 X X - (1)
        | [|     _;  false;  true;   true; false; false;      _ |] -> [| Array.sub input 2 2 |]
        // - X 1 1 X - - (1)
        | [|     _;  false;  true;   true; false;     _;      _ |] -> [| Array.sub input 2 2 |]
        // - X X 1 X X - (1)
        | [|     _;  false; false;   true; false; false;      _ |] -> [| Array.sub input 3 1 |]
        // - X X 1 1 X - (1)
        | [|     _;  false; false;   true;  true; false;      _ |] -> [| Array.sub input 3 2 |]
        // - - X 1 1 X - (1)
        | [|     _;      _; false;   true;  true; false;      _ |] -> [| Array.sub input 3 2 |]
        // - - X 1 1 1 - (1)
        | [|     _;      _; false;   true;  true;  true;      _ |] -> [| Array.sub input 3 3 |]
        | _ -> [||]
    check

let getGear (schematic: string[,]) (row: int) (col: int) =
    let currentSurrounding =
        seq {
            // check up
            let up = Array.sub schematic.[row - 1, *] (col - 3) 7
            let checkUp = getCheckers up
            for c in checkUp do
                yield transformPart c

            // check down
            let down = Array.sub schematic.[row + 1, *] (col - 3) 7
            let checkDown = getCheckers down
            for c in checkDown do
                yield transformPart c

            // check left middle
            if schematic[row, col - 1] <> "." then
                let middleLeft = Array.sub schematic.[row, *] (col - 3) 3
                let ml = transformPart middleLeft
                if ml <> 0 then
                    yield ml
            // check right middle
            if schematic[row, col + 1] <> "." then
                let middleRight = Array.sub schematic.[row, *] (col + 1) 3
                let mr = transformPart middleRight
                if mr <> 0 then
                    yield mr
                
        } |> Seq.toList
    currentSurrounding

let processSchematic (schematic: string[,]) =
    let regex = Regex("(\*)")
    let numbers =
        seq {
            for i in 0..schematic.GetLength(0) - 1 do
                let matches = regex.Matches(String.Join("",schematic.[i,*]))
                let stars = matches |> Seq.map(fun m -> { number = m.Value; row = i; col = m.Index })
                
                for star in stars do
                    let checkGear = getGear schematic star.row star.col
                    if checkGear.Length = 2 then
                        //printfn "Found number %A" foundNumber
                        yield (checkGear |> List.reduce (*))
                    else
                        //printfn "Symbol * at %i % i is not a gear" star.row star.col
                        ()
        }
    numbers

//let path = "day03/test_input_01.txt"
//let path = "day03/test_input_02.txt"
let lines = GetLinesFromFile path
let engineSchematic = Array2D.create lines.Length lines.[0].Length ""
buildSchematicEngine engineSchematic lines [||]
//printSchematic engineSchematic
let linkedNumbers = processSchematic engineSchematic
linkedNumbers |> Seq.sum