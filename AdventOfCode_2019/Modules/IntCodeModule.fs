namespace AoC_2019.Modules

open System.Collections.Generic
open System.Numerics

[<AutoOpen>]
module IntCodeModule = 
    type IntCodeResult = 
        {
            Idx: bigint
            Continue: bool
            Output: bigint
            Input: bigint list
            RelativeBase: bigint
            MemoryMode: bool
            Pause: bool
        }

    let getInput(path: string) =
        let values = System.IO.File.ReadAllText(path).Split(',') |> Array.map (fun x -> BigInteger.Parse(x))
        let dataContainer = new Dictionary<bigint, bigint>()
        for idx in [|0..values.Length - 1|] do
            dataContainer.Add(bigint idx, values.[idx])
        dataContainer

    let getValue(values: Dictionary<bigint, bigint>, idx: bigint) =
        let found, value = values.TryGetValue idx
        match found with
        | true -> value
        | false -> 
            values.Add(idx, 0I)
            0I

    let getOperatorValue (values: Dictionary<bigint, bigint>) (relativeBase: bigint) (idx: bigint) (mode: int) =
        match mode with
        | 0 -> getValue(values, getValue(values, idx))
        | 1 -> getValue(values, idx)
        | 2 -> getValue(values, relativeBase + getValue(values, idx))
        | _ -> 0I

    let getOperatorAddress(values: Dictionary<bigint, bigint>) (relativeBase: bigint) (idx: bigint) (mode: int) =
        match mode with
        | 0 -> getValue(values, idx)
        | 1 -> idx
        | 2 -> relativeBase + getValue(values, idx)
        | _ -> 0I

    let setValue (values: Dictionary<bigint, bigint>) (idx: bigint) (newValue: bigint) =
        let found, value = values.TryGetValue idx
        match found with
        | true -> values.[idx] <- newValue
        | false -> values.Add(idx, newValue)

    let performOperation 
        (values: Dictionary<bigint, bigint>) (opDef: string array) (idx: bigint) 
        (relativeBase: bigint) (input:bigint list) (memoryMode: bool)
        (output: bigint) =

        let op = int(opDef.[4]) + int(opDef.[3]) * 10
        let param1Mode = int opDef.[2]
        let param2Mode = int opDef.[1]
        let param3Mode = int opDef.[0]
    
        match op with
        | 1 -> // ADD
            let operator1 = getOperatorValue values relativeBase (idx + 1I) param1Mode
            let operator2 = getOperatorValue values relativeBase (idx + 2I) param2Mode
            let writeAddress = getOperatorAddress values relativeBase (idx + 3I) param3Mode
            setValue values writeAddress (operator1 + operator2)           
            //printfn "opcode= %d op1= %A op2= %A op3= %A idx= %A" op operator1 operator2 writeAddress idx
            { Idx = idx + 4I; MemoryMode = memoryMode; Pause = false; Continue = true; Output = output; Input = input; RelativeBase = relativeBase }

        | 2 -> // MULTIPLY
            let operator1 = getOperatorValue values relativeBase (idx + 1I) param1Mode 
            let operator2 = getOperatorValue values relativeBase (idx + 2I) param2Mode
            let writeAddress = getOperatorAddress values relativeBase (idx + 3I) param3Mode
            setValue values writeAddress (operator1 * operator2)  
            //printfn "opcode= %d op1= %A op2= %A op3= %A idx= %A" op operator1 operator2 writeAddress idx
            { Idx = idx + 4I; MemoryMode = memoryMode; Pause = false; Continue = true; Output = output; Input = input; RelativeBase = relativeBase }

        | 3 -> // WRITE INPUT
            let writeAddress = getOperatorAddress values relativeBase (idx + 1I) param1Mode
            
            if input.Length > 0 then
                //printfn "VM index %A - Opcode %A used input %A" idx 3 input.Head
                setValue values writeAddress input.Head
                { Idx = idx + 2I; MemoryMode = memoryMode; Pause = false; Continue = true; Output = output; Input = input.Tail; RelativeBase = relativeBase }
            else      
                //printfn "VM No input index %A - Opcode %A used input" idx 3
                { Idx = idx; MemoryMode = memoryMode; Pause = true; Continue = true; Output = output; Input = []; RelativeBase = relativeBase }

        | 4 -> // OUTPUT
            let newOutput = getOperatorValue values relativeBase (idx + 1I) param1Mode
            //printfn "OUTPUT-->opcode= %A op1= %A idx= %A" op newOutput idx
            { Idx = idx + 2I; MemoryMode = memoryMode; Pause = memoryMode; Continue = true; Output = newOutput; Input = input; RelativeBase = relativeBase }

        | 5 -> // JUMP IF TRUE
            let operator1 = getOperatorValue values relativeBase (idx + 1I) param1Mode
            let operator2 = getOperatorValue values relativeBase (idx + 2I) param2Mode
            //printfn "opcode= %A op1= %A op2= %A idx= %A" op operator1 operator2 idx
            { Idx = (if (int(operator1) = 0) then (idx + 3I) else operator2); MemoryMode = memoryMode; Pause = false; Continue = true; Output = output; Input = input; RelativeBase = relativeBase }

        | 6 -> // JUMP IF FALSE
            let operator1 = getOperatorValue values relativeBase (idx + 1I) param1Mode
            let operator2 = getOperatorValue values relativeBase (idx + 2I) param2Mode
            //printfn "opcode= %A op1= %A op2= %A idx= %A" op operator1 operator2 idx
            { Idx = (if (int(operator1) = 0) then operator2 else (idx + 3I)); MemoryMode = memoryMode; Pause = false; Continue = true; Output = output; Input = input; RelativeBase = relativeBase }

        | 7 -> // LESS THAN
            let operator1 = getOperatorValue values relativeBase (idx + 1I) param1Mode
            let operator2 = getOperatorValue values relativeBase (idx + 2I) param2Mode
            let writeAddress = getOperatorAddress values relativeBase (idx + 3I) param3Mode
            if (operator1 < operator2) then setValue values writeAddress 1I
            else setValue values writeAddress 0I
            //printfn "opcode= %A op1= %A op2= %A op3= %A idx= %A" op operator1 operator2 writeAddress idx
            { Idx = idx + 4I; MemoryMode = memoryMode; Pause = false; Continue = true; Output = output; Input = input; RelativeBase = relativeBase }

        | 8 -> // EQUALS
            let operator1 = getOperatorValue values relativeBase (idx + 1I) param1Mode
            let operator2 = getOperatorValue values relativeBase (idx + 2I) param2Mode
            let writeAddress = getOperatorAddress values relativeBase (idx + 3I) param3Mode
            if (operator1 = operator2) then setValue values writeAddress 1I
            else setValue values writeAddress 0I
            //printfn "opcode= %A op1= %A op2= %A op3= %A idx= %A" op operator1 operator2 writeAddress idx
            { Idx = idx + 4I; MemoryMode = memoryMode; Pause = false; Continue = true; Output = output; Input = input; RelativeBase = relativeBase }
        
        | 9 -> // ADJUST RELATIVE BASE
            let operator1 = getOperatorValue values relativeBase (idx + 1I) param1Mode
            //printfn "RELATIVE-->opcode= %A relative base= %A idx= %A" op relativeBase idx
            { Idx = idx + 2I; MemoryMode = memoryMode; Pause = false; Continue = true; Output = output; Input = input; RelativeBase = relativeBase + operator1 }

        | 99 -> // EXIT
            //printfn "HALT opcode= %A idx= %A" op idx
            { Idx = idx; MemoryMode = memoryMode; Pause = false; Continue = false; Output = output; Input = input; RelativeBase = relativeBase }

        | _ -> 
            //printfn "UNDEFINED opcode= %A idx= %A" op idx
            failwith "Undefined opcode"

    let rec getOutput 
        (values: Dictionary<bigint, bigint>) (idx: bigint) (relativeBase: bigint) 
        (input: bigint list) (memoryMode:bool)
        (output:bigint) =
        let opDef = values.[idx].ToString().PadLeft(5, '0') |> Seq.toArray |> Array.map string 
        let resultOp = performOperation values opDef idx relativeBase input memoryMode output
        match (resultOp.Pause, resultOp.Continue) with
        | (false, true) ->             
            getOutput values resultOp.Idx resultOp.RelativeBase resultOp.Input memoryMode resultOp.Output
        | (_, _) -> 
            resultOp