#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_2016.Modules
open AdventOfCode_Utilities

//let path = "day07/test_input_02.txt"
let path = "day07/day07_input.txt"

let input = LocalHelper.GetLinesFromFile path

let findParts (line: string) : (string*string) list =
    let parts = 
        seq {
            for i in 0 .. line.Length - 3 do
                let p = line.Substring(i, 3)
                if not (p.Contains("[")) && not (p.Contains("]")) then
                    let tmp = p.ToCharArray()
                    if tmp.[0] = tmp.[2] && tmp.[0] <> tmp.[1] then
                        yield p
        } |> Seq.toList

    let perms = 
        (Utilities.combination 2 parts)
        |> List.map(fun comb -> (comb.[0], comb.[1]))
        |> List.filter(fun comb -> (fst comb).[0] = (snd comb).[1] && (snd comb).[0] = (fst comb).[1])
    perms

//findParts "aba[bab]xyz"
//findParts "xyx[xyx]xyx"
//findParts "aaa[kek]eke"
//findParts "zazbz[bzb]cdb"
//findParts "zazbz[bzb]cdb"

let isInner (line: string) (part: string) =
    let pattern = "\[\w*"+part+"\w*\]"
    let regexp = new Regex(pattern)
    let matches = regexp.IsMatch(line)
    //if matches then
    //    printfn "pattern: %s for line %s - is inner %b" pattern line matches
    matches

let isOuter (line: string) (part: string) =
    let patternright = "\]\w*"+part+"\w*\[*"
    let regexpright = new Regex(patternright)
    let matchesright = regexpright.IsMatch(line)

    let patternleft = "\w*"+part+"\w*\["
    let regexprleft = new Regex(patternleft)
    let matchesleft = regexprleft.IsMatch(line)

    let patternmid = "\]\w*"+part+"\w*\["
    let regexpmid = new Regex(patternmid)
    let matchesmid = regexpmid.IsMatch(line)
    //if matches then
    //    printfn "pattern: %s for line %s - is inner %b" pattern line matches
    matchesright || matchesleft || matchesmid

//isInner "aba[bab]xyz" "aba"
//isInner "aba[bab]xyz" "bab"

let parts = (input |> Array.map(fun l -> (l, findParts l)))
let notEmptyParts = parts |> Array.filter(fun (l, p) -> (not (List.isEmpty p)))
let ssls = 
    notEmptyParts 
    |> Array.filter(fun (l, p) -> 
        p |> List.exists(fun (p1, p2) -> 
            ((isInner l p1) && (isOuter l p2)) ||
            ((isInner l p2) && (isOuter l p1)))
    )
ssls |> Array.iter(fun (l, p) -> printfn "%s: %A" l p)
ssls |> Array.length
