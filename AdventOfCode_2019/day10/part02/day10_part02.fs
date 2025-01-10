module day10_part02

open System.IO
open System.Collections.Generic
open AoC_2019.Modules

let execute(basePoint:int[]) = 
    let filepath = __SOURCE_DIRECTORY__ + @"../../day10_input.txt"
    //let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_01.txt"
    //let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_05.txt"
    //let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_06.txt"
    let values = File.ReadAllLines(filepath)|> Array.map (fun line -> line.ToCharArray())

    let width = values.[0].Length - 1
    let height = values.Length - 1

    let asteroids =
        seq {
            for idx in [|0..height|] do
                for jdx in [|0 .. width|] do
                    match values.[idx].[jdx] with
                        | '#' -> 
                            //printfn "%d,%d" jdx idx
                            yield [|jdx; idx|]
                        | _  -> ()
        }
    let numberOfPoints = asteroids |> Seq.toList |> List.length
    //let initPoint = [|8; 3|]
    let initAsteroidIdx = asteroids |> Seq.findIndex(fun x -> x.[0] = basePoint.[0] && x.[1] = basePoint.[1])
    let initPoint = asteroids |> Seq.item(initAsteroidIdx)

    let pointsWithAngleDictionary = new Dictionary<int[], float>() 
    let distanceToInitPointDictionary = new Dictionary<int[], float>() 
    let angles = 
        let endIdxs = [|0 .. numberOfPoints - 1|] |> Array.filter (fun x -> x <> initAsteroidIdx)
        for endIdx in endIdxs do
            let endPoint = asteroids |> Seq.item(endIdx)
            distanceToInitPointDictionary.Add(endPoint, getDistance(initPoint, endPoint))
            pointsWithAngleDictionary.Add(endPoint, getAngleBetweenPointsNormalized(initPoint, endPoint))
        pointsWithAngleDictionary.Values |> Seq.distinct |> Seq.sortBy (fun ang -> ang) 
    
    let matchedAsteroids = new List<int[]>()
    let angle = Seq.item (199) angles
    let keys = pointsWithAngleDictionary |> Seq.filter (fun point -> point.Value = angle) |> Seq.map (fun x -> x.Key)
    let possibleAsteroids = asteroids |> Seq.filter(fun ast -> Seq.contains ast keys) 
    let closestDistance = distanceToInitPointDictionary |> Seq.filter (fun dist -> Seq.contains dist.Key possibleAsteroids) |> Seq.sortBy (fun dist -> dist.Value) |> Seq.head
    (closestDistance.Key.[0] * 100 + closestDistance.Key.[1])

    // NOT NEEDED
    //let mutable elementsLeft = 1
    //let matchedAsteroids = new List<int[]>()
    //let mutable output = 0
    //for astIdx in [1..200] do
    //    let angle = Seq.item (astIdx - 1) angles
    //    let keys = pointsWithAngleDictionary |> Seq.filter (fun point -> point.Value = angle) |> Seq.map (fun x -> x.Key)
    //    let possibleAsteroids = asteroids |> Seq.filter(fun ast -> Seq.contains ast keys && not(Seq.contains ast matchedAsteroids)) 
    //    let closestDistance = distanceToInitPointDictionary |> Seq.filter (fun dist -> Seq.contains dist.Key possibleAsteroids) |> Seq.sortBy (fun dist -> dist.Value) |> Seq.head
    //    matchedAsteroids.Add(closestDistance.Key)
    //    match astIdx with
    //    | 200 -> 
    //        output <- (closestDistance.Key.[0] * 100 + closestDistance.Key.[1])
    //        printfn "Asteroid %d at position %A. Value= %d" astIdx (closestDistance.Key) output
    //    | _ -> printfn "Asteroid %d at position %A" astIdx (closestDistance.Key)
    //output  