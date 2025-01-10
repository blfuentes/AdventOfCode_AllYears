module day23_part01

open AdventOfCode_2018.Modules
open AdventOfCode_Utilities

open FSharpx.Collections
open System
open System.IO

type Vec3 =
    {x: int; y: int; z: int}
    static member Zero = {x=0; y=0; z=0}
let corners {x=x; y=y; z=z} radius = 
    [
        {x=x - radius; y=y; z=z}
        {x=x + radius; y=y; z=z}
        {x=x; y=y - radius; z=z}
        {x=x; y=y + radius; z=z}
        {x=x; y=y; z=z - radius}
        {x=x; y=y; z=z + radius}
    ]

let manhattan {x=x0; y=y0; z=z0} {x=x1; y=y1; z=z1} =
    abs (x0 - x1) + abs (y0 - y1) + abs (z0 - z1)

type Octohedron = { pos: Vec3; radius: int }
let doOctohedronsOverlap o1 o2 = manhattan o1.pos o2.pos <= o1.radius + o2.radius
let doesOctohedronContainPoint octo pt = manhattan octo.pos pt <= octo.radius
let doesOctohedronContainAnother o1 o2 = 
    corners o1.pos o1.radius |> List.exists (doesOctohedronContainPoint o2 >> not) |> not
let minManhattanDistanceToOrigin octo = manhattan octo.pos Vec3.Zero - octo.radius

let divideOctohedron {pos=pos; radius=radius} =
    let offset =
        if radius >= 3 then
            ((float radius) / 3.0) |> floor |> int
        elif radius > 0 then
            1
        else
            0
    let newRadius = radius - offset
    let axisOffsets = corners pos offset
    let offsets = if radius = 1 then pos :: axisOffsets else axisOffsets
    offsets |> List.map (fun p -> {pos=p; radius=newRadius})

type Nanobot = {id: int; pos: Vec3; radius: int}
let asOctohedron bot = {pos=bot.pos; radius=bot.radius}

let asNanobot i line =
    let pos, radius = splitByFn ">, " (fun p -> p.[0].[5..], int p.[1].[2..]) line
    let px, py, pz = splitByFn "," (fun p -> int p.[0], int p.[1], int p.[2]) pos
    { id=i; pos={ x=px; y=py; z=pz }; radius=radius }

let numberOfBots bots =
    let maxRadiusBot = Array.maxBy (fun b -> b.radius) bots
    bots 
    |> Array.filter (fun b -> manhattan b.pos maxRadiusBot.pos <= maxRadiusBot.radius)
    |> Array.length

let execute =
    let path = "day23/day23_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let bots = content |> Array.mapi asNanobot
    numberOfBots bots