module day22_part01

open FSharpx.Collections
open AdventOfCode_Utilities
open AdventOfCode_2018.Modules

open System
open System.Text.RegularExpressions

let toLines (text:string) = 
    text.Split([|'\r'; '\n'|], StringSplitOptions.RemoveEmptyEntries)
let groupValue (m:Match) (i:int) = m.Groups.[i].Value
let rxMatch pattern str = Regex.Match(str, pattern)
let toString (chrs : seq<char>) = String(Array.ofSeq chrs)

(* ================ Part A ================ *) 

type Location = int*int
type Type = Rocky | Narrow | Wet
type Times = {Torch: int; Gear: int; Neither: int}
type Region = {
    Location: Location; 
    Type: Type; 
    Erosion: int; 
    Times: Times}
type Cave = { Map: Region[,]; MaxX: int; MaxY:int}

let noRoute = Int32.MaxValue - 8
let defaultTimes = {Torch = noRoute; Gear = noRoute; Neither = noRoute}
let mouthTimes = {defaultTimes with Torch = 0; Gear = 7}
let nonRegion = {
    Location= (-1, -1); 
    Type= Rocky; 
    Erosion= -1; 
    Times= defaultTimes}

let parse text  = 
    let lines = text |> toLines
    let depth = rxMatch "\d+" lines.[0] |> (fun m -> m.Value |> int)
    let (x,y) = rxMatch "(\d+),(\d+)" lines.[1] |> (fun mtch ->
        let grp idx = groupValue mtch idx
        let grpi = grp >> int
        grpi 1, grpi 2)
    (depth, (x, y))

let display tLoc (cave:Cave) =
    let symbol {Type=typ} = 
        match typ with
        | Rocky -> '.'
        | Wet -> '='
        | Narrow -> '|'
    Array.init (cave.MaxY+1)  (fun  y -> 
        Array.init (cave.MaxX+1) (fun x -> 
            cave.Map.[x,y] |> function
                | {Location = (0,0)} -> 'M'
                | {Location = loc} when loc = tLoc-> 'T'
                | r -> symbol r))
    |> Array.iter (fun (line:char[]) -> printfn "%s" (toString line))
    cave

let getType erosion =
    match erosion % 3 with
    | 0 -> Rocky
    | 1 -> Wet
    | 2 -> Narrow

let newRegion depth tLoc (cave:Cave) loc =
    let erosion  =
        let (x,y) = loc
        let geoIndex =
            if loc = (0,0) || loc = tLoc then 0 else
            if y = 0 then x * 16807 else
            if x = 0 then y * 48271 else
            cave.Map.[x-1,y].Erosion * cave.Map.[x,y-1].Erosion
        (geoIndex + depth) % 20183
    {Location = loc; 
     Type = (getType erosion); 
     Erosion = erosion; 
     Times = defaultTimes}

let enumCoords (grid: 'a[,]) = 
    let (X,Y) = grid.GetUpperBound(0), grid.GetUpperBound(1)
    seq{for y in 0..Y do for x in 0..X do yield (x,y)}

let buildCave depth maxes tLoc : Cave =
    let (X,Y) = maxes
    let map = Array2D.create (X+1) (Y+1) nonRegion
    let cave = {Map = map;  MaxX = X; MaxY = Y}
    enumCoords map
    |> Seq.iter (fun (x,y) ->
        map.[x,y] <- (newRegion depth tLoc cave (x,y)))
    map.[0,0] <- {map.[0,0] with Times = mouthTimes}
    cave

let assessRisk cave =
    cave.Map
    |> Seq.cast<Region>
    |> Seq.sumBy (fun {Type=typ} -> 
        match typ with Rocky -> 0 | Wet -> 1 | Narrow -> 2)

let Part1 (input : string) =
    let depth, targetLoc = parse input    
    buildCave depth targetLoc targetLoc
    //|> display targetLoc
    |> assessRisk

let execute =
    let path = "day22/day22_input.txt"
    let content = LocalHelper.GetContentFromFile path
    Part1 content