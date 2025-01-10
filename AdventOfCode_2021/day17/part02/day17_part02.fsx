open System
open System.IO
open System.Text.RegularExpressions
open System.Collections.Generic
open System.Globalization

let path = "day17_input.txt"
//let path = "test_input.txt"        

let regexpattern = "target area: x=(?<x1>-?[0-9]+)..(?<x2>-?[0-9]+), y=(?<y1>-?[0-9]+)..(?<y2>-?[0-9]+)"
let extractparts(input: string, pattern:string) =
    let m = Regex.Match(input, pattern)
    if m.Success then [| for g in m.Groups -> g.Value |] |> Array.skip(1) |> Array.map(fun v -> int(v))
    else [|0; 0; 0; 0|]

let message = 
    File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> 
        Seq.map(fun l -> extractparts(l, regexpattern)) |> Seq.exactlyOne

let value = [|0|]
let (v, n) = (0, int(Math.Pow(float(message.[0]) * 2.0, 0.5) - 1.0))
value.[0] <- v
let rec doStep(input: int[], myinitY: int, myinitX: int, myX: int, myY: int, mydx: int, mydy: int, myv: int) =
    match (myX <= message.[1] && myY >= message.[2] && 
    (mydx = 0 && message.[0] <= myX || mydx <> 0)) with
    | true ->
        let newX = myX + mydx
        let newY = myY + mydy
        let newdx = if mydx > 0 then mydx - 1 else mydx
        let newdy = mydy - 1
        match [message.[0]..message.[1]] |> List.contains(newX) && [message.[2]..message.[3]] |> List.contains(newY) with
        | true ->
            printfn "Value in (%i, %i) = %i" myinitX myinitY (myv + 1)
            myv + 1
        | false -> doStep(input, myinitY, myinitX, newX, newY, newdx, newdy, myv)
    | false -> myv


for initY in message.[2]..(message.[2] * -1) do
    for initX in n..(message.[1] + 1) do
        let (x, y, dx, dy) = (0, 0, initX, initY)
        let newvalue = doStep(message, initY, initX, x, y, dx, dy, value.[0])
        value.[0] <- newvalue
value.[0]