open System.IO
open System.Collections.Generic
open System.Text.RegularExpressions

#load @"../../Modules/HelperModule.fs"
open AoC_2019.Modules

let filepath = __SOURCE_DIRECTORY__ + @"../../day12_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_01.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_02.txt"

let extractCoordinates input =
    match input with
    | Regex @"<\w=([-]?\d+),[-. ]?\w=([-]?\d+),[-. ]?\w=([-]?\d+)>" [ area; prefix; suffix ] ->
        [| area; prefix; suffix |] |> Array.map int
    | _ -> [||]

let applyGravities(moonA:(int[]*int[]*int[]), moonB:(int[]*int[]*int[])) =
    let (posA, velA, initPosA) = moonA
    let (posB, velB, initPosB) = moonB
    let velocityA = [|velA.[0] + tern(posA.[0], posB.[0]); velA.[1] + tern(posA.[1], posB.[1]); velA.[2] + tern(posA.[2], posB.[2])|]
    let velocityB = [|velB.[0] + tern(posB.[0], posA.[0]); velB.[1] + tern(posB.[1], posA.[1]); velB.[2] + tern(posB.[2], posA.[2])|]
    let resultA = (posA, velocityA, initPosA)
    let resultB = (posB, velocityB, initPosB)
    (resultA, resultB)

let applyVelocity(moon:(int[]*int[]*int[])) = 
    let (position, velocity, initialPosition) = moon
    ([|position.[0] + velocity.[0]; position.[1] + velocity.[1]; position.[2] + velocity.[2]|], velocity, initialPosition)

let permutations(moons:(int[]*int[]*int[])[]) = 
    seq{
        for moonAIdx in [|0..moons.Length - 1|] do
            for moonBIdx in [|moonAIdx..moons.Length - 1|] do
                yield (moonAIdx, moonBIdx) 
    } 
    |> Seq.toArray

let rec gcd(x: bigint, y:bigint) = if y = 0I then abs x else gcd(y,(x % y))

let lcm(x: bigint, y: bigint) = x * y / (gcd(x,y))

let findCycle(position: int, permutations:(int*int)[], moonArray: (int[]*int[]*int[])[]) =
    let mutable continueRunning = true
    let mutable cc = 1I

    for idx in [0..moonArray.Length-1] do
        let (pos, vel, init) = moonArray.[idx]
        Array.set pos 0 init.[0]
        Array.set vel 0 0
        Array.set pos 1 init.[1]
        Array.set vel 1 0
        Array.set pos 2 init.[2]
        Array.set vel 2 0

    while continueRunning do
        // SET Gravities
        for idx in [0..permutations.Length-1] do
            let (aIdx, bIdx) = permutations.[idx]
            let (moonA, moonB) = applyGravities(moonArray.[aIdx], moonArray.[bIdx])
            Array.set moonArray aIdx moonA
            Array.set moonArray bIdx moonB

        // UPDATE position
        for idx in [0..moonArray.Length - 1] do
            Array.set moonArray idx (applyVelocity(moonArray.[idx]))
        // FIND moon with initial state for that position
        let moon = moonArray |> Array.toList |> List.tryFind (fun (pos, vel, init) -> pos.[position] <> init.[position] || vel.[position] <> 0)
        match moon with
        | None -> continueRunning <- false
        | Some x -> 
            cc <- cc + 1I
            ()
    cc

let execute =
    let filepath = __SOURCE_DIRECTORY__ + @"../../day12_input.txt"
    let moonArray = File.ReadAllLines filepath |> Seq.map (fun l -> (extractCoordinates l, [|0; 0; 0|], extractCoordinates l)) |> Seq.toArray
    let permutations = permutations(moonArray)
    let cycleX = findCycle(0, permutations, moonArray)
    let cycleY = findCycle(1, permutations, moonArray)
    let cycleZ = findCycle(2, permutations, moonArray)
    lcm(cycleX,lcm(cycleY,cycleZ))
