module day25_part01


open AdventOfCode_2023.Modules
open System

let random = new Random()

let rec find (parent: int array) node =
    if node = parent[node] then
        node
    else
        let p = find parent (parent[node])
        parent[node] <- p
        p

let union (parent: int array) (rank: int array) u v =
    let mutable u, v = find parent u, find parent v

    if u <> v then
        if rank[u] < rank[v] then
            let temp = u
            u <- v
            v <- temp

        parent[v] <- u

        if rank[u] = rank[v] then
            rank[u] <- rank[u] + 1

// https://www.scaler.com/topics/data-structures/kargers-algorithm-for-minimum-cut/
let minimalCut (edges: (string * string) array) (vertices: string array) =
    let verticeIds =
        vertices |> Array.indexed |> Array.map (fun (idx, vId) -> vId, idx) |> Map.ofArray
    let numOfVertex = Array.length vertices
    let fromV = Array.init numOfVertex id
    let rank = Array.create numOfVertex 0
    let mutable vs = numOfVertex

    while vs > 2 do
        let i = random.Next(Array.length edges)
        let u, v = verticeIds[fst edges[i]], verticeIds[snd edges[i]]

        if (find fromV u <> find fromV v) then
            union fromV rank u v
            vs <- vs - 1

    let edgesToCut =
        edges
        |> Seq.filter (fun (e1, e2) ->
            let u, v = verticeIds[e1], verticeIds[e2]
            find fromV u <> find fromV v)
        |> Seq.length

    let set1, set2 =
        fromV |> Array.countBy id |> Array.map snd |> (fun arr -> arr[0], arr[1])

    edgesToCut, set1, set2


let execute =
    let path = "day25/day25_input.txt"
    let input =
        LocalHelper.GetLinesFromFile path
        |> Array.map (fun line -> line.Split(" ") |> Array.map (fun def -> def.Split(":").[0]))
    let edges =
        input
        |> Seq.collect (fun comps -> comps[1..] |> List.ofArray |> List.map (fun v -> comps[0], v))
        |> Array.ofSeq

    let vertex =
        edges |> Seq.collect (fun (a, b) -> [ a; b ]) |> Seq.distinct |> Array.ofSeq

    let result = 
        Seq.unfold
            (fun _ ->
                let cutEdges, leftSection, rightSection = (minimalCut edges vertex)
                Some((cutEdges, leftSection, rightSection), ()))
            ()
        |> Seq.find (fun (cutEdges, _, _) -> cutEdges = 3)
        |> fun (_, leftSection, rightSection) -> leftSection * rightSection
    result