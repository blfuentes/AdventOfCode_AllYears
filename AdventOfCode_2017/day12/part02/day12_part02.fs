module day12_part02
open System.Collections.Generic   
open AdventOfCode_2017.Modules

type Node = {
    Value: int
}

type Edge = {
    Connection: Node*Node
}

type Graph<'T> = Dictionary<'T, List<'T>>

let parseContent(lines: string array) =
    let thegraph = Dictionary<Node, Node list>()

    let edges =
        lines
        |> Array.collect(fun l -> 
            let fromnode = l.Split(" <-> ")[0] |> int
            let tonodes = (l.Split(" <-> ")[1]).Split(", ") |> Array.map int
            tonodes
            |> Array.map(fun tnode ->
               { Connection = ({ Value = fromnode }, { Value = tnode }) }
            )
        )
        |> Array.collect(fun e -> [| e; { Connection = (snd e.Connection,  fst e.Connection) }|])

    edges
    |> Array.groupBy(fun e -> fst e.Connection)   
    |> Array.iter(fun (n, edges) ->
        let toconnets = edges |> Array.map(fun e -> snd e.Connection) |> List.ofArray |> List.distinct
        thegraph.Add(n, toconnets)
    )

    thegraph

let isConnected (graph: Dictionary<Node, Node list>) (from: Node) =
    let rec collectNodes (start: Node) (visited: Set<Node>) =
        match graph.TryGetValue start with
        | false, _ -> visited
        | true, children ->
            let toProcess = children |> List.filter (fun c -> not (visited.Contains c))
            let updatedVisited = visited.Add start
            toProcess
            |> List.fold (fun acc child -> collectNodes child acc) updatedVisited

    collectNodes from Set.empty

let findGroups(graph: Dictionary<Node, Node list>) =
    let groups = 
        graph.Keys
        |> Seq.map(fun k -> isConnected graph k)
        |> Seq.distinct
        |> List.ofSeq
    groups

let execute() =
    let path = "day12/day12_input.txt"

    let content = LocalHelper.GetLinesFromFile path
    let graph = parseContent content
    let groups = findGroups graph
    groups.Length