module day14_part02

open System
open System.IO
open System.Collections.Generic

let rec runStep(currentStep: int, steps: int, counter: (string * bigint)[], myrules: Dictionary<string, string[]>) =
    match currentStep = steps with
    | true -> counter       
    | false -> 
        let newCounter = new Dictionary<string, bigint>()
        for rule in myrules do
           newCounter.Add(rule.Key, 0) 
        for co in counter do
            newCounter.Item(myrules.Item(fst co).[0]) <- newCounter.Item(myrules.Item(fst co).[0]) + (snd co)
            newCounter.Item(myrules.Item(fst co).[1]) <- newCounter.Item(myrules.Item(fst co).[1]) + (snd co)
        let tmpCounter = 
            seq {
                for cc in newCounter do
                    yield (cc.Key, cc.Value)
            } |> Seq.toArray
        runStep(currentStep + 1, steps, tmpCounter, myrules)

let run(mypolymer: string, steps: int, pairs: string[], myrules: Dictionary<string, string[]>) =
    let counter = pairs |> Array.countBy(fun p -> p) |> Array.map(fun p -> (fst p,(snd p) |> bigint))
    let resultCounter = runStep(0, steps, counter, myrules)
    let appearances = new Dictionary<string, bigint>()
    for l in "ABCDEFGHIJKLMNOPQRSTUVWXYZ" do
        appearances.Add((string)l, 0I)
    resultCounter |> Array.iter(fun rc -> 
                                    appearances.Item((string)(fst rc).[0]) <- appearances.Item((string)(fst rc).[0]) + (snd rc))

    appearances.Item(mypolymer.Substring(mypolymer.Length - 1, 1)) <- appearances.Item(mypolymer.Substring(mypolymer.Length - 1, 1)) + 1I
    let max = appearances.Values |> Seq.max
    let min = (appearances.Values |> Seq.filter(fun v -> v <> 0I)) |> Seq.min
    max - min

let execute =
    let path = "day14_input.txt"

    let polymertemplate = 
        File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.toList
    
    let parts = polymertemplate |> List.splitAt(polymertemplate |> List.findIndex(fun l -> l = ""))
    let polymer = (fst parts).Head
    let transitions = (snd parts).Tail |> List.map(fun t -> [|t.Split([|" -> "|], StringSplitOptions.None).[0]; t.Split([|" -> "|], StringSplitOptions.None).[1]|])
    
    let rules = new Dictionary<string, string[]>()
    for tran in transitions do
        rules.Add(tran.[0], [|tran.[0].Substring(0, 1) + tran.[1]; tran.[1] + tran.[0].Substring(1, 1)|])
    let pairs = polymer.ToCharArray() |> Array.pairwise |> Array.map(fun c -> ((string)(fst c)) + ((string)(snd c)))
    
    run(polymer, 40, pairs, rules)