module day24_part02

open Microsoft.Z3
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

let initializeSolver (data: Hailstone list) =

    let ctx = new Context()
    let solver = ctx.MkSolver()
    
    let x = ctx.MkIntConst("x")
    let y = ctx.MkIntConst("y")
    let z = ctx.MkIntConst("z")
    let vx = ctx.MkIntConst("vx")
    let vy = ctx.MkIntConst("vy")
    let vz = ctx.MkIntConst("vz")

    // For each iteration, we will add 3 new equations to the solver.
    // We want to find 9 variables (x, y, z, vx, vy, vz, t0, t1, t2) that satisfy all the equations, so a system of 9 equations is enough.
    for idx in 0..2 do
        let t = ctx.MkIntConst($"t{idx}")
        let hail = data.Item(idx)

        let px = ctx.MkInt( hail.direction.dx |> int64)
        let py = ctx.MkInt(hail.direction.dy |> int64)
        let pz = ctx.MkInt(hail.direction.dz |> int64)

        let pvx = ctx.MkInt(hail.speed.sx |> int64)
        let pvy = ctx.MkInt(hail.speed.sy |> int64)
        let pvz = ctx.MkInt(hail.speed.sz |> int64)

        let xLeft = ctx.MkAdd(x + ctx.MkMul(t,vx)) // x + t * vx
        let yLeft = ctx.MkAdd(y + ctx.MkMul(t,vy)) // y + t * vy
        let zLeft = ctx.MkAdd(z + ctx.MkMul(t,vz)) // z + t * vz

        let xRight = ctx.MkAdd(px + ctx.MkMul(t,pvx)) // px + t * pvx
        let yRight = ctx.MkAdd(py + ctx.MkMul(t,pvy)) // py + t * pvy
        let zRight = ctx.MkAdd(pz + ctx.MkMul(t,pvz)) // pz + t * pvz

        solver.Add(ctx.MkGe(t, ctx.MkInt(0)))
        solver.Add(ctx.MkEq(xLeft, xRight)) // x + t * vx = px + t * pvx
        solver.Add(ctx.MkEq( yLeft, yRight)) // y + t * vy = py + t * pvy
        solver.Add(ctx.MkEq(zLeft, zRight)) // z + t * vz = pz + t * pvz

    solver.Check() |> ignore
    let model = solver.Model
    let rx = model.Eval(x)
    let ry = model.Eval(y)
    let rz = model.Eval(z)
    bigint.Parse(rx.ToString()) + bigint.Parse(ry.ToString()) + bigint.Parse(rz.ToString())

let execute =
    let path = "day24/day24_input.txt"
    let hailstones = LocalHelper.GetLinesFromFile path |> List.ofSeq |> parseInput
    let combhailtones = Utilities.comb 2 hailstones
    initializeSolver hailstones