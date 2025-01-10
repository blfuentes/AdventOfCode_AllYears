module day21_part02

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
    Similar to part 1, except changing (!C && D) to (!(B && C) && D && (E || H))
    The (E || H) part ensures that when we jump to D, that a floor exists a step or a jump after it.
    The !C is changed to !(B && C) since we want to jump immediately on ##.##...#
*)
let part2 =
    [ "OR H J" // J = H
      "OR E J" // J = E || H
      "AND D J" // J = D && (E || H)
      "OR C T" // T = C
      "AND B T" // T = C && B
      "NOT T T" // T = !(C && B)
      "AND T J" // J = !(B && C) && D && (E || H)
      "NOT A T" // T = !A
      "OR T J" // J = !A || (!(B && C) && D && (E || H))
      "RUN" ]

let execute =
    let path = "day21/day21_input.txt"
    let content = (LocalHelper.GetContentFromFile path).Split(",") |> Array.map int64
    solve part2 content