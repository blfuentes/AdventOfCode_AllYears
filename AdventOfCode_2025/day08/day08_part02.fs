module day08_part02

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

let buildSingleCircuit (boxes: Box array) =
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
    
    let getCircuitCount () =
        let roots = System.Collections.Generic.HashSet<int>()
        for i in 0 .. boxes.Length - 1 do
            roots.Add(find i) |> ignore
        roots.Count
    
    let allPairs = 
        [| for i in 0 .. boxes.Length - 1 do
            for j in i + 1 .. boxes.Length - 1 do
                let distance = euclideanDistanceSquared boxes[i] boxes[j]
                yield (i, j, distance)
        |]
        |> Array.sortBy (fun (_, _, dist) -> dist)
    
    let rec processUntilOnlyOne pairIndex =
        if pairIndex >= allPairs.Length then
            failwith "cannot build single circuit"
        else
            let (idA, idB, _) = allPairs[pairIndex]
            union idA idB |> ignore
            
            if getCircuitCount() = 1 then
                (idA, idB)
            else
                processUntilOnlyOne (pairIndex + 1)
    
    let (idA, idB) = processUntilOnlyOne 0
    //printfn "single circuit is box %d and box %d" idA idB
    
    boxes[idA].X * boxes[idB].X

let execute() =
    //let path = "day08/test_input_08.txt"
    let path = "day08/day08_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let boxes = parseContent content
    buildSingleCircuit boxes
