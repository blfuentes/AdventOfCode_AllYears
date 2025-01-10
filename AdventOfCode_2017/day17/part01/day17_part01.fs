module day17_part01

open AdventOfCode_2017.Modules

let startInsertions (buffer: int array) (numsteps: int) (stopat: int) =
    let rec insertElement (index: int) (currentvalue: int) (currentBuffer: int array) =
        if currentvalue = stopat+1 then
            currentBuffer[index+1]
        else
            let newindex = (index + numsteps) % currentBuffer.Length
            match newindex = currentBuffer.Length-1 with
            | true ->
                let newbuffer = Array.concat [| currentBuffer; [| currentvalue |] |]
                insertElement (newindex+1) (currentvalue+1) newbuffer
            | false ->
                let leftpart = Array.sub currentBuffer 0 (newindex + 1)
                let rightpart = Array.sub currentBuffer (newindex + 1) (currentBuffer.Length - newindex - 1)
                let newbuffer = Array.concat [| leftpart; [| currentvalue |]; rightpart |]
                insertElement (newindex+1) (currentvalue+1) newbuffer
        
    insertElement 0 1 buffer

let execute() =
    let path = "day17/day17_input.txt"
    let steps = LocalHelper.GetContentFromFile path |> int

    let buffer = [| 0 |]
    startInsertions buffer steps 2017