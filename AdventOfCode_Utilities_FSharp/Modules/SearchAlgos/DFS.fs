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

// DFS Function
let dfs (graph: IDictionary<'T, 'T list>) (start: 'T) =
    let visited = HashSet<'T>()        // Keep track of visited nodes
    let stack = Stack<'T>()            // Use a stack for DFS
    let result = ResizeArray<'T>()     // To store the traversal order

    stack.Push(start)

    while stack.Count > 0 do
        let node = stack.Pop()
        if not (visited.Contains(node)) then
            visited.Add(node)
            result.Add(node)
            // Push neighbors to the stack (reverse to maintain order)
            graph.[node] |> List.iter (fun neighbor -> stack.Push(neighbor))

    result |> Seq.toList

// Example usage
let startNode = "A"
let dfsResult = dfs graph startNode

printfn "DFS Traversal starting from '%s': %A" startNode dfsResult
