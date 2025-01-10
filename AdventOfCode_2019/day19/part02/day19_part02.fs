module day19_part02

open Intcode

open AdventOfCode_2019.Modules

let getIsBeamCallback intcode =
    let comp = Computer.create intcode |> run
    let isBeam i j=
        match comp with
        | Input f ->
            match f (int64 i) with
            | Input f -> 
                match f (int64 j) with
                | Output (o, _) -> List.head o = 1L
                | _ -> failwith "Expected output"
            | _ -> failwith "Expected a second input"
        | _ -> failwith "Expected an input"
    isBeam

type BeamArea =
    { TopRight : int * int
      BottomLeft : int * int }

    static member topLeft  { TopRight = (_, y); BottomLeft = (x, _) } = (x, y)
    static member height { TopRight = (_, y1); BottomLeft = (_, y2) } = y2 - y1
    static member width { TopRight = (x1, _); BottomLeft = (x2, _) } = x1 - x2

    static member increaseWidth amount isBeam beamArea =
        let x, y = beamArea.TopRight
        let rec findNext y =
            if isBeam (x + amount) y then y
            else findNext (y + 1)
        { beamArea with TopRight = x + amount, findNext y }

    static member increaseHeight amount isBeam beamArea =
        let x, y = beamArea.BottomLeft
        let rec findNext x =
            if isBeam x (y + amount) then x
            else findNext (x + 1)
        { beamArea with BottomLeft = findNext x, y + amount }
    
let solvePart2 intcode = 
    let isBeam = getIsBeamCallback intcode

    let rec seek beamArea =
        let width = BeamArea.width beamArea
        let height = BeamArea.height beamArea
        if width = 99 && height = 99 then
            let (x, y) = BeamArea.topLeft beamArea
            x * 10000 + y
        else
            if width <= height then
                BeamArea.increaseWidth (99 - width) isBeam beamArea |> seek
            else
                BeamArea.increaseHeight (99 - height) isBeam beamArea |> seek
    
    seek { TopRight = (0, 0); BottomLeft = (0, 0) }

let execute =
    let path = "day19/day19_input.txt"
    let content = (LocalHelper.GetContentFromFile path).Split(",") |> Array.map int64
    solvePart2 content