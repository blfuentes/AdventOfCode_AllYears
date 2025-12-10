module day10_part02

open AdventOfCode_2025.Modules
open System.Text.RegularExpressions

type Machine = {
    Id: int;
    LightDiagram: bool array;
    Buttons: (int array) seq
    Joltages: int array
}

let parseContent (lines: string array) =
    lines
    |> Array.mapi (fun idx line ->
        let id = idx
        let parts = line.Split(" ")
        let lightsRegex = Regex.Match(parts[0], @"\[([^\]]*)\]")
        let lightDiagram = 
            lightsRegex.Groups[1].Value.ToCharArray() 
            |> Array.map (fun c -> c = '#')
        let buttons=
            parts[1..parts.Length-2]
            |> Array.map (fun btnStr ->
                Regex.Match(btnStr, @"\(([^)]*)\)").Groups[1].Value.Split(",")
                |> Array.map int
            )
        let joltajes = 
            Regex.Match(parts[parts.Length-1], @"{([^>]*)}").Groups[1].Value.Split(",")
            |> Array.map int
        {
            Id = id;
            LightDiagram = lightDiagram;
            Buttons = buttons;
            Joltages = joltajes
        }
    )

let rec combinationWithRepetition (num: int) (list: 'a list) : 'a list list = 
    match num, list with
    | 0, _ -> [[]]
    | _, [] -> []
    | k, (x::xs) -> 
        List.map ((@) [x]) (combinationWithRepetition (k-1) list) 
        @ (combinationWithRepetition k xs)

let findCombination (machine: Machine) =
    let applyButton (state: int array) (buttonIndices: int array) =
        let newState = Array.copy state
        for idx in buttonIndices do
            if idx >= 0 && idx < newState.Length then
                newState[idx] <- newState[idx] + 1
        newState
    
    let buttonIndices = [0 .. (Seq.length machine.Buttons) - 1]
    let initialState = Array.zeroCreate<int> machine.Joltages.Length
    
    Seq.initInfinite (fun pressCount ->
        combinationWithRepetition pressCount buttonIndices
    )
    |> Seq.concat 
    |> Seq.map (fun buttonSequence ->
        let finalState = 
            buttonSequence 
            |> List.fold (fun state btnIdx -> 
                applyButton state (machine.Buttons |> Seq.item btnIdx)
            ) initialState
        (buttonSequence.Length, finalState)
    )
    |> Seq.find (fun (count, state) -> state = machine.Joltages)
    |> fst

let findCombinationDP (machine: Machine) =
    let target = machine.Joltages
    let buttons = machine.Buttons |> Seq.toArray
    let numPositions = target.Length
    
    // BFS to find minimum button presses
    let queue = System.Collections.Generic.Queue<int array * int>()
    let visited = System.Collections.Generic.HashSet<string>()
    
    let stateKey (state: int array) = 
        state |> Array.map string |> String.concat ","
    
    let initialState = Array.zeroCreate numPositions
    queue.Enqueue(initialState, 0)
    visited.Add(stateKey initialState) |> ignore
    
    let rec bfs () =
        if queue.Count = 0 then
            failwith "No solution found"
        else
            let (state, pressCount) = queue.Dequeue()
            
            if state = target then
                printfn "Found solution with %d presses" pressCount
                pressCount
            else
                // Try each button
                for button in buttons do
                    let newState = Array.copy state
                    for idx in button do
                        if idx >= 0 && idx < numPositions then
                            newState.[idx] <- newState.[idx] + 1
                    
                    let key = stateKey newState
                    
                    // Only continue if all values <= target (pruning)
                    let valid = 
                        newState 
                        |> Array.zip target
                        |> Array.forall (fun (t, s) -> s <= t)
                    
                    if valid && not (visited.Contains(key)) then
                        visited.Add(key) |> ignore
                        queue.Enqueue(newState, pressCount + 1)
                
                bfs()  // Continue BFS - this now properly returns the result
    
    bfs()

let rec gcd a b = if b = 0 then a else gcd b (a % b)

let lcm a b = (a * b) / (gcd a b)

// Extended Euclidean Algorithm for solving ax + by = gcd(a,b)
let rec extendedGcd a b =
    if b = 0 then (a, 1, 0)
    else
        let (g, x1, y1) = extendedGcd b (a % b)
        (g, y1, x1 - (a / b) * y1)

let findCombinationMath (machine: Machine) =
    let target = machine.Joltages
    let buttons = machine.Buttons |> Seq.toArray
    
    // Convert button effects to a matrix
    // Each column represents how button affects each position
    let buttonMatrix = 
        buttons 
        |> Array.map (fun button ->
            let effects = Array.zeroCreate target.Length
            for idx in button do
                if idx >= 0 && idx < target.Length then
                    effects.[idx] <- effects.[idx] + 1
            effects
        )
    
    // For each position, find GCD of all button effects on that position
    let positionGcds =
        [| for pos in 0 .. target.Length - 1 do
            let values = buttonMatrix |> Array.map (fun btn -> btn.[pos]) |> Array.filter ((<>) 0)
            if values.Length = 0 then
                yield 1
            else
                yield values |> Array.reduce gcd
        |]
    
    // Check if solution is possible
    let solvable = 
        target 
        |> Array.zip positionGcds
        |> Array.forall (fun (g, t) -> t % g = 0)
    
    if not solvable then
        failwith "No solution possible (GCD constraint violated)"
    
    // Reduce the problem by dividing by GCD
    let reducedTarget = 
        target 
        |> Array.zip positionGcds
        |> Array.map (fun (g, t) -> t / g)
    
    let reducedButtons =
        buttonMatrix
        |> Array.map (fun btn ->
            btn 
            |> Array.zip positionGcds
            |> Array.map (fun (g, b) -> b / g)
        )
    
    // Now solve with reduced values (smaller state space)
    let queue = System.Collections.Generic.Queue<int array * int>()
    let visited = System.Collections.Generic.HashSet<string>()
    
    let stateKey (state: int array) = 
        state |> Array.map string |> String.concat ","
    
    let initialState = Array.zeroCreate target.Length
    queue.Enqueue(initialState, 0)
    visited.Add(stateKey initialState) |> ignore
    
    let rec bfs () =
        if queue.Count = 0 then
            failwith "No solution found"
        else
            let (state, pressCount) = queue.Dequeue()
            
            if state = reducedTarget then
                printfn "Found solution with %d presses" pressCount
                pressCount
            else
                // Try each button
                for buttonIdx in 0 .. reducedButtons.Length - 1 do
                    let newState = Array.copy state
                    for idx in 0 .. newState.Length - 1 do
                        newState.[idx] <- newState.[idx] + reducedButtons.[buttonIdx].[idx]
                    
                    let key = stateKey newState
                    
                    // Pruning with reduced targets
                    let valid = 
                        newState 
                        |> Array.zip reducedTarget
                        |> Array.forall (fun (t, s) -> s <= t)
                    
                    if valid && not (visited.Contains(key)) then
                        visited.Add(key) |> ignore
                        queue.Enqueue(newState, pressCount + 1)
                
                bfs()
    
    bfs()

let getButtonPressesMath (machines: Machine array) =
    machines 
    |> Array.Parallel.map findCombinationMath  // Parallelize for extra speed
    |> Array.sum

let execute() =
    //let path = "day10/test_input_10.txt"
    let path = "day10/day10_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let machines = parseContent content
    getButtonPressesMath machines