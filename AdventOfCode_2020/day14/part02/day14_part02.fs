module day14_part02

open System.Text.RegularExpressions

open AdventOfCode_2020.Modules
open System.Collections.Generic

type MaskGroup = {
    Mask: char array
    Memory: (int64*int64) list
}

let parseContent(input: string array) =
    let mutable idx = 0
    let mutable currentGroup = { Mask = [||]; Memory = [] }
    let mutable currentBlocks = []
    [
        while idx < input.Length do
            if input[idx].StartsWith("mask") then
                let mask = (input[idx].Split(" ")[2]).ToCharArray()
                if currentBlocks.Length = 0 then
                    currentGroup <- { currentGroup with Mask = mask }
                else
                    yield { currentGroup with Memory = currentBlocks }
                    currentGroup <- { currentGroup with Mask = mask }
                    currentBlocks <- []
            else
                let regexpattern = @"mem\[(\d+)] = (\d+)"
                let groups = Regex.Match(input[idx], regexpattern).Groups
                let (target, towrite) = ((int64)(groups[1].Value), (int64)(groups[2].Value))
                currentBlocks <- currentBlocks @ [(target, towrite)]
            idx <- idx + 1
        yield { currentGroup with Memory = currentBlocks }
    ]

let binarynumber (number: int64) =
    System.Convert.ToString(number, 2).PadLeft(36, '0').ToCharArray()

let rec generateBinaryStrings (chars: char array) index =
    if index = chars.Length then
        [ new string(chars) ]
    else
        match chars.[index] with
        | 'X' ->
            chars[index] <- '0'
            let withZero = generateBinaryStrings chars (index + 1)
            
            chars[index] <- '1'
            let withOne = generateBinaryStrings chars (index + 1)
            
            chars[index] <- 'X'
            
            withZero @ withOne
        | _ ->
            generateBinaryStrings chars (index + 1)  

let runBlock(maskgroup: MaskGroup) (memory: Dictionary<int64, int64>) =
    maskgroup.Memory
    |> List.iter(fun (m, v) ->
        let memorydir = binarynumber m
        let maskedm =
            String.concat "" ((Array.map2 (fun v1 v2 ->
                match v1 with
                | 'X' -> 'X'
                | '0' -> v2
                | '1' -> '1'
            ) maskgroup.Mask memorydir) |> Array.map string)

        let alldirections = generateBinaryStrings (maskedm.ToCharArray()) 0
        alldirections
        |> List.iter(fun d ->
            let mtoinsert = System.Convert.ToInt64(d, 2)
            if memory.ContainsKey(mtoinsert) then
                memory[mtoinsert] <- v
            else
                memory.Add(mtoinsert, v)
        )
    )

let memorzyValue (maskgroups: MaskGroup list) =
    let memory = Dictionary<int64, int64>()
    maskgroups |> List.iter(fun m ->  runBlock m memory)
    memory.Values |> Seq.sum

let execute =
    let path = "day14/day14_input.txt"
    let content = LocalHelper.GetLinesFromFile(path)
    let maskgroups = parseContent content

    memorzyValue maskgroups