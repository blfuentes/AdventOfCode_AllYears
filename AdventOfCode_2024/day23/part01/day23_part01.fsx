#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"

open System
open System.Collections.Generic

open AdventOfCode_Utilities

//let path = "day23/test_input_01.txt"
let path = "day23/day23_input.txt"

let set1 = Set.ofList ["aa"; "bb"]
let set2 = Set.ofList ["bb"; "cc"]
let set3 = Set.ofList ["aa"; "cc"]

let result = Set.unionMany [set1; set2; set3]

result;;
