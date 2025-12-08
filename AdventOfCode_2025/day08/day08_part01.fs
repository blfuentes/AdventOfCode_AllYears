module day08_part01

open AdventOfCode_2025.Modules

type Box = {
    Id: int
    X: bigint
    Y: bigint
    Z: bigint
}

let parseContent(lines: string array) : Box array =
    lines
    |> Array.mapi (fun i line ->
        let parts = line.Split ','
        {
            Id = i
            X = bigint.Parse(parts[0])
            Y = bigint.Parse(parts[1])
            Z = bigint.Parse(parts[2])
        }
    )

let euclideanDistanceSquared (boxA: Box) (boxB: Box) : bigint =
    (boxA.X - boxB.X) * (boxA.X - boxB.X) +
    (boxA.Y - boxB.Y) * (boxA.Y - boxB.Y) +
    (boxA.Z - boxB.Z) * (boxA.Z - boxB.Z)

let findConnections (boxes: Box array) numPairsToProcess =
    let parent = Array.init boxes.Length id
    
    let rec find x =
        if parent[x] = x then x
        else
            parent[x] <- find parent[x]
            parent[x]
    
    let union x y =
        let rootX = find x
        let rootY = find y
        if rootX <> rootY then
            parent[rootY] <- rootX
            true
        else
            false
    
    let allPairs = 
        [| for i in 0 .. boxes.Length - 1 do
            for j in i + 1 .. boxes.Length - 1 do
                let distance = euclideanDistanceSquared boxes[i] boxes[j]
                yield (i, j, distance)
        |]
        |> Array.sortBy (fun (_, _, dist) -> dist)
    
    //printfn "processing first %d pairs:" numPairsToProcess
    
    for pairIndex in 0 .. (min (numPairsToProcess - 1) (allPairs.Length - 1)) do
        let (i, j, dist) = allPairs[pairIndex]
        union i j |> ignore
        //if union i j then
        //    printfn "Connect Box %d with Box %d (distance: %A)" i j dist
        //else
        //    printfn "Skip Box %d with Box %d" i j
    
    let circuitSizes = System.Collections.Generic.Dictionary<int, int>()
    for i in 0 .. boxes.Length - 1 do
        let root = find i
        if circuitSizes.ContainsKey(root) then
            circuitSizes[root] <- circuitSizes[root] + 1
        else
            circuitSizes[root] <- 1
    //printfn ""
    //printfn "Circuit sizes: %A" (circuitSizes.Values |> Seq.sortDescending |> Seq.toList)
    
    circuitSizes.Values
    |> Seq.sortByDescending id
    |> Seq.take 3
    |> Seq.fold (fun acc size -> acc * bigint size) bigint.One


let execute() =
    //let path = "day08/test_input_08.txt"
    //let numOfConnections = 10
    let path = "day08/day08_input.txt"
    let numOfConnections = 1000
    let content = LocalHelper.GetLinesFromFile path
    let boxes = parseContent content
    findConnections boxes numOfConnections