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

let heuristic (a: int * int) (b: int * int) =
    let dx = float (fst b - fst a)
    let dy = float (snd b - snd a)
    Math.Abs(dx) + Math.Abs(dy) // Manhattan distance

let neighbors (grid: int list list) (position: int * int) =
    let x, y = position
    let directions = [(0, -1); (0, 1); (1, 0); (-1, 0)]
    directions
    |> List.map (fun (dx, dy) -> (x + dx, y + dy))
    |> List.filter (fun (nx, ny) -> 
        nx >= 0 && ny >= 0 && nx < List.length grid && ny < List.length (List.head grid))

let aStar (grid: int list list) (start: int * int) (goal: int * int) =
    let openSet = CustomPriorityQueue()
    let startNode = { Position = start; Cost = 0.0; Heuristic = heuristic start goal; Parent = None }
    openSet.Enqueue(startNode)

    let visited = Dictionary<(int * int), float>() // Stores the best known cost to each position

    let rec search () =
        if openSet.Count > 0 then
            let currentNode = openSet.Dequeue()
            if currentNode.Position = goal then
                // Reconstruct the path
                let rec buildPath node path =
                    match node.Parent with
                    | Some parent -> buildPath parent (node.Position :: path)
                    | None -> node.Position :: path
                buildPath currentNode []
            else
                for neighbor in neighbors grid currentNode.Position do
                    let nx, ny = neighbor
                    let neighborCost = float (List.item ny (List.item nx grid)) // Corrected grid access
                    let newCost = currentNode.Cost + neighborCost
                    if not (visited.ContainsKey neighbor) || newCost < visited.[neighbor] then
                        visited.[neighbor] <- newCost
                        let neighborNode = { Position = neighbor; Cost = newCost; Heuristic = heuristic neighbor goal; Parent = Some currentNode }
                        openSet.Enqueue(neighborNode)
                search ()
        else
            [] // No path found

    search ()

// Example usage
let grid = [
    [1; 10; 5; 1; 1];
    [1; 2; 2; 2; 2];
    [3; 2; 3; 2; 3];
    [1; 2; 2; 2; 1];
    [1; 1; 15; 1; 1]
]
let start = (0, 0)
let goal = (4, 4)
let path = aStar grid start goal

printfn "Path: %A" path
