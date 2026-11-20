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

(message.[2] + 1) * message.[2] / 2