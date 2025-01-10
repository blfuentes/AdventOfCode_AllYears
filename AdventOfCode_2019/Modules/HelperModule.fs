namespace AoC_2019.Modules

open System.Collections.Generic
open System.Text.RegularExpressions

[<AutoOpen>]
module HelperModule =
    type DirectionEnum = UP | DOWN | LEFT | RIGHT

    let bigint (x:int) = bigint(x)

    //let int (x:bigint) = int(x)

    let createPanel(size: int, color: int) =
        let panel = new Dictionary<(int * int), int>()
        for idx in [-50 .. size] do
            for jdx in [-50 .. size] do
                panel.Add((jdx, idx), color)      
        panel
    
    let printPanel(panel: Dictionary<(int*int),int>, width: int, height: int) =
        for idx in [0 .. height] do
            for jdx in [0 .. width] do
                let color = panel.[(jdx, idx)]
                match color with
                | 1 -> 
                    printf "#"
                    ()
                | _ -> 
                    printf " "
                    ()
            printfn ""



    let getNextPosition(direction: DirectionEnum, position:int[]) =
        match direction with
        | UP -> (direction, [|position.[0]; position.[1] - 1|])
        | DOWN -> (direction, [|position.[0]; position.[1] + 1|])
        | LEFT -> (direction, [|position.[0] - 1; position.[1]|])
        | RIGHT -> (direction, [|position.[0] + 1; position.[1]|])

    let (|Regex|_|) pattern input =
        let m = Regex.Match(input, pattern)
        if m.Success then Some(List.tail [ for g in m.Groups -> g.Value ])
        else None

    let distrib e L =
        let rec aux pre post = 
            seq {
                match post with
                | [] -> yield (L @ [e])
                | h::t -> yield (List.rev pre @ [e] @ post)
                          yield! aux (h::pre) t 
            }
        aux [] L

    let rec perms = function 
    | [] -> Seq.singleton []
    | h::t -> Seq.collect (distrib h) (perms t)

    let tern(x:int, p:int): int =
        match x with
        | x when x < p -> 1
        | x when x > p ->  -1
        | _ -> 0