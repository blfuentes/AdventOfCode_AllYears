module day09_part01

open AoC_2019.Modules

let filepath = __SOURCE_DIRECTORY__ + @"../../day09_input.txt"

let execute =
    (IntCodeModule.getOutput (IntCodeModule.getInput filepath)  0I 0I [1I] false 0I).Output