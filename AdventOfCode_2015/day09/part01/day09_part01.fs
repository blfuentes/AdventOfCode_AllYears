module day09_part01

open AdventOfCode_2015.Modules

type Node = { Id: string; Neighbors: (string * int) list }

let parseDefinition(definition: string) =
    let regexstr = "(\\w+) to (\\w+) = (\\d+)"
    let regex = System.Text.RegularExpressions.Regex(regexstr)
    let defs = regex.Match(definition)
    (defs.Groups.[1].Value, defs.Groups.[2].Value, (int)defs.Groups.[3].Value)

let rec parseLines(lines: string list) (nodes: Node list): Node list =
    match lines with
    | [] -> nodes
    | head :: tail ->
        let (origin, destination, distance) = (parseDefinition head)
        let oldList = nodes |> List.filter (fun n -> n.Id <> origin && n.Id <> destination)
        let newOrigin =
            match nodes |> List.tryFind (fun n -> n.Id = origin) with
            | Some node ->
                { node with Neighbors = (destination, distance) :: node.Neighbors }
            | None ->
                { Id = origin; Neighbors = [(destination, distance)] }
        let newDestionation =
            match nodes |> List.tryFind (fun n -> n.Id = destination) with
            | Some node ->
                { node with Neighbors = (origin, distance) :: node.Neighbors }
            | None ->
                { Id = destination; Neighbors = [(origin, distance)] }
        parseLines tail ([newOrigin; newDestionation] @ oldList)

let rec permutations (list: 'a list) =
    match list with
    | [] -> [[]]
    | _ -> List.collect (fun x -> List.map (fun p -> x::p) (permutations (List.filter ((<>) x) list))) list

let rec calculateDistance (nodes: string list) (graph: Node list) =
    match nodes with
    | [] -> 0
    | [_] -> 0
    | node1 :: node2 :: rest ->
        let distance = graph |> List.find (fun n -> n.Id = node1) |> fun n -> n.Neighbors |> List.find (fun (n, d) -> n = node2) |> snd
        distance + calculateDistance (node2 :: rest) graph

let shortestPathLength graph =
    permutations (graph |> List.map (fun n -> n.Id))
    |> List.map (fun perm -> calculateDistance perm graph)
    |> List.min

let execute =
    let path = "day09/day09_input.txt"

    let lines = LocalHelper.GetLinesFromFile path |> List.ofSeq
    let graph = parseLines lines []
    shortestPathLength graph