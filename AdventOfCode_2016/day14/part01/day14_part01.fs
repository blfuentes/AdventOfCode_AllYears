module day14_part01

open AdventOfCode_2016.Modules

open System.Collections.Generic
open System.Text.RegularExpressions
open System.Security.Cryptography
open System.Text

let parseContent(input: string) =
    input

let hexMap = "0123456789abcdef".ToCharArray()
let generateMD5Hash (input: string) =
    let bytes = Encoding.UTF8.GetBytes(input)

    using (MD5.Create()) (fun md5 ->
        let hashBytes = md5.ComputeHash(bytes)

        let hexChars = Array.zeroCreate (hashBytes.Length * 2)
        for i in 0 .. hashBytes.Length - 1 do
            let b = int hashBytes[i] // Convert byte to int
            hexChars.[i * 2] <- hexMap[(b >>> 4) &&& 0xF]
            hexChars.[i * 2 + 1] <- hexMap[b &&& 0xF]
        new string(hexChars)

        //hashBytes
        //|> Array.map (fun b -> b.ToString("x2"))
        //|> String.concat ""
    )

let isKey(salt: string) (index: int) (memo: Dictionary<int, string>)=
    let hashinput = salt + $"{index}"
    let generatedMd5 = 
        if memo.ContainsKey(index) then memo[index] 
        else 
            let newHash = generateMD5Hash hashinput
            memo.Add(index, newHash)
            newHash
    let regexppattern = @"([0-9a-fA-F])\1\1"
    match Regex.Match(generatedMd5, regexppattern) with
    | m when m.Success ->
        ([index+1..index+1000]
        |> List.exists(fun i ->
            let subhasinput = salt + $"{i}"
            let generatedSubMd5 = 
                if memo.ContainsKey(i) then
                    memo[i]
                else
                    let newsubhash = generateMD5Hash subhasinput
                    memo.Add(i, newsubhash)
                    newsubhash
            Regex.IsMatch(generatedSubMd5, @$"({m.Value[0]})\1\1\1\1")
        ), index)
    | _ -> (false, -1)

let find64thKey (salt: string) (memo: Dictionary<int, string>) =
    let mutable currentIdx = 0
    let queue = Stack<int>()
    while queue.Count < 64 do
        match (isKey salt currentIdx memo) with
        | (found, key) when found ->
            queue.Push((currentIdx))
            currentIdx <- currentIdx + 1
        | _ ->
            currentIdx <- currentIdx + 1
    
    queue.Peek()

let execute =
    let path = "day14/day14_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let salt = parseContent content
    let hashmemo = Dictionary<int, string>()
    find64thKey salt hashmemo