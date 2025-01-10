module day11_part02
open AdventOfCode_2017.Modules

type direction = {
    X: int;
    Y: int;
    Z: int;
}

let cubeDistance coord1 coord2 = 
    max (abs (coord1.X - coord2.X)) (max (abs (coord1.Y - coord2.Y)) (abs (coord1.Z - coord2.Z)))

let parseContent (content: string) =
    let parts = 
        content.Split(",")
    parts |> List.ofArray
  
let getCoordinate (mov: string) : direction =
    match mov with
    | "nw" -> { X = -1; Y = 1; Z = 0 }
    | "n" -> { X = 0; Y = 1; Z = -1 }
    | "ne" -> { X = 1; Y = 0; Z = -1 }
    | "sw" -> { X = -1; Y = 0; Z = 1 }
    | "s" -> { X = 0; Y = -1; Z = 1 }
    | "se" -> { X = 1; Y = -1; Z = 0 }
    | _ -> failwith "error"
 
let rec furthestPosition (movements: string list) (current: direction) (maxdistance: int) =
    match movements with
    | [] -> maxdistance
    | mov :: rest -> 
        let n' = getCoordinate mov
        let newcurrent = { X = current.X + n'.X; Y = current.Y + n'.Y; Z = current.Z + n'.Z }
        let newdistance = cubeDistance { X = 0; Y = 0; Z = 0 } newcurrent
        furthestPosition rest newcurrent (if newdistance > maxdistance then newdistance else maxdistance)

let execute() =
    let path = "day11/day11_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let parts = parseContent content
    furthestPosition parts { X = 0; Y = 0; Z  = 0 } 0