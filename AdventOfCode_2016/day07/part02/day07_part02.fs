module day07_part02

open AdventOfCode_Utilities
open System.Text.RegularExpressions
open AdventOfCode_2016.Modules

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

let isInner (line: string) (part: string) =
    let pattern = "\[\w*"+part+"\w*\]"
    let regexp = new Regex(pattern)
    let matches = regexp.IsMatch(line)

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

    matchesright || matchesleft || matchesmid


let execute =
    let path = "day07/day07_input.txt"
    let input = LocalHelper.GetLinesFromFile path
    let parts = (input |> Array.map(fun l -> (l, findParts l)))
    let notEmptyParts = parts |> Array.filter(fun (l, p) -> (not (List.isEmpty p)))
    let ssls = 
        notEmptyParts 
        |> Array.filter(fun (l, p) -> 
            p |> List.exists(fun (p1, p2) -> 
                ((isInner l p1) && (isOuter l p2)) ||
                ((isInner l p2) && (isOuter l p1)))
        )
    ssls |> Array.length