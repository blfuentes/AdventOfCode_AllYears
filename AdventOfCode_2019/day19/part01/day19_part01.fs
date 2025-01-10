module day19_part01

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

let affectedTiles intcode =
    let isBeam = getIsBeamCallback intcode
    Seq.init (50*50) (fun i -> isBeam (i / 50) (i % 50))
    |> Seq.filter id
    |> Seq.length

let execute =
    let path = "day19/day19_input.txt"
    let content = (LocalHelper.GetContentFromFile path).Split(",") |> Array.map int64
    affectedTiles content