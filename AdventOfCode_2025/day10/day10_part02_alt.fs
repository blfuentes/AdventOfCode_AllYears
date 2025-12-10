module day10_part02_alt

open AdventOfCode_2025.Modules
open System.Text.RegularExpressions

type Machine = {
    Id: int
    LightDiagram: bool array
    Buttons: int array array
    Joltages: int array
}

let private parseMachine idx (line: string) =
    let parts = line.Split(" ")
    let extractMatch pattern input = 
        Regex.Match(input, pattern).Groups[1].Value
    
    let lightDiagram = 
        extractMatch @"\[([^\]]*)\]" parts[0]
        |> Seq.map ((=) '#')
        |> Seq.toArray
    
    let buttons =
        parts[1..parts.Length-2]
        |> Array.map (fun btnStr ->
            extractMatch @"\(([^)]*)\)" btnStr
            |> fun s -> s.Split(",")
            |> Array.map int)
    
    let joltages = 
        extractMatch @"{([^>]*)}" parts[parts.Length-1]
        |> fun s -> s.Split(",")
        |> Array.map int
    
    { Id = idx; LightDiagram = lightDiagram; Buttons = buttons; Joltages = joltages }

let parseContent (lines: string array) =
    lines |> Array.mapi parseMachine

let private tryFindWithIndex predicate array =
    array 
    |> Array.tryFindIndex predicate 
    |> Option.map (fun idx -> idx, array[idx])

// gaussian elimination with integer arithmetic
let private gaussianElimination (matrix: int[][]) =
    if matrix.Length = 0 then None
    else
        let n = matrix[0].Length - 1
        let mat = matrix |> Array.map Array.copy
        
        let rec eliminateColumns col currentRow pivotCols =
            if col >= n || currentRow >= mat.Length then
                pivotCols |> List.rev |> List.toArray
            else
                match mat[currentRow..] |> tryFindWithIndex (fun row -> row[col] <> 0) with
                | None -> eliminateColumns (col + 1) currentRow pivotCols
                | Some (offset, _) ->
                    let pivotRow = currentRow + offset
                    
                    // Swap rows
                    let temp = mat[currentRow]
                    mat[currentRow] <- mat[pivotRow]
                    mat[pivotRow] <- temp
                    
                    // Eliminate below
                    for row in currentRow + 1 .. mat.Length - 1 do
                        if mat[row][col] <> 0 then
                            let factor = mat[row][col]
                            let pivotVal = mat[currentRow][col]
                            for j in col .. n do
                                mat[row][j] <- mat[row][j] * pivotVal - mat[currentRow][j] * factor
                    
                    eliminateColumns (col + 1) (currentRow + 1) (col :: pivotCols)
        
        Some(eliminateColumns 0 0 [], mat)

// Build constraint matrix from buttons and joltages
let private buildConstraintMatrix (buttons: int[][]) (joltages: int[]) =
    joltages
    |> Array.mapi (fun i target ->
        let row = Array.zeroCreate (buttons.Length + 1)
        buttons 
        |> Array.iteri (fun j btnIndices ->
            if btnIndices |> Array.contains i then row[j] <- 1)
        row[buttons.Length] <- target
        row)

// Back-substitute to find solution for given free variable values
let private backSubstitute (pivotCols: int[]) (reducedMatrix: int[][]) (freeVars: int[]) (freeValues: int[]) n =
    let solution = Array.zeroCreate n
    
    // Set free variables
    Array.zip freeVars freeValues
    |> Array.iter (fun (var, value) -> solution[var] <- value)
    
    // Back-substitution for pivot variables
    let rec substitute i =
        if i < 0 then Some solution
        else
            let row = i
            let col = pivotCols[i]
            let mutable total = reducedMatrix[row][n]
            
            for j in col + 1 .. n - 1 do
                total <- total - reducedMatrix[row][j] * solution[j]
            
            let pivotVal = reducedMatrix[row][col]
            if pivotVal = 0 || total % pivotVal <> 0 then None
            else
                let value = total / pivotVal
                if value < 0 then None
                else
                    solution[col] <- value
                    substitute (i - 1)
    
    substitute (pivotCols.Length - 1)

// Verify solution satisfies original constraints
let private verifySolution (buttons: int[][]) (joltages: int[]) (solution: int[]) =
    joltages
    |> Array.mapi (fun i target ->
        buttons
        |> Array.mapi (fun j btnIndices -> 
            if solution[j] > 0 && btnIndices |> Array.contains i then solution[j] else 0)
        |> Array.sum
        |> (=) target)
    |> Array.forall id

// Generate search ranges based on number of free variables
let private getSearchRanges freeVarCount maxVal =
    match freeVarCount with
    | 0 -> seq { yield [||] }
    | 1 -> seq { for v1 in 0..maxVal -> [| v1 |] }
    | 2 -> 
        let limit = max 200 maxVal
        seq { for v1 in 0..limit do
              for v2 in 0..limit -> [| v1; v2 |] }
    | 3 -> 
        seq { for v1 in 0..249 do
              for v2 in 0..249 do
              for v3 in 0..249 -> [| v1; v2; v3 |] }
    | 4 -> 
        seq { for v1 in 0..29 do
              for v2 in 0..29 do
              for v3 in 0..29 do
              for v4 in 0..29 -> [| v1; v2; v3; v4 |] }
    | _ -> seq { yield Array.zeroCreate freeVarCount }

// Solve the linear system for button presses
let private solveSystem (machine: Machine) =
    let buttons = machine.Buttons
    let joltages = machine.Joltages
    let n = buttons.Length
    
    let matrix = buildConstraintMatrix buttons joltages
    
    match gaussianElimination matrix with
    | None -> Array.zeroCreate n
    | Some(pivotCols, reducedMatrix) ->
        let pivotSet = Set.ofArray pivotCols
        let freeVars = [| 0..n-1 |] |> Array.filter (pivotSet.Contains >> not)
        
        let maxVal = if joltages.Length > 0 then (joltages |> Array.max) * 3 else 100
        
        getSearchRanges freeVars.Length maxVal
        |> Seq.choose (fun freeValues ->
            backSubstitute pivotCols reducedMatrix freeVars freeValues n
            |> Option.filter (verifySolution buttons joltages)
            |> Option.map (fun sol -> sol, Array.sum sol))
        |> Seq.tryHead
        |> Option.map fst
        |> Option.defaultValue (Array.zeroCreate n)

let minimumPresses (machine: Machine) =
    machine |> solveSystem |> Array.sum

let execute () =
    let path = "day10/day10_input.txt"
    LocalHelper.GetLinesFromFile path
    |> parseContent
    |> Array.Parallel.map minimumPresses
    |> Array.sum