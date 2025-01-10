open System
open System.Collections.Generic

type Node<'a> = {
    Position: 'a
    Cost: float
    Heuristic: float
    Parent: Node<'a> option
}

type CustomPriorityQueue<'a> () =
    let elements = new SortedDictionary<float, Queue<Node<'a>>>() // Priority -> Queue of nodes with same priority

    member this.Enqueue (node: Node<'a>) =
        let priority = node.Cost + node.Heuristic
        if not (elements.ContainsKey priority) then
            elements.[priority] <- new Queue<Node<'a>>()
        elements.[priority].Enqueue(node)

    member this.Dequeue () =
        let firstKey = elements.Keys |> Seq.head
        let queue = elements.[firstKey]
        let node = queue.Dequeue()
        if queue.Count = 0 then
            elements.Remove(firstKey) |> ignore
        node

    member this.Count = elements.Values |> Seq.sumBy (fun q -> q.Count)

/// A* Algorithm for Graphs
let aStar 
    (graph: Dictionary<'a, ('a * float) list>) // Graph as an adjacency list: node -> [(neighbor, weight)]
    (heuristic: 'a -> 'a -> float)            // Heuristic function
    (start: 'a)                               // Start node
    (goal: 'a)                                // Goal node
    =
    let openSet = CustomPriorityQueue()
    let startNode = { Position = start; Cost = 0.0; Heuristic = heuristic start goal; Parent = None }
    openSet.Enqueue(startNode)

    let visited = Dictionary<'a, float>() // Tracks the best known cost to each node

    let rec search () =
        if openSet.Count > 0 then
            let currentNode = openSet.Dequeue()
            if currentNode.Position = goal then
                // Reconstruct path
                let rec buildPath node path =
                    match node.Parent with
                    | Some parent -> buildPath parent (node.Position :: path)
                    | None -> node.Position :: path
                buildPath currentNode []
            else
                for (neighbor, weight) in graph.[currentNode.Position] do
                    let newCost = currentNode.Cost + weight
                    if not (visited.ContainsKey neighbor) || newCost < visited.[neighbor] then
                        visited.[neighbor] <- newCost
                        let neighborNode = { Position = neighbor; Cost = newCost; Heuristic = heuristic neighbor goal; Parent = Some currentNode }
                        openSet.Enqueue(neighborNode)
                search ()
        else
            [] // No path found

    search ()

/// Example usage

// Graph as an adjacency list (node -> [(neighbor, weight)])
let graph = Dictionary<string, (string * float) list>()
graph.["A"] <- [("B", 1.0); ("C", 4.0)]
graph.["B"] <- [("D", 1.0); ("E", 2.0)]
graph.["C"] <- [("F", 5.0)]
graph.["D"] <- []
graph.["E"] <- [("F", 1.0)]
graph.["F"] <- []

// Heuristic function (example: straight-line distance to the goal)
let heuristic (node: string) (goal: string) =
    match node, goal with
    | "A", "F" -> 6.0
    | "B", "F" -> 4.0
    | "C", "F" -> 5.0
    | "D", "F" -> 3.0
    | "E", "F" -> 1.0
    | "F", "F" -> 0.0
    | _ -> 0.0

// Run the A* algorithm
let start = "A"
let goal = "F"
let path = aStar graph heuristic start goal

printfn "Path from '%s' to '%s': %A" start goal path
