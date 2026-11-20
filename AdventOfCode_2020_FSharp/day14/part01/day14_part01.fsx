open System.Collections.Generic
open AdventOfCode_2020_FSharp.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities_FSharp/Modules/Utilities.fs"
#load @"../../Model/CustomDataTypes.fs"
#load @"../../Modules/Helpers.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities_FSharp
open AdventOfCode_2020_FSharp.Modules.Helpers
open AdventOfCode_Utilities_FSharp

//let file = "test_input.txt"
let file = "day14_input.txt"
let path = "day14/" + file
let inputLines = GetLinesFromFile(path)