module day05_part02

open System.Numerics
open AoC_2019.Modules


let filepath = __SOURCE_DIRECTORY__ + @"../../day05_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input.txt"

let execute(input:bigint) =
    let values = IntCodeModule.getInput filepath
    (IntCodeModule.getOutput values 0I 0I [input] false 0I).Output