module day18_part01

open System.Collections.Generic
open AdventOfCode_2015.Modules

type LightState =
    | ON
    | OFF

type Coord = {
    Row: int
    Col: int
    State: LightState
}

let parseContent(lines: string array) =
    let (maxrows, maxcols) = (lines.Length, lines[0].Length)
    let lightmap = Array2D.init maxrows maxcols (fun r c -> { Row = r; Col = c; State = OFF })
    for row in 0..maxrows-1 do
        for col in 0..maxcols-1 do
            if lines[row][col] = '#' then
                lightmap[row, col] <- { lightmap[row, col] with State = ON }
    lightmap

let switchLights(lightmap: Coord[,]) =
    let (maxrows, maxcols) = (lightmap.GetLength(0), lightmap.GetLength(1))
    let isInBoundaries (row, col) =
        row >= 0 && col >= 0 && row < maxrows && col < maxcols

    let neighbors(row, col) =
        [for r in -1..1 do
            for c in -1..1 do
                if r <> 0 || c <> 0 then
                    yield (row+r, col+c)
        ]
    let newstates =
        [for row in 0..maxrows-1 do
            for col in 0..maxcols-1 do
                let neigh = neighbors (row, col) |> List.filter isInBoundaries
                let lightstates = Dictionary<LightState, int>()
                lightstates.Add(ON, 0)
                lightstates.Add(OFF, 0)

                let states = 
                    neigh 
                    |> List.map(fun (r, c) -> lightmap[r, c])
                    |> List.groupBy(fun lm -> lm.State)
                match states |> List.tryFind(fun (s, list) -> s.IsON)  with
                | Some(s) -> lightstates[ON] <- lightstates[ON] + (snd s).Length
                | _ -> ()
                match states |> List.tryFind(fun (s, list) -> s.IsOFF)  with
                | Some(s) -> lightstates[OFF] <- lightstates[OFF] + (snd s).Length
                | _ -> ()

                ((row, col), lightstates)
        ]
    newstates
    |> List.iter(fun ((row, col), statecoords) ->
        let currentLight = lightmap[row, col]
        if currentLight.State.IsON then
            if statecoords[ON] <> 2 && statecoords[ON] <> 3 then
                lightmap[row, col] <- { lightmap[row, col] with State = OFF }
        else
            if statecoords[ON] = 3 then
                lightmap[row, col] <- { lightmap[row, col] with State = ON }   
    )

let printLightMap(lightmap: Coord[,]) =
    let (maxrows, maxcols) = (lightmap.GetLength(0), lightmap.GetLength(1))
    for row in 0..maxrows-1 do
        for col in 0..maxcols-1 do
            printf "%s" (if lightmap[row, col].State.IsON then "#" else ".")
        printfn ""

let startSwitching(counter: int) (lightmap: Coord[,]) =
    let mutable c = 1
    while c <= counter do
        switchLights lightmap
        c <- c + 1

let flat2Darray array2D = 
    seq { for x in [0..(Array2D.length1 array2D) - 1] do 
              for y in [0..(Array2D.length2 array2D) - 1] do 
                  yield array2D[x, y] }

let countLights(lightmap: Coord[,]) =
    flat2Darray lightmap |> Seq.sumBy(fun c -> if c.State.IsON then 1 else 0)
    
let execute =
    let input = "./day18/day18_input.txt"
    let content = LocalHelper.GetLinesFromFile input
    let numOfSwitches = 100

    let lightmap = parseContent content
    startSwitching numOfSwitches lightmap
    countLights lightmap