module day14_part02

open AdventOfCode_2024.Modules
open System.Text.RegularExpressions
open System.Drawing

type Pos = {
    X: int64;
    Y: int64;
}

type Robot = {
    Position: Pos;
    Velocity: Pos;
}

let INT64 s =
    System.Int64.Parse(s)

let parseContent(lines: string array) : Robot array =
    let extractValues(l: string) =
        let regexp = @"p=(-?\d+),(-?\d+)\s+v=(-?\d+),(-?\d+)"
        let matches = Regex.Match(l, regexp)
        {Position = {X = INT64 (matches.Groups[2].Value); Y = INT64 (matches.Groups[1].Value) };
            Velocity = { X = INT64 (matches.Groups[4].Value); Y = INT64 (matches.Groups[3].Value) }}
    lines
    |> Array.map extractValues

let move(point: Robot) (times: int64) maxRows maxCols =
    let newX = 
        match (point.Position.X + (point.Velocity.X * times))%maxRows with
        | x when x >= 0L && x < maxRows -> x
        | x when x < 0L -> maxRows + x
        | _ -> failwith "error"
    let newY = 
        match (point.Position.Y + (point.Velocity.Y * times))%maxCols with
        | y when y >= 0L && y < maxCols -> y
        | y when y < 0L -> maxCols + y
        | _ -> failwith "error"
    { point with Position = {X = newX; Y = newY} }

let moveAll(positions: Robot array) (times: int64) maxRows maxCols =
    positions
    |> Array.map(fun p -> move p times maxRows maxCols)

let belongToQ(position: Robot) (qfromX,qtoX,qfromY,qtoY) =
    position.Position.X >= qfromX && position.Position.X <= qtoX &&
        position.Position.Y >= qfromY && position.Position.Y <= qtoY

let getSectors(positions: Robot array) maxRows maxCols =
    let Q1fromX,Q1toX,Q1fromY,Q1toY = 0L,               (maxRows/2L)-1L,    0L,                 (maxCols/2L)-1L
    let Q2fromX,Q2toX,Q2fromY,Q2toY = 0L,               (maxRows/2L)-1L,    (maxCols/2L)+1L,    maxCols-1L
    let Q3fromX,Q3toX,Q3fromY,Q3toY = (maxRows/2L)+1L,  maxRows-1L,         0L,                 (maxCols/2L)-1L
    let Q4fromX,Q4toX,Q4fromY,Q4toY = (maxRows/2L)+1L,  maxRows-1L,         (maxCols/2L)+1L,    maxCols-1L
    let mapped =
        positions
        |> Array.map(fun p ->
            if belongToQ p (Q1fromX,Q1toX,Q1fromY,Q1toY) then
                (1L, p)
            elif belongToQ p (Q2fromX,Q2toX,Q2fromY,Q2toY) then
                (2L, p)
            elif belongToQ p (Q3fromX,Q3toX,Q3fromY,Q3toY) then
                (3L, p)
            elif belongToQ p (Q4fromX,Q4toX,Q4fromY,Q4toY) then
                (4L, p)
            else
                (0L, p)
        ) 
    mapped

// for printing the image...
let saveChristmasTreeAsImage(points: Robot array) (seconds: int) maxrows maxcols =
    let newpositions = moveAll points seconds maxrows maxcols
    if newpositions |> Array.map _.Position |> Set.ofArray |> Seq.length = newpositions.Length then
        let bmp = new Bitmap(int maxcols * 16, int maxrows * 16)
        let graphics = Graphics.FromImage(bmp)
        graphics.Clear(Color.White)

        let font = new Font("Segoe UI Emoji", 16.0f)
        let brush = new SolidBrush(Color.Black)

        for row in 0L..(maxrows-1L) do
            for col in 0L..(maxcols-1L) do
                match newpositions |> Array.tryFind(fun p -> p.Position.X = row && p.Position.Y = col) with
                | Some(_) ->
                    graphics.DrawString("🤖", font, brush, float32 (col * 16L), float32 (row * 16L))
                | None -> ()

        bmp.Save("christmastree.png")
        true
    else
        false


let printChristmasTree(points: Robot array) (seconds: int) maxrows maxcols =
    let newpositions = moveAll points seconds maxrows maxcols
    if newpositions |> Array.map _.Position |> Set.ofArray |> Seq.length = newpositions.Length then
        for row in 0L..(maxrows-1L) do
            for col in 0L..(maxcols-1L) do
                match newpositions |> Array.tryFind(fun p -> p.Position.X = row && p.Position.Y = col) with
                | Some(p) -> printf "🤖"
                | None -> printf "  "
            printfn ""
        printfn "%s" System.Environment.NewLine
        true
    else
        false

let calculateSafetyFactor(positions: Robot array) seconds maxRows maxCols = 
    let newpositions = moveAll positions seconds maxRows maxCols
    getSectors newpositions maxRows maxCols
    |> Array.filter(fun (s, p) -> s <> 0L)
    |> Array.groupBy fst
    |> Array.map(fun (k,g) -> g.Length)
    |> Array.reduce (*)

let execute() =
    let maxrows, maxcols = 103, 101
    let path = "day14/day14_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let positions = parseContent content
    let (seconds, safetyfactor) = 
        [0..(maxrows*maxcols)]
        |> List.map (fun seconds -> (seconds, calculateSafetyFactor positions  seconds maxrows maxcols))
        |> List.minBy snd
    // Enable for printing-saving image
    //printChristmasTree positions seconds maxrows maxcols
    //saveChristmasTreeAsImage positions seconds maxrows maxcols
    seconds
