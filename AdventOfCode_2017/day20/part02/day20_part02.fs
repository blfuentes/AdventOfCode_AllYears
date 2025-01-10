module day20_part02

open System.Text.RegularExpressions
open AdventOfCode_2017.Modules

type Particle = {
    Id: int
    Position: float * float * float
    Velocity: float * float * float
    Acceleration: float * float * float
}

let inline (=|=) (x1,y1,z1) (x2,y2,z2) = x1 = x2 && y1 = y2 && z1 = z2

let updateParticle(particle: Particle) =
    let (px, py, pz) = particle.Position
    let (vx, vy, vz) = particle.Velocity
    let (ax, ay, az) = particle.Acceleration
    let newVelocity = vx + ax, vy + ay, vz + az
    let newPosition = px + vx + ax, py + vy + ay, pz + vz + az
    { particle with Position = newPosition; Velocity = newVelocity }

let parseContent(lines: string array) =
    lines
    |> Array.mapi (fun i line ->
        let regexpattern = @"p=<\s*(-?\d+),\s*(-?\d+),\s*(-?\d+)>, v=<\s*(-?\d+),\s*(-?\d+),\s*(-?\d+)>, a=<\s*(-?\d+),\s*(-?\d+),\s*(-?\d+)>"

        let m' = Regex.Match(line, regexpattern)

        let position = ((float)m'.Groups[1].Value, (float)m'.Groups[2].Value, (float)m'.Groups[3].Value)
        let velocity = ((float)m'.Groups[4].Value, (float)m'.Groups[5].Value, (float)m'.Groups[6].Value)
        let acceleration = ((float)m'.Groups[7].Value, (float)m'.Groups[8].Value, (float)m'.Groups[9].Value)

        { Id = i; Position = position; Velocity = velocity; Acceleration = acceleration }
    )


let rec reduceWithCollisions(particles: Particle list) =
    {0..500}
    |> Seq.fold (fun particles _ ->
        particles
        |> List.filter (fun p ->
            particles
            |> List.exists (fun p2 -> p.Id <> p2.Id && p.Position =|= p2.Position)
            |> not
        )
        |> List.map updateParticle
    ) particles
    |> List.length

let execute() =
    let path = "day20/day20_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let particles = parseContent content |> List.ofArray

    reduceWithCollisions particles