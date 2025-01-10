module day12_part01

open AoC_2019.Modules
open System.IO

let extractCoordinates input =
    match input with
    | Regex @"<\w=([-]?\d+),[-. ]?\w=([-]?\d+),[-. ]?\w=([-]?\d+)>" [ area; prefix; suffix ] ->
        [| area; prefix; suffix |] |> Array.map int
    | _ -> [||]

let applyGravities(moonA:(int[]*int[]), moonB:(int[]*int[])) =
    let (posA, velA) = moonA
    let (posB, velB) = moonB
    let velocityA = [|velA.[0] + tern(posA.[0], posB.[0]); velA.[1] + tern(posA.[1], posB.[1]); velA.[2] + tern(posA.[2], posB.[2])|]
    let velocityB = [|velB.[0] + tern(posB.[0], posA.[0]); velB.[1] + tern(posB.[1], posA.[1]); velB.[2] + tern(posB.[2], posA.[2])|]
    let resultA = (posA, velocityA)
    let resultB = (posB, velocityB)
    (resultA, resultB)

let applyVelocities(moonA:(int[]*int[]), moonB:(int[]*int[])) =
    let (posA, velA) = moonA
    let (posB, velB) = moonB
    let gravityA = [|posA.[0] + tern(velA.[0], velB.[0]); posA.[1] + tern(velA.[1], velB.[1]); posA.[2] + tern(velA.[2], velB.[2])|]
    let gravityB = [|posB.[0] + tern(velB.[0], velA.[0]); posB.[1] + tern(velB.[1], velA.[1]); posB.[2] + tern(velB.[2], velA.[2])|]
    let resultA = (gravityA, velA)
    let resultB = (gravityB, velB)
    (resultA, resultB)

let applyVelocity(moon:(int[]*int[])) = 
    let (position, velocity) = moon
    ([|position.[0] + velocity.[0]; position.[1] + velocity.[1]; position.[2] + velocity.[2]|], velocity)

let permutations(moons:(int[]*int[])[]) = 
    seq{
        for moonAIdx in [|0..moons.Length - 1|] do
            for moonBIdx in [|moonAIdx..moons.Length - 1|] do
                yield (moonAIdx, moonBIdx) 
    } 
    |> Seq.toArray

let getTotalEnergy(moons: (int[]*int[])[]) =
    moons |> Array.map (fun (p, v) -> 
        (abs(p.[0]) + abs(p.[1]) + abs(p.[2])) * (abs(v.[0]) + abs(v.[1]) + abs(v.[2]))
    ) |> Array.sum

let execute =
    let filepath = __SOURCE_DIRECTORY__ + @"../../day12_input.txt"
    let moonArray = File.ReadAllLines filepath |> Seq.map (fun l -> (extractCoordinates l, [|0; 0; 0|])) |> Seq.toArray
    let permutations = permutations(moonArray)
    let steps = 1000
    //printfn "After %d Steps" 0
    //for idx in [0..moonArray.Length - 1] do
    //    Array.set moonArray idx (applyVelocity(moonArray.[idx]))
    //    let (position, velocity) = moonArray.[idx]
    //    printfn "pos=<x= %d, y= %d, z=%d>, vel=<x=%d, y=%d, z= %d>" position.[0] position.[1] position.[2] velocity.[0] velocity.[1] velocity.[2]
     
    
    for cc in [1..steps] do
        // SET Gravities
        for idx in [0..permutations.Length-1] do
            let (aIdx, bIdx) = permutations.[idx]
            let (moonA, moonB) = applyGravities(moonArray.[aIdx], moonArray.[bIdx])
            Array.set moonArray aIdx moonA
            Array.set moonArray bIdx moonB

        // UPDATE position
        //printfn "After %d Steps" cc
        for idx in [0..moonArray.Length - 1] do
            Array.set moonArray idx (applyVelocity(moonArray.[idx]))
            //let (position, velocity) = moonArray.[idx]
            //printfn "pos=<x= %d, y= %d, z=%d>, vel=<x=%d, y=%d, z= %d>" position.[0] position.[1] position.[2] velocity.[0] velocity.[1] velocity.[2]
    getTotalEnergy moonArray