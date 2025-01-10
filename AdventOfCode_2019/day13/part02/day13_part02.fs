module day13_part02

open System.Collections.Generic
open AoC_2019.Modules
open System

let rec executeRound (values: Dictionary<bigint, bigint>) (idx: bigint) (relativeBase: bigint) (paddlePos: bigint) (ballPos: bigint)
    (xPos: bigint) (totalBlocks: int) (latestScore: bigint)=
    if (xPos = 99I) then
        totalBlocks
    else
        let newJoystick =
            match paddlePos - ballPos with
            | x when x < 0I -> 1I
            | x when x > 0I -> -1I
            | _ -> 0I

        let result1 = IntCodeModule.getOutput values idx relativeBase [newJoystick] true 0I
        let result2 = IntCodeModule.getOutput values result1.Idx result1.RelativeBase [newJoystick] true 0I
        let result3 = IntCodeModule.getOutput values result2.Idx result2.RelativeBase [newJoystick] true 0I
        
        let newLatestScore = 
            if (result1.Output = -1I && result2.Output = 0I) then
                //printfn "Score is %A" result3.Output            
                result3.Output
            else
                latestScore
        
        if result3.Continue then
            match (int)result3.Output with
            | 2 ->
                executeRound values result3.Idx result3.RelativeBase paddlePos ballPos result1.Output  (totalBlocks + 1) newLatestScore
            | 3 ->    
                executeRound values result3.Idx result3.RelativeBase result1.Output ballPos result1.Output totalBlocks newLatestScore
            | 4->
                executeRound values result3.Idx result3.RelativeBase paddlePos result1.Output result1.Output totalBlocks newLatestScore
            | _ ->
                executeRound values result3.Idx result3.RelativeBase paddlePos ballPos result1.Output totalBlocks newLatestScore
        else
            newLatestScore |> int

let execute =
    let filepath = __SOURCE_DIRECTORY__ + @"../../day13_input.txt"
    let values = IntcodeComputerModule.getInputBigData filepath
    
    let xPos = 0I
    let totalBlocks = 0
    let paddlePos = 0I
    let ballPos = 0I
    values.[0I] <- 2I

    executeRound values 0I paddlePos ballPos xPos 0I totalBlocks 0I