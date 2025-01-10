module day23_part01

open AdventOfCode_2024.Modules
open AdventOfCode_Utilities

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
    
let findVertexWithT ((graphdescription, edges, nodes): Map<Node, Node list> * Set<Edge> * Set<Node>) =
    let nodesWithT = 
        nodes 
        |> Set.filter (fun n -> n.Name.StartsWith("t"))

    let possibleGroups =
        nodesWithT
        |> Seq.collect (fun n -> 
            let neighbors = graphdescription[n]
            Utilities.combination 2 neighbors
            |> Seq.map (fun nodes -> (n, nodes.Item(0), nodes.Item(1)))
        )

    let validGroups =
        possibleGroups
        |> Seq.filter (fun (first, second, third) ->
            Set.contains { Connection = (first, second) } edges &&
            Set.contains { Connection = (second, third) } edges &&
            Set.contains { Connection = (first, third) } edges
        )

    let triangles =
        validGroups
        |> Seq.map (fun (first, second, third) -> Set.ofList [ first; second; third ])
        |> Set.ofSeq

    Set.count triangles

let execute() =
    let path = "day23/day23_input.txt"
    let content = LocalHelper.GetLinesFromFile path

    let connections = parseContent content
    let (graph, edges, nodes) = buildGraph connections
    findVertexWithT (graph, edges, nodes)
