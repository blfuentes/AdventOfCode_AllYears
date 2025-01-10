module day20_part01

open System.Text.RegularExpressions
open AdventOfCode_2017.Modules
open System

type Particle = {
    Id: int
    Position: int64 * int64 * int64
    Velocity: int64 * int64 * int64
    Acceleration: int64 * int64 * int64
}

let parseContent(lines: string array) =
    lines
    |> Array.mapi (fun i line ->
        let regexpattern = @"p=<\s*(-?\d+),\s*(-?\d+),\s*(-?\d+)>, v=<\s*(-?\d+),\s*(-?\d+),\s*(-?\d+)>, a=<\s*(-?\d+),\s*(-?\d+),\s*(-?\d+)>"

        let m' = Regex.Match(line, regexpattern)

        let position = ((int64)m'.Groups[1].Value, (int64)m'.Groups[2].Value, (int64)m'.Groups[3].Value)
        let velocity = ((int64)m'.Groups[4].Value, (int64)m'.Groups[5].Value, (int64)m'.Groups[6].Value)
        let acceleration = ((int64)m'.Groups[7].Value, (int64)m'.Groups[8].Value, (int64)m'.Groups[9].Value)

        { Id = i; Position = position; Velocity = velocity; Acceleration = acceleration }
    )

let updateParticle(particle: Particle) (second: int64) =
    let (px, py, pz) = particle.Position
    let (vx, vy, vz) = particle.Velocity
    let (ax, ay, az) = particle.Acceleration
    let newVelocity = vx + ax, vy + ay, vz + az
    let newPosition = px + vx + ax, py + vy + ay, pz + vz + az
    { particle with Position = newPosition; Velocity = newVelocity }

let distanceFromOrigin(particle: Particle) =
    let (px, py, pz) = particle.Position
    Math.Abs(px) + Math.Abs(py) + Math.Abs(pz)

let rec findClosestParticle(particles: Particle array) (second: int) =
    let updatedParticles = particles |> Array.map (fun p -> updateParticle p second)
    let closestParticle = updatedParticles |> Array.minBy distanceFromOrigin
    if second = 1000 then closestParticle.Id
    else findClosestParticle updatedParticles (second + 1)

let execute() =
    let path = "day20/day20_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let particles = parseContent content

    findClosestParticle particles 0