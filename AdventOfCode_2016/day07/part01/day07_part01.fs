module day07_part01

open AdventOfCode_2016.Modules
open System.Text.RegularExpressions

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
    matches

let execute =
    let path = "day07/day07_input.txt"
    let input = LocalHelper.GetLinesFromFile path
    (input 
        |> Array.map(fun l -> (l, findParts l)))
        |> Array.filter(fun (l, p) -> 
            not (Seq.isEmpty p) && 
            p |> Seq.forall(fun part -> not (isInner l part))) 
        |> Array.length