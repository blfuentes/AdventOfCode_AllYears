
#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic

open AdventOfCode_2017.Modules
open AdventOfCode_Utilities

type GarbageState = NotGarbage | Garbage | Cancelled
type State = { level: int; state: GarbageState; score: int; garbage: int }

let step current nextChar =
    match (current.state, nextChar) with
    | (Garbage, '!') -> { current with state = Cancelled }
    | (Garbage, '>') -> { current with state = NotGarbage } 
    | (Garbage, _)   -> { current with garbage = current.garbage + 1 }
    | (Cancelled, _) | (NotGarbage, '<') -> { current with state = Garbage }
    | (NotGarbage, '{') -> { current with level = current.level + 1 }
    | (NotGarbage, '}') -> { current with level = current.level - 1; score = current.score + current.level }
    | _ -> current;

let solve = Seq.fold step { level=0; state=NotGarbage; score=0; garbage=0 }
let solvePart1 = solve >> (fun state -> state.score)

//let path = "day09/test_input_01.txt"
let path = "day09/day09_input.txt"
let input = LocalHelper.GetContentFromFile path

solvePart1 input