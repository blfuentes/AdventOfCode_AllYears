module day24_part01

open AdventOfCode_2023.Modules
open AdventOfCode_Utilities

type Speed = {
    sx: double
    sy: double
    sz: double
}

type Direction = {
    dx: double
    dy: double
    dz: double
}

type Hailstone = {
    direction: Direction
    speed: Speed
}

type PairOfHails = {
    h1: Hailstone
    h2: Hailstone
    lambda: float
    x: double
    y: double
}

let getcollisionXY (h1: Hailstone) (h2: Hailstone) =
    let (x1, y1, z1) = (h1.direction.dx, h1.direction.dy, h1.direction.dz)
    let (x2, y2, z2) = (h2.direction.dx, h2.direction.dy, h2.direction.dz)
    let (s1x, s1y, s1z) = (h1.speed.sx, h1.speed.sy, h1.speed.sz)
    let (s2x, s2y, s2z) = (h2.speed.sx, h2.speed.sy, h2.speed.sz)
    let a = x1
    let b = y1
    let c = x1 + s1x
    let d = y1 + s1y
    let p = x2
    let q = y2
    let r = x2 + s2x
    let s = y2 + s2y
    let det = (c - a) * (s - q) - (r - p) * (d - b)
    if det = 0.0 then
        None
    else
        let lambda = ((s - q) * (r - a) + (p - r) * (s - b)) / det
        let x = x1 + lambda * s1x
        let y = y1 + lambda * s1y
        let result = { h1 = h1; h2 = h2; lambda = lambda; x = x; y = y}
        Some(result)

let parseInput (input: string list) =
    input
    |> List.map(fun line ->
        let parts = line.Split("@")
        let direction =
            {
                dx = double (parts.[0].Split(",").[0].Trim())
                dy = double (parts.[0].Split(",").[1].Trim())
                dz = double (parts.[0].Split(",").[2].Trim())
            }
        let speed =
            {
                sx = double (parts.[1].Split(",").[0].Trim())
                sy = double (parts.[1].Split(",").[1].Trim())
                sz = double (parts.[1].Split(",").[2].Trim())
            }
        {
            direction = direction
            speed = speed
        }
    )

let rec getIntersections (acc: int) (hailstones: PairOfHails list) (min: double) (max: double)=
    match hailstones with
    | [] -> acc
    | head :: tail ->
        if (
            (head.x < head.h1.direction.dx && head.h1.speed.sx > 0.0) || (head.x > head.h1.direction.dx && head.h1.speed.sx < 0.0) 
            ||
            (head.x < head.h2.direction.dx && head.h2.speed.sx > 0.0) || (head.x > head.h2.direction.dx && head.h2.speed.sx < 0.0)
            ) then
                getIntersections acc tail min max
        else
            if head.x < min || head.x > max || head.y < min || head.y > max then
                getIntersections acc tail min max
            else
                getIntersections (acc + 1) tail min max
let execute =
    let path = "day24/day24_input.txt"
    let hailstones = LocalHelper.GetLinesFromFile path |> List.ofSeq |> parseInput
    let combhailtones = Utilities.comb 2 hailstones
    let collisions = combhailtones |> List.map(fun c -> getcollisionXY c.Head c.Tail.Head) |> List.choose (fun c -> c)
    let min = 200000000000000.
    let max = 400000000000000.
    let numOfIntersections = getIntersections 0 collisions min max
    numOfIntersections