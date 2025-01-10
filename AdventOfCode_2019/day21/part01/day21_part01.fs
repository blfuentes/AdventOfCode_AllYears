module day21_part01

open Intcode

open AdventOfCode_Utilities
open AdventOfCode_2019.Modules

let debug = false

let solve program =
    let printAscii = Seq.map char >> charsToStr
    let rec runProg input comp =
        match input, comp with
        | x :: xs, Input f -> f x |> runProg xs
        | x :: xs, Output (o, s) ->
            if debug then printf "%s" (printAscii o)
            s |> runProg input
        | [], Output (o, s) ->
            if debug then printf "%s" (printAscii o)
            o |> List.last
        | _, _ -> failwith "ERR"

    let inputAsStr = ((String.concat "\n" program) + "\n" |> Seq.map int64 |> Seq.toList)
    Computer.create >> run >> runProg inputAsStr

(*
    If A is a hole we must jump or we fall into it
    If C is a hole and D is not a hole we must jump or we fail on ####.#..###
    This can be represented by !A || (!C && D)
*)
let part1 =
    [ "NOT C J" // J = !C
      "AND D J" // J = !C && D
      "NOT A T" // T = !A
      "OR T J" // J = !A || (!C && D)
      "WALK" ]

let execute =
    let path = "day21/day21_input.txt"
    let content = (LocalHelper.GetContentFromFile path).Split(",") |> Array.map int64
    solve part1 content