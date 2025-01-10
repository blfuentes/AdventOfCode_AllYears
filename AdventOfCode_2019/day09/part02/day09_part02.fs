module day09_part02

open AoC_2019.Modules

let filepath = __SOURCE_DIRECTORY__ + @"../../day09_input.txt"

let execute =
    (IntCodeModule.getOutput (IntCodeModule.getInput filepath)  0I 0I [2I] false 0I).Output
