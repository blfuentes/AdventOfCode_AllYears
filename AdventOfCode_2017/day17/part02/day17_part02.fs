module day17_part02

open AdventOfCode_2017.Modules

let startInsertions (buffer: int array) (numsteps: int) (stopat: int) =
    let rec insertElement (index: int) (currentvalue: int) (currentBuffer: int array) =
        if currentvalue = stopat+1 then
            currentBuffer[1]
        else
            let newindex = (index + numsteps) % currentvalue
            if newindex = 0 then
                currentBuffer[1] <- currentvalue
            insertElement (newindex+1) (currentvalue+1) currentBuffer
        
    insertElement 0 1 buffer

let execute() =
    let path = "day17/day17_input.txt"
    let steps = LocalHelper.GetContentFromFile path |> int

    let buffer = [| 0; 0 |]
    startInsertions buffer steps 50000000