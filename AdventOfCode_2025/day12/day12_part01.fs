module day12_part01

open AdventOfCode_2025.Modules

type Shape = {
    Id: int
    Description: bool[,]
}

type Region = {
    Id: int
    Dimensions: int * int
    ShapesCount: int list
}

let parseContent (lines: string) =
    let allLines = lines.Split(System.Environment.NewLine, System.StringSplitOptions.RemoveEmptyEntries)
    
    let regionStartIdx = 
        allLines 
        |> Array.tryFindIndex (fun line -> line.Contains("x") && line.Contains(":"))
        |> Option.defaultValue allLines.Length
    
    let shapes = 
        allLines[0..regionStartIdx-1]
        |> Array.fold (fun (currentShapes: ResizeArray<Shape>, currentId: int option, currentRows: ResizeArray<string>) line ->
            if line.EndsWith(":") then
                let updatedShapes = 
                    match currentId with
                    | Some id when currentRows.Count > 0 ->
                        let height = currentRows.Count
                        let width = if height > 0 then currentRows[0].Length else 0
                        let desc = Array2D.init height width (fun row col -> 
                            currentRows[row][col] = '#')
                        currentShapes.Add({ Id = id; Description = desc })
                        currentShapes
                    | _ -> currentShapes
                
                let newId = int(line[0].ToString())
                (updatedShapes, Some newId, ResizeArray<string>())
            else
                currentRows.Add(line.Trim())
                (currentShapes, currentId, currentRows)
        ) (ResizeArray<Shape>(), None, ResizeArray<string>())
        |> fun (shapes, lastId, lastRows) ->
            match lastId with
            | Some id when lastRows.Count > 0 ->
                let height = lastRows.Count
                let width = if height > 0 then lastRows[0].Length else 0
                let desc = Array2D.init height width (fun row col -> 
                    lastRows[row][col] = '#')
                shapes.Add({ Id = id; Description = desc })
            | _ -> ()
            shapes |> Seq.toArray
    
    let regions = 
        if regionStartIdx < allLines.Length then
            allLines[regionStartIdx..]
            |> Array.mapi (fun idx line ->
                let parts = line.Split(':')
                let dimensions = parts[0].Trim().Split('x')
                let width = int(dimensions[0])
                let height = int(dimensions[1])
                
                let counts = 
                    parts[1].Trim().Split(' ', System.StringSplitOptions.RemoveEmptyEntries)
                    |> Array.map int
                    |> List.ofArray
                
                { Id = idx; Dimensions = (width, height); ShapesCount = counts }
            )
        else
            [||]
    
    (shapes, regions)

let canFit (shapes: Shape array) (region: Region) =
    let (regionWidth, regionHeight) = region.Dimensions
    let regionArea = regionWidth * regionHeight
    
    let filledSize = 
        Array.zip shapes (region.ShapesCount |> List.toArray)
        |> Array.sumBy (fun (shape, qty) ->
            let shapeArea = 
                shape.Description 
                |> Seq.cast<bool> 
                |> Seq.filter id 
                |> Seq.length
            shapeArea * qty
        )
    
    regionArea >= filledSize

let countFits(shapes: Shape array) (regions: Region array) =
    regions |> Array.sumBy (fun region ->
        if canFit shapes region then 1 else 0
    )

let execute() =
    //let path = "day12/test_input_12.txt"
    let path = "day12/day12_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let (shapes, regions) = parseContent content
    
    countFits shapes regions