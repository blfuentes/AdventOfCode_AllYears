module day14_part02

open System.Collections.Generic
open System.Text

open AdventOfCode_2016.Modules
open System

let md5 = System.Security.Cryptography.MD5.Create()

let hash (s:string) =
    Encoding.Default.GetBytes(s)
    |> md5.ComputeHash
    |> (fun h -> BitConverter.ToString(h).ToLower().Replace("-",""))

let threeInARow s = 
    s
    |> Seq.windowed 3
    |> Seq.tryFind (function | [|a;b;c|] -> a=b && b=c | _ -> false)
    |> Option.map (fun a -> a[0])

let getHash salt n =
    sprintf "%s%d" salt n
    |> hash

let stretchedHash salt n =
    [1..2016] |> List.fold (fun s n -> hash s) (getHash salt n)

let isKey (hashes:string[]) =
    let next1000Contains rpt =
        let find = String(rpt,5)
        [for n in 1..1000 -> hashes[n]] 
        |> Seq.exists (fun h -> h.Contains(find))
    match threeInARow hashes[0] with
    | Some c -> next1000Contains c
    | _ -> false

let solve hasher targetIndex =
    Seq.initInfinite id
    |> Seq.map hasher 
    |> Seq.windowed 1001
    |> Seq.indexed
    |> Seq.filter (snd >> isKey)
    |> Seq.skip (targetIndex-1)
    |> Seq.head
    |> fst

let parseContent(input: string) =
    input

let execute =
    let path = "day14/day14_input.txt"
    let content = LocalHelper.GetContentFromFile path

    let salt = parseContent content
    solve (stretchedHash salt) 64