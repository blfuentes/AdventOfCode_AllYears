open System.IO
open System.Collections.Generic
open System
open AdventOfCode_2020.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"
#load @"../../Model/CustomDataTypes.fs"
#load @"../../Modules/Helpers.fs"

open AdventOfCode_2020.Modules.Helpers

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

//let file = "test_input.txt"
//let file = "test_input_invalid.txt"
//let file = "test_input_valid.txt"
//let file = "test_input_valid01.txt"
//let file = "test_input_valid02.txt"
//let file = "test_input_valid03.txt"
//let file = "test_input_valid04.txt"
let file = "day04_input.txt"
let path = "day04/" + file
let inputLines = GetLinesFromFile(path) |> List.ofArray

let allFields = [|"byr"; "iyr"; "eyr"; "hgt"; "hcl"; "ecl"; "pid"; "cid"|]
let requiredFields = [|"byr"; "iyr"; "eyr"; "hgt"; "hcl"; "ecl"; "pid"|]

let passportList = new List<List<string>>()
passportList.Add(new List<string>())

let values = getLinesGroupBySeparator2 inputLines ""

let passPortIsValid (credentials: string list) =
    let allFieldsRequired = requiredFields |> Array.forall (fun field -> credentials |> List.exists(fun cred -> cred.StartsWith(field)))
    let cred = credentials |> Array.ofSeq
    let valueIsCorret = cred |> Array.forall (fun field ->
        let parts = field.Split(':')
        match parts with
        | [|"byr"; thevalue|] -> byrValid thevalue
        | [|"iyr"; thevalue|] -> iyrValid thevalue
        | [|"eyr"; thevalue|] -> eyrValid thevalue
        | [|"hgt"; thevalue|] -> hgtValid thevalue
        | [|"hcl"; thevalue|] -> hclValid thevalue
        | [|"ecl"; thevalue|] -> eclValid thevalue
        | [|"pid"; thevalue|] -> pidValid thevalue
        | _ -> true
    ) 
    valueIsCorret && allFieldsRequired

let validPassports  = values |> List.filter(fun p -> passPortIsValid p)
validPassports |> List.length