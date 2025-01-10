open System
open System.IO
open System.Text.RegularExpressions

#load @"../../Modules/Utilities.fs"

let path = "day08_input.txt"
//let path = "test_input.txt"
//let path = "test_input_01.txt"

let inputPartsCollection = 
    File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) 
        |> Seq.map(fun l -> l.Trim().Split('|') |> Array.map(fun s -> s.Split(' ') |> Array.filter(fun p -> p <> ""))) |> Seq.toArray

let leds = ["a"; "b"; "c"; "d"; "e"; "f"; "g"];

let numbers = [|[|"a"; "b"; "c"; "e"; "f"; "g"|]; [|"c"; "f"|]; // 0 (6), 1 (2)
                [|"a"; "c"; "d"; "e"; "g"|]; [|"a"; "c"; "d"; "f"; "g"|]; // 2 (5), 3 (5)
                [|"b"; "c"; "d"; "f"|]; [|"a"; "b"; "d"; "f"; "g"|]; // 4 (4), 5 (5)
                [|"a"; "b"; "d"; "e"; "f"; "g"|]; [|"a"; "c"; "f"|]; // 6 (6), 7 (3)
                [|"a"; "b"; "c"; "d"; "e"; "f"; "g"|]; [|"a"; "b"; "c"; "d"; "f"; "g"|]|] // 8 (7), 9 (6)

let crypt (cables: string list) (num: string[][]) =
    [0..num.Length - 1] |> List.map (fun n ->
                                    match n with
                                    | 0 -> [|cables |> List.findIndex(fun c -> c = num.[0].[0]);
                                            cables |> List.findIndex(fun c -> c = num.[0].[1]);
                                            cables |> List.findIndex(fun c -> c = num.[0].[2]);
                                            cables |> List.findIndex(fun c -> c = num.[0].[3]);
                                            cables |> List.findIndex(fun c -> c = num.[0].[4]);
                                            cables |> List.findIndex(fun c -> c = num.[0].[5])|]
                                    | 1 -> [|cables |> List.findIndex(fun c -> c = num.[1].[0]); 
                                            cables |> List.findIndex(fun c -> c = num.[1].[1])|]
                                    | 2 -> [|cables |> List.findIndex(fun c -> c = num.[2].[0]);
                                            cables |> List.findIndex(fun c -> c = num.[2].[1]);
                                            cables |> List.findIndex(fun c -> c = num.[2].[2]);
                                            cables |> List.findIndex(fun c -> c = num.[2].[3]);
                                            cables |> List.findIndex(fun c -> c = num.[2].[4])|]
                                    | 3 -> [|cables |> List.findIndex(fun c -> c = num.[3].[0]);
                                            cables |> List.findIndex(fun c -> c = num.[3].[1]);
                                            cables |> List.findIndex(fun c -> c = num.[3].[2]);
                                            cables |> List.findIndex(fun c -> c = num.[3].[3]);
                                            cables |> List.findIndex(fun c -> c = num.[3].[4])|]
                                    | 4 -> [|cables |> List.findIndex(fun c -> c = num.[4].[0]);
                                            cables |> List.findIndex(fun c -> c = num.[4].[1]);
                                            cables |> List.findIndex(fun c -> c = num.[4].[2]);
                                            cables |> List.findIndex(fun c -> c = num.[4].[3])|]
                                    | 5 -> [|cables |> List.findIndex(fun c -> c = num.[5].[0]);
                                            cables |> List.findIndex(fun c -> c = num.[5].[1]);
                                            cables |> List.findIndex(fun c -> c = num.[5].[2]);
                                            cables |> List.findIndex(fun c -> c = num.[5].[3]);
                                            cables |> List.findIndex(fun c -> c = num.[5].[4])|]
                                    | 6 -> [|cables |> List.findIndex(fun c -> c = num.[6].[0]);
                                            cables |> List.findIndex(fun c -> c = num.[6].[1]);
                                            cables |> List.findIndex(fun c -> c = num.[6].[2]);
                                            cables |> List.findIndex(fun c -> c = num.[6].[3]);
                                            cables |> List.findIndex(fun c -> c = num.[6].[4]);
                                            cables |> List.findIndex(fun c -> c = num.[6].[5])|]
                                    | 7 -> [|cables |> List.findIndex(fun c -> c = num.[7].[0]);
                                            cables |> List.findIndex(fun c -> c = num.[7].[1]);
                                            cables |> List.findIndex(fun c -> c = num.[7].[2])|]
                                    | 8 -> [|cables |> List.findIndex(fun c -> c = num.[8].[0]);
                                            cables |> List.findIndex(fun c -> c = num.[8].[1]);
                                            cables |> List.findIndex(fun c -> c = num.[8].[2]);
                                            cables |> List.findIndex(fun c -> c = num.[8].[3]);
                                            cables |> List.findIndex(fun c -> c = num.[8].[4]);
                                            cables |> List.findIndex(fun c -> c = num.[8].[5]);
                                            cables |> List.findIndex(fun c -> c = num.[8].[6])|]
                                    | 9 -> [|cables |> List.findIndex(fun c -> c = num.[9].[0]);
                                            cables |> List.findIndex(fun c -> c = num.[9].[1]);
                                            cables |> List.findIndex(fun c -> c = num.[9].[2]);
                                            cables |> List.findIndex(fun c -> c = num.[9].[3]);
                                            cables |> List.findIndex(fun c -> c = num.[9].[4]);
                                            cables |> List.findIndex(fun c -> c = num.[9].[5])|]
                                    | _ -> [||]
                        )
let decrypt (key: string) (salt: string list) =
    let translated = crypt salt numbers
    let expectedIndexes = (key.ToCharArray() |> Array.map string) |> Array.map(fun i -> leds |> List.findIndex(fun l -> l = i)) |> Array.toList
    translated |> List.findIndex(fun t -> t |> Array.forall(fun c -> expectedIndexes |> List.contains c) && t.Length = expectedIndexes.Length)

let matchesCables (entry: string) (values: string list) =
    let translated = crypt values numbers
    let expectedIndexes = (entry.ToCharArray() |> Array.map string) |> Array.map(fun i -> leds |> List.findIndex(fun l -> l = i)) |> Array.toList
    let key = translated |> List.filter(fun t -> expectedIndexes.Length = t.Length) |> List.filter(fun t -> expectedIndexes |> List.forall(fun c -> t |> Array.contains c))
    key.Length > 0

let calculateOuput (values:string[], key) =
    (decrypt values.[0] key) * 1000 + (decrypt values.[1] key) * 100 + (decrypt values.[2] key) * 10 + (decrypt values.[3] key)

let processLine(line: string[][]) = 
    let uniquesignalpatterns = (line.[0], line.[1])

    let selector2 = ((fst uniquesignalpatterns) |> Array.find(fun p -> p.Length = 2)).ToCharArray() |> Array.map string
    let indexes2 = selector2 |> Array.map(fun k -> leds |> List.findIndex(fun e -> e = k))

    let tmp3 = (fst uniquesignalpatterns |> Array.find(fun p -> p.Length = 3)).ToCharArray() |> Array.map string
    let selector3 = tmp3 |> Array.filter(fun t -> not((selector2 |> Array.toList) |> List.contains t))
    let indexes3 = leds |> List.findIndex(fun e -> e = selector3.[0])

    let tmp4 = (fst uniquesignalpatterns |> Array.find(fun p -> p.Length = 4)).ToCharArray() |> Array.map string
    let selector4 = tmp4 |> Array.filter(fun t -> not((selector2 |> Array.toList) |> List.contains t) && not((selector3 |> Array.toList) |> List.contains t))
    let indexes4 = [|leds |> List.findIndex(fun e -> e = selector4.[0]); leds |> List.findIndex(fun e -> e = selector4.[1])|]

    let possiblecombinations = Utilities.perms leds |> Seq.toList
    let cablesCandidates = possiblecombinations 
                            |> List.filter(fun l -> (l.Item(indexes2.[0]) = leds.[2] && l.Item(indexes2.[1]) = leds.[5] && l.Item(indexes3) = leds.[0]) ||   
                                                    (l.Item(indexes2.[0]) = leds.[5] && l.Item(indexes2.[1]) = leds.[2] && l.Item(indexes3) = leds.[0]))
    let secondFilter = cablesCandidates |> List.filter(fun l -> l.Item(indexes4.[0]) = leds.[1] && l.Item(indexes4.[1]) = leds.[3] ||
                                                                l.Item(indexes4.[0]) = leds.[3] && l.Item(indexes4.[1]) = leds.[1])

    let result =  secondFilter |> List.find(fun key -> line.[0] |> Array.forall(fun e -> matchesCables e key))
    let value = calculateOuput((snd uniquesignalpatterns), result)
    (result, value)
    
inputPartsCollection |> Array.map(fun l -> processLine l) |> Array.sumBy(fun e -> snd e)