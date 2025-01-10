module day04_part02

open System.Collections.Generic

open AdventOfCode_Utilities
open AdventOfCode_2020.Modules


let path = "day04/day04_input.txt"
let inputLines = LocalHelper.GetLinesFromFile(path) |> List.ofArray

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

let execute =
    values |> List.filter(fun p -> passPortIsValid p) |> List.length