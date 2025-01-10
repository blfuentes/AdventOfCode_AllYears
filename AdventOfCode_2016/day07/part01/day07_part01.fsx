#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_2016.Modules
open AdventOfCode_Utilities

//let path = "day07/test_input_01.txt"
let path = "day07/day07_input.txt"

let input = LocalHelper.GetLinesFromFile path

let findParts (line: string) =
    let parts = seq {
        for i in 0 .. line.Length - 4 do
            let p = line.Substring(i, 4)
            if not (p.Contains("[")) && not (p.Contains("]")) then
                let tmp = p.ToCharArray()
                if tmp.[0] = tmp.[3] && tmp.[1] = tmp.[2] && tmp.[0] <> tmp.[1] then
                    yield p
    }
    parts

let isInner (line: string) (part: string) =
    let pattern = "\[\w*"+part+"\w*\]"
    let regexp = new Regex(pattern)
    let matches = regexp.IsMatch(line)
    //printfn "pattern: %s for line %s - is inner %b" pattern line matches
    matches

//isInner "abba[mnop]qrst" "abba"
//isInner "abcd[bddb]xyyx" "bddb"
//isInner "abcd[bddb]xyyx" "xyyx"
//isInner "aaaa[qwer]tyui" "aaaa"
//isInner "ioxxoj[asdfgh]zxcvbn" "oxxo"

(input |> Array.map(fun l -> (l, findParts l)))
    |> Array.filter(fun (l, p) -> not (Seq.isEmpty p) && p |> Seq.forall(fun part -> not (isInner l part))) |> Array.length
