module day09_part02

open AdventOfCode_2017.Modules

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

let folder = Seq.fold step { level=0; state=NotGarbage; score=0; garbage=0 }
let solver = folder >> (fun state -> state.garbage)

let execute() =
    let path = "day09/day09_input.txt"
    let input = LocalHelper.GetContentFromFile path
    solver input