open System.Collections.Generic

// Define the graph as an adjacency list
let graph = 
    dict [
        "A", ["B"; "C"];
        "B", ["D"; "E"];
        "C", ["F"];
        "D", [];
        "E", ["F"];
        "F", []
    ]

// BFS Function
let bfs (graph: IDictionary<'T, 'T list>) (start: 'T) =
    let visited = HashSet<'T>()        // Keep track of visited nodes
    let queue = Queue<'T>()            // Use a queue for BFS
    let result = ResizeArray<'T>()     // To store the traversal order

    queue.Enqueue(start)
    visited.Add(start)

    while queue.Count > 0 do
        let node = queue.Dequeue()
        result.Add(node)

        // Enqueue unvisited neighbors
        graph.[node] |> List.iter (fun neighbor ->
            if not (visited.Contains(neighbor)) then
                visited.Add(neighbor)
                queue.Enqueue(neighbor)
        )

    result |> Seq.toList

// Example usage
let startNode = "A"
let bfsResult = bfs graph startNode

printfn "BFS Traversal starting from '%s': %A" startNode bfsResult
