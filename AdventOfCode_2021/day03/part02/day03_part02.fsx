open System
open System.IO

//let path = "day03_input.txt"
let path = "test_input.txt"
let inputLines = File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.map (fun x -> x.ToCharArray() |> Array.map string) |> Seq.toList

let calculateRate(input: list<string[]>, criteria: string, noncriteria: string, op: (int -> int -> bool)) =
    let rate = [0 .. input.Head.Length - 1] 
                |> List.map(fun idx -> input |> List.filter (fun e -> e.[idx] = criteria) |> List.length)
    rate |> List.map(fun r -> if op r (input.Length - r) then criteria else noncriteria) |> List.toArray

let rec caculateCriteriaRating(criteria: string, noncriteria: string, input: List<string[]>, pos: int, op: (int -> int -> bool)) =
    match input with
    | [x] -> Convert.ToInt32(x |> Array.fold (+) "", 2)
    | _ -> 
        let newCriteria = calculateRate(input, criteria, noncriteria, op)
        caculateCriteriaRating(criteria, noncriteria, (input |> List.filter(fun x -> x.[pos] = newCriteria.[pos])), pos + 1, op)

let oxygengeneratorrating = caculateCriteriaRating ("1", "0", inputLines, 0, (>=))
let co2scrubberrating = caculateCriteriaRating ("0", "1", inputLines, 0, (<=))

oxygengeneratorrating * co2scrubberrating