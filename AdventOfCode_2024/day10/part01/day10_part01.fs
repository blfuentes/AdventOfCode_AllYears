module day10_part01

open AdventOfCode_2024.Modules

type Node = {
    Name: int;
    Row: int;
    Col: int;
    Neighbours : Node list;
}

let isInBoundaries (row: int) (col: int) (maxRows: int) (maxCols: int) =
    row >= 0 && row < maxRows && col >= 0 && col < maxCols

let buildGraph (map: int[,]) =
    let maxRows = Array2D.length1 map
    let maxCols = Array2D.length2 map

    let nodes =
        [ for row in 0 .. maxRows - 1 do
            for col in 0 .. maxCols - 1 do
                let value = map[row, col]
                if value > -1 then
                    yield (row, col), { Name = value; Row = row; Col = col; Neighbours = [] } ]
        |> Map.ofList

    let getNeighbors row col =
        [ (row - 1, col)
          (row + 1, col)
          (row, col - 1)
          (row, col + 1)]
        |> List.choose (fun (rowIdx, colIdx) ->
            if isInBoundaries rowIdx colIdx maxRows maxCols then
                Map.tryFind (rowIdx, colIdx) nodes
            else
                None)

    nodes
    |> Map.map (fun (row, col) node ->
        { node with Neighbours = getNeighbors row col })
    |> Map.toList
    |> List.map snd

let parseContent (lines: string array) =
    let maxRows = lines.Length
    let maxCols = lines[0].Length
    let map = Array2D.create maxRows maxCols -1

    for row in 0..maxRows - 1 do
        for col in 0..maxCols - 1 do
            map[row, col] <- (int)(lines[row][col]) - int '0'

    map

let findHeads (nodes: Node list) =
    nodes |> List.filter(fun node -> node.Name = 0)

let isComplete (startNode: Node) (connections: Node list) =
    let nodeMap =
        connections
        |> List.map (fun node -> (node.Row, node.Col), node)
        |> Map.ofList

    let rec foundValidTrails (currentNode: Node)=
        if currentNode.Name = 9 then
            [currentNode]
        else
            match nodeMap.TryFind((currentNode.Row, currentNode.Col)) with
            | Some c ->
                c.Neighbours
                |> List.filter (fun neighbor -> neighbor.Name - currentNode.Name = 1)
                |> List.collect (fun neighbor -> foundValidTrails neighbor)
            | None -> []
    foundValidTrails startNode
    |> List.distinct

let execute() =
    let path = "day10/day10_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    
    let connections = 
        parseContent content |> buildGraph
    findHeads connections
    |> List.collect (fun h -> isComplete h connections)
    |> List.length
