module day22_part01

open System.Collections.Generic

open AdventOfCode_2023.Modules

type BrickSize = { start: int; finish: int }
type Brick = 
    {
        xSize: BrickSize
        ySize: BrickSize
        zSize: BrickSize   
    }
    member this.Top = this.zSize.finish
    member this.Bottom = this.zSize.start

type Holders = {
    blocksToHold : Dictionary<Brick, HashSet<Brick>>
    blocksThatHoldMe : Dictionary<Brick, HashSet<Brick>>
}

let collides (a: BrickSize) (b: BrickSize) =
    a.start <= b.finish && b.start <= a.finish

let collidesInXandY (a: Brick) (b: Brick) =
    collides a.xSize b.xSize && collides a.ySize b.ySize

let startFall (bricks: Brick[]) =
    let sortedbricks = bricks |> Array.sortBy (fun b -> b.Bottom)

    for idx in 0..sortedbricks.Length - 1 do
        let mutable minBottom = 1
        for jdx in 0..idx - 1 do
            if collidesInXandY sortedbricks.[idx] sortedbricks.[jdx] then
                minBottom <- System.Math.Max(minBottom, sortedbricks.[jdx].Top + 1)

        let fall = sortedbricks.[idx].Bottom - minBottom
        sortedbricks.[idx] <- { 
            sortedbricks.[idx] 
            with zSize = { 
                start = sortedbricks.[idx].Bottom - fall; 
                finish = sortedbricks.[idx].Top - fall 
            } 
        }
    sortedbricks

let holders (bricks: Brick[]) =
    let blocksToHold = Dictionary<Brick, HashSet<Brick>>()
    let blocksThatHoldMe = Dictionary<Brick, HashSet<Brick>>()
    for b in bricks do
        blocksToHold.Add(b, HashSet<Brick>())
        blocksThatHoldMe.Add(b, HashSet<Brick>())

    for idx in 0..bricks.Length - 1 do
        for jdx in 0..bricks.Length - 1 do
            let zs = bricks.[jdx].Bottom = (1 + bricks.[idx].Top)
            if zs && collidesInXandY bricks.[idx] bricks.[jdx] then
                blocksThatHoldMe.[bricks.[jdx]].Add(bricks.[idx]) |> ignore
                blocksToHold.[bricks.[idx]].Add(bricks.[jdx]) |> ignore
    { blocksToHold = blocksToHold; blocksThatHoldMe = blocksThatHoldMe }

let explode(bricks: Brick[]) =
    let fallenBricks = startFall bricks
    let holders = holders fallenBricks

    let blocksFalling (holders: Holders) (bricks: Brick) (falling: Set<Brick>) =
        let blocksToHold = holders.blocksToHold.[bricks]
        let subsets = 
            seq {
                for b in blocksToHold do
                    let belows = holders.blocksThatHoldMe.[b]
                    if Set.isSubset (belows |> Set.ofSeq) falling then
                        yield b
            }
        subsets |> List.ofSeq

    let explossions = 
        seq {
            for brick in fallenBricks do
                let queue = new Queue<Brick>()
                queue.Enqueue(brick)
                let falling = HashSet<Brick>()
                while queue.Count > 0 do
                    let b = queue.Dequeue()
                    falling.Add(b) |> ignore
                    let blocksToFall = blocksFalling holders b (falling |> Set.ofSeq)
                    blocksToFall |> List.iter (fun b -> queue.Enqueue(b))
                yield falling.Count - 1
        }
    explossions
                
let parseInput (input: string list) =
    let bricks =
            input
                |> List.map (fun line ->
                    let parts = line.Split([|'~'|])
                    let front = parts.[0].Split([|','|]) |> Array.map int
                    let back = parts.[1].Split([|','|]) |> Array.map int
                    {                         
                        xSize = { start = front.[0]; finish = back.[0] }
                        ySize = { start = front.[1]; finish = back.[1] }
                        zSize = { start = front.[2]; finish = back.[2] }
                    }
                )
    bricks

let execute =
    let path = "day22/day22_input.txt"
    let bricks = LocalHelper.GetLinesFromFile path |> List.ofSeq |> parseInput |> Array.ofList
    let explossions = (explode bricks) |> Seq.filter ((=) 0) |> Seq.length
    explossions