module day10_part01

open System.IO
open System.Collections.Generic
open AoC_2019.Modules

let execute = 
    let filepath = __SOURCE_DIRECTORY__ + @"../../day10_input.txt"
    //let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_05.txt"
    let values = File.ReadAllLines(filepath)|> Array.map (fun line -> line.ToCharArray())

    let width = values.[0].Length - 1
    let height = values.Length - 1

    let asteroids =
        seq {
            for idx in [|0..height|] do
                for jdx in [|0 .. width|] do
                    match values.[idx].[jdx] with
                        | '#' -> yield [|jdx; idx|]
                        | _  -> ()
        }
    let numberOfPoints = asteroids |> Seq.toList |> List.length

    let pointsDictionary = new Dictionary<(int*int), int>()
    for initIdx in [|0 .. numberOfPoints - 1|] do
        let endIdxs = [|0 .. numberOfPoints - 1|] |> Array.filter (fun x -> x <> initIdx)
        for endIdx in endIdxs do
            let initPoint = asteroids |> Seq.item(initIdx)
            let endPoint = asteroids |> Seq.item(endIdx)
            let blockers = findPossibleBlockers(asteroids, initPoint, endPoint)
            let walls = blockers |> Seq.filter (fun midPoint -> isBlockedByLine(initPoint, endPoint, midPoint))
            let notvalid = blockers |> Seq.exists (fun midPoint -> isBlockedByLine(initPoint, endPoint, midPoint))
            let addValue =
                match notvalid with
                | true -> 0
                | false -> 1
            let found, content = pointsDictionary.TryGetValue ((initPoint.[0], initPoint.[1]))
            match found with 
            | true -> pointsDictionary.[(initPoint.[0], initPoint.[1])] <- content + addValue
            | false -> pointsDictionary.Add((initPoint.[0], initPoint.[1]), addValue)
    let converted =
        pointsDictionary
        |> Seq.map (fun (KeyValue(k,v)) -> (k, v))
    //converted |> Seq.iter (fun elem -> printfn "%A - %d" (fst elem) (snd elem))
    let result = converted |> Seq.maxBy (fun x -> snd x)
    snd result
