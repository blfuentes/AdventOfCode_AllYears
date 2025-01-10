namespace AdventOfCode_Utilities.Modules.Graph

module Graph =
    type Graph<'T when 'T: comparison> =
        {
            Nodes: Set<'T>
            Edges: Set<('T * 'T)>
        }
        static member Empty = { Nodes = Set.empty; Edges = Set.empty }
    
    let addNode node (graph: Graph<'T>) =
        { graph with Nodes = graph.Nodes.Add(node) }
    
    let addEdge edge (graph: Graph<'T>) =
        { graph with Edges = graph.Edges.Add(edge) }
    
    let nodeExists node (graph: Graph<'T>) =
        graph.Nodes.Contains(node)
    
    let getNeighbors node (graph: Graph<'T>) =
        graph.Edges
        |> Set.filter (fun (u, v) -> u = node || v = node)
        |> Set.map (fun (u, v) -> if u = node then v else u)
        |> Set.toList

    let bfs start (graph: Graph<'T>) =
        let rec bfsImpl (visited: Set<'T>) (queue: List<'T>) =
            match queue with
            | [] -> visited
            | node :: tail ->
                if not (visited.Contains(node)) then
                    let neighbors = getNeighbors node graph
                    bfsImpl (visited.Add(node)) (tail @ neighbors)
                else
                    bfsImpl visited tail
        bfsImpl Set.empty [start]

    let dfs start (graph: Graph<'T>) =
        let rec dfsImpl (visited: Set<'T>) (stack: List<'T>) =
            match stack with
            | [] -> visited
            | node :: tail ->
                if not (visited.Contains(node)) then
                    let neighbors = getNeighbors node graph
                    dfsImpl (visited.Add(node)) (neighbors @ tail)
                else
                    dfsImpl visited tail
        dfsImpl Set.empty [start]


//let graph = 
//    { Nodes = set ['A'; 'B'; 'C'; 'D'; 'E']
//      Edges = set [('A', 'B'); ('A', 'C'); ('B', 'D'); ('C', 'E'); ('D', 'E')] }

//let result = bfs 'A' graph
//printfn "%A" result // Output: Set ['A'; 'B'; 'C'; 'D'; 'E']