namespace AoC_2019.Modules

open System.Collections.Generic
open System.Numerics

[<AutoOpen>]
module IntcodeComputerModule = 
    let mutable availableInputs = 2I
    let mutable loopOutputMode = false
    let getInput(path: string) =
        System.IO.File.ReadAllText(path).Split(',') |> Array.map int

    let getInputBigData(path: string) =
        let values = System.IO.File.ReadAllText(path).Split(',') |> Array.map (fun x -> BigInteger.Parse(x))
        let dataContainer = new Dictionary<bigint, bigint>()
        for idx in [|0..values.Length - 1|] do
            dataContainer.Add(bigint idx, values.[idx])
        dataContainer

    let getValueBigDataByIdx(values: Dictionary<bigint, bigint>, idx: bigint) =
        let found, value = values.TryGetValue idx
        match found with
        | true -> value
        | false -> 
            values.Add(idx, 0I)
            0I
    
    let setValueBigDataByIdx(values: Dictionary<bigint, bigint>, idx: bigint, newValue: bigint) =
        let found, value = values.TryGetValue idx
        match found with
        | true -> values.[idx] <- newValue
        | false -> values.Add(idx, newValue)

    let getOperatorValueBigData(values: Dictionary<bigint, bigint>, relativeBase: bigint, idx: bigint, mode: int) =
        match mode with
        | 0 -> getValueBigDataByIdx(values, getValueBigDataByIdx(values, idx))
        | 1 -> getValueBigDataByIdx(values, idx)
        | 2 -> getValueBigDataByIdx(values, relativeBase + getValueBigDataByIdx(values, idx))
        | _ -> 0I

    let getOperatorAddressBigData(values: Dictionary<bigint, bigint>, relativeBase: bigint, idx: bigint, mode: int) =
        match mode with
        | 0 -> getValueBigDataByIdx(values, idx)
        | 1 -> idx
        | 2 -> relativeBase + getValueBigDataByIdx(values, idx)
        | _ -> 0I

    let performOperationBigData(values: Dictionary<bigint, bigint>, relativeBase: bigint, phase:bigint, input:bigint, idx: bigint, opDef: string array, currentOutput: bigint) =
        let op = int(opDef.[4]) + int(opDef.[3]) * 10
        let param1Mode = int opDef.[2]
        let param2Mode = int opDef.[1]
        let param3Mode = int opDef.[0]

        match op with
        | 1 -> // ADD
            let operator1 = getOperatorValueBigData(values, relativeBase, idx + 1I, param1Mode)
            let operator2 = getOperatorValueBigData(values, relativeBase, idx + 2I, param2Mode)
            let writeAddress = getOperatorAddressBigData(values, relativeBase, idx + 3I, param3Mode)

            setValueBigDataByIdx(values, writeAddress, operator1 + operator2)
            //printfn "opcode= %d op1= %A op2= %A op3= %A idx= %A" op operator1 operator2 writeAddress idx
            ((false, true), [|idx + 4I; currentOutput; input|], relativeBase)
        | 2 -> // MULTIPLY
            let operator1 = getOperatorValueBigData(values, relativeBase, idx + 1I, param1Mode)
            let operator2 = getOperatorValueBigData(values, relativeBase, idx + 2I, param2Mode)
            let writeAddress = getOperatorAddressBigData(values, relativeBase, idx + 3I, param3Mode)

            setValueBigDataByIdx(values, writeAddress, operator1 * operator2)
            //printfn "opcode= %d op1= %A op2= %A op3= %A idx= %A" op operator1 operator2 writeAddress idx

            ((false, true), [|idx + 4I; currentOutput; input|], relativeBase)
        | 3 -> // WRITE INPUT
            let writeAddress = getOperatorAddressBigData(values, relativeBase, idx + 1I, param1Mode)
            if availableInputs > 0I then
                //printfn "VM index %A - Opcode %A used input %A" idx 3 phase
                availableInputs <- availableInputs - 1I
                setValueBigDataByIdx(values, writeAddress, phase)
                ((false, true), [|idx + 2I; currentOutput; input|], relativeBase)
            else
                ((true, true), [|idx; currentOutput; input|], relativeBase)
        | 4 -> // OUTPUT
            let output = getOperatorValueBigData(values, relativeBase, idx + 1I, param1Mode)
            //printfn "OUTPUT-->opcode= %A op1= %A idx= %A" op output idx
            if not loopOutputMode then
                ((false, true), [|idx + 2I; output; input|], relativeBase)
            else
                ((true, true), [|idx + 2I; output; input|], relativeBase)
        | 5 -> // JUMP IF TRUE
            let operator1 = getOperatorValueBigData(values, relativeBase, idx + 1I, param1Mode)
            let operator2 = getOperatorValueBigData(values, relativeBase, idx + 2I, param2Mode)
            //printfn "opcode= %A op1= %A op2= %A idx= %A" op operator1 operator2 idx
            match int(operator1) with
            | 0 -> ((false, true), [|idx + 3I; currentOutput; input|], relativeBase)
            | _ -> ((false, true), [|operator2; currentOutput; input|], relativeBase)
        | 6 -> // JUMP IF FALSE
            let operator1 = getOperatorValueBigData(values, relativeBase, idx + 1I, param1Mode)
            let operator2 = getOperatorValueBigData(values, relativeBase, idx + 2I, param2Mode)
            //printfn "opcode= %A op1= %A op2= %A idx= %A" op operator1 operator2 idx
            match int(operator1) with
            | 0 -> ((false, true), [|operator2; currentOutput; input|], relativeBase)
            | _ -> ( (false, true), [|idx + 3I; currentOutput; input|], relativeBase) 
        | 7 -> // LESS THAN
            let operator1 = getOperatorValueBigData(values, relativeBase, idx + 1I, param1Mode)
            let operator2 = getOperatorValueBigData(values, relativeBase, idx + 2I, param2Mode)
            let writeAddress = getOperatorAddressBigData(values, relativeBase, idx + 3I, param3Mode)
            //printfn "opcode= %A op1= %A op2= %A op3= %A idx= %A" op operator1 operator2 writeAddress idx
            if (operator1 < operator2) then setValueBigDataByIdx(values, writeAddress, 1I)
            else setValueBigDataByIdx(values, writeAddress, 0I)
            ((false, true), [|idx + 4I; currentOutput; input|], relativeBase) 
        | 8 -> // EQUALS
            let operator1 = getOperatorValueBigData(values, relativeBase, idx + 1I, param1Mode)
            let operator2 = getOperatorValueBigData(values, relativeBase, idx + 2I, param2Mode)
            let writeAddress = getOperatorAddressBigData(values, relativeBase, idx + 3I, param3Mode)
            //printfn "opcode= %A op1= %A op2= %A op3= %A idx= %A" op operator1 operator2 writeAddress idx
            if (operator1 = operator2) then setValueBigDataByIdx(values, writeAddress, 1I)
            else setValueBigDataByIdx(values, writeAddress, 0I)
            ((false, true), [|idx + 4I; currentOutput; input|], relativeBase) 
        | 9 -> // ADJUST RELATIVE BASE
            let operator1 = getOperatorValueBigData(values, relativeBase, idx + 1I, param1Mode)
            //printfn "RELATIVE-->opcode= %A relative base= %A idx= %A" op relativeBase idx
            ((false, true), [|idx + 2I; currentOutput; input|], relativeBase + operator1) 
        | 99 -> 
            //printfn "HALT opcode= %A idx= %A" op idx
            ((false, false), [|idx; currentOutput; input|], relativeBase) // (lastoutput, false)
        | _ -> 
            //printfn "UNDEFINED opcode= %A idx= %A" op idx
            ((false, true), [|idx; currentOutput; input|], relativeBase) // (0, true)

    let rec getOutputBigData(values: Dictionary<bigint, bigint>, relativeBase: bigint, phase:bigint, input:bigint, idx: bigint, currentOutput:bigint) =
        let opDefinition = values.[idx].ToString().PadLeft(5, '0') |> Seq.toArray |> Array.map string 
        availableInputs <- 2I
        let resultOp = performOperationBigData(values, relativeBase, phase, input, idx, opDefinition, currentOutput)
        match resultOp with
        | ((_, true), result, relBase) -> getOutputBigData(values, relBase, result.[2], input, result.[0], result.[1])
        | ((_, false), result, relBase) -> result.[1]

    let rec getOutputBigDataWithMemory(values: Dictionary<bigint, bigint>, relativeBase: bigint, phase:bigint, input:bigint, idx: bigint, currentOutput:bigint) =
        let opDefinition = values.[idx].ToString().PadLeft(5, '0') |> Seq.toArray |> Array.map string 

        let resultOp = performOperationBigData(values, relativeBase, phase, input, idx, opDefinition, currentOutput)
        match resultOp with
        | ((false, false), result, relBase) -> (result.[1], (result.[0], false), relBase) // no pausa + no continuar
        | ((false, true), result, relBase) -> getOutputBigDataWithMemory(values, relBase, result.[2], input, result.[0], result.[1]) // no pausa + continue
        | ((true, false), result, relBase) -> (result.[1], (result.[0], true), relBase) // pausa + no continuar
        | ((true, true), result, relBase) -> (result.[1], (result.[0], true), relBase) // pausa + continuar

    let rec getOutputPhaseLoopMode(values: Dictionary<bigint, bigint>, relativeBase, phase:bigint, input:bigint, idx: bigint, currentOutput:bigint) =
        let opDefinition = values.[idx].ToString().PadLeft(5, '0') |> Seq.toArray |> Array.map string 
        let resultOp = performOperationBigData(values, relativeBase, phase, input, idx, opDefinition, currentOutput)

        match resultOp with
        | ((false, false), result, relBase) -> (result.[1], (result.[0], false), relBase) // no pausa + no continuar
        | ((false, true), result, relBase) -> getOutputPhaseLoopMode(values, relBase, result.[2], input, result.[0], result.[1]) // no pausa + continue
        | ((true, false), result, relBase) -> (result.[1], (result.[0], true), relBase) // pausa + no continuar
        | ((true, true), result, relBase) -> (result.[1], (result.[0], true), relBase) // pausa + continuar

    let execute(path:string, input:bigint) =
        let values = getInputBigData path
        let idx = 0I

        getOutputBigData(values, 0I, input, input, idx, 0I)

    let executeWithPhase(path: string, phase:bigint, input:bigint) =
        let values = getInputBigData path
        let idx = 0I
        getOutputBigData(values, 0I, phase, input, idx, 0I)  

    let executeBigData(path:string, input:bigint) =
        let values = getInputBigData path
        let idx = 0I
        getOutputBigData(values, 0I, input, input, idx, 0I)

    let executeBigDataWithMemory(values: Dictionary<bigint, bigint>, relativeBase: bigint, idx:bigint, input:bigint, numberOfInputs: bigint) =
        availableInputs <- numberOfInputs
        loopOutputMode <- true
        getOutputBigDataWithMemory(values, relativeBase, input, input, idx, 0I)

    let executeWithPhaseLoopMode(values: Dictionary<bigint, bigint>, phase: bigint, idx:bigint, input:bigint, numberOfInputs: bigint) =    
        availableInputs <- numberOfInputs
        getOutputPhaseLoopMode(values, 0I, phase, input,idx, 0I)  
