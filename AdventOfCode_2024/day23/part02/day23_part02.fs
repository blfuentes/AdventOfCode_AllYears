module day23_part02

open AdventOfCode_2024.Modules

type Node = {
    Name: string
}

type Edge = {
    Connection: Node*Node
}

let parseContent(lines: string array) =
    lines
    |> Array.collect(fun line -> 
        let parts = line.Split("-")
        [|
            { Connection = { Name = parts[0] }, { Name = parts[1] } }; 
            { Connection = { Name = parts[1] }, { Name = parts[0] } }
        |]
    )

let buildGraph (connections: Edge array) =
    let edges = Set.ofArray connections

    let graphDescription =
        let groupedEdges = 
            connections 
            |> Seq.groupBy (fun edge -> fst edge.Connection)

        groupedEdges
        |> Seq.map (fun (node, edges) ->
            let connectedNodes = 
                edges 
                |> Seq.map (fun edge -> snd edge.Connection)
                |> List.ofSeq
            (node, connectedNodes)
        )
        |> Map.ofSeq

    let nodes =
        connections
        |> Array.collect (fun edge -> 
            let (fromNode, toNode) = edge.Connection
            [| fromNode; toNode |]
        )
        |> Set.ofArray

    (graphDescription, edges, nodes)
 
// https://en.wikipedia.org/wiki/Clique_problem
let rec biggestClique ((cliques, remaining, edges): (Node list*Node list*Set<Edge>)) =
    match remaining with
    | [] -> cliques
    | fromNode :: rest -> 
        if cliques |> Seq.forall (fun node -> Set.contains { Connection = (node, fromNode) } edges) then 
            biggestClique ((fromNode :: cliques), rest, edges)
        else 
            biggestClique(cliques, rest, edges)
        
let findMaxLanParty ((edges, nodes): Set<Edge> * Set<Node>) =
    let cliques =
        nodes
        |> Seq.map (fun n -> biggestClique([n], List.ofSeq nodes, edges))

    let largestClique =
        cliques |> Seq.maxBy List.length

    let sortedNodeNames =
        largestClique
        |> List.map (fun node -> node.Name)
        |> List.sort

    String.concat "," sortedNodeNames

let execute() =
    let path = "day23/day23_input.txt"

    let content = LocalHelper.GetLinesFromFile path
    let connections = parseContent content
    let (_, edges, nodes) = buildGraph connections
    findMaxLanParty (edges, nodes)