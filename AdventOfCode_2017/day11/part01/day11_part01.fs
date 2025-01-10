module day11_part01

open AdventOfCode_2017.Modules

type direction = {
    X: int;
    Y: int;
    Z: int;
}

let parseContent (content: string) =
    let parts = 
        content.Split(",")
        |> Array.countBy(fun e -> e)
    parts
  
let getCoordinate (mov: string) : direction =
    match mov with
    | "nw" -> { X = -1; Y = 1; Z = 0 }
    | "n" -> { X = 0; Y = 1; Z = -1 }
    | "ne" -> { X = 1; Y = 0; Z = -1 }
    | "sw" -> { X = -1; Y = 0; Z = 1 }
    | "s" -> { X = 0; Y = -1; Z = 1 }
    | "se" -> { X = 1; Y = -1; Z = 0 }
    | _ -> failwith "error"
 
let calculatePosition (instructions: (string * int) array) =
    let position =
        instructions
        |> Array.map(fun (k, v) -> 
            let coord = getCoordinate k
            { X = coord.X * v; Y = coord.Y * v; Z = coord.Z * v }
        )
        |> Array.reduce(fun acc elem -> {
            X = acc.X + elem.X;
            Y = acc.Y + elem.Y;
            Z = acc.Z + elem.Z
        })
    position

let cubeDistance coord1 coord2 = 
    max (abs (coord1.X - coord2.X)) (max (abs (coord1.Y - coord2.Y)) (abs (coord1.Z - coord2.Z)))

let execute() =
    let path = "day11/day11_input.txt"
    //let path = "day11/test_input_01.txt"
    let content = LocalHelper.GetContentFromFile path
    let parts = parseContent content
    let position = calculatePosition parts
    cubeDistance { X = 0; Y = 0; Z  = 0 } position