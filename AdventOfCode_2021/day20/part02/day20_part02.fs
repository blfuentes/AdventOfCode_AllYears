module day20_part02

open AdventOfCode_2021.Modules
open System

let parseContent(input: string) =
    let split = 
        input.Split(Environment.NewLine + Environment.NewLine)

    let imageEnhancementAlgorithm = 
        split[0]

    let inputImage = 
        split[1].Split(Environment.NewLine)
        |> Seq.mapi (fun y row -> 
            row.ToCharArray()
            |> Seq.mapi (fun x pixel -> (x, y), pixel))
        |> Seq.concat
        |> Map

    imageEnhancementAlgorithm, inputImage

let printImage (inputImage: Map<int * int,char>) =
    let xMin = inputImage |> Map.keys |> Seq.map (fun (x, _) -> x) |> Seq.min
    let xMax = inputImage |> Map.keys |> Seq.map (fun (x, _) -> x) |> Seq.max
    let yMin = inputImage |> Map.keys |> Seq.map (fun (_, y) -> y) |> Seq.min
    let yMax = inputImage |> Map.keys |> Seq.map (fun (_, y) -> y) |> Seq.max

    [
        for y = yMin to yMax do
            [
                for x = xMin to xMax do
                    inputImage[(x, y)]
            ]
            |> String.Concat
    ]
    |> fun line -> String.Join(Environment.NewLine, line)

let enhance infinitePixelValue imageEnhancementAlgorithm (inputImage: Map<int * int,char>) = 
    let xMin = inputImage |> Map.keys |> Seq.map (fun (x, _) -> x) |> Seq.min
    let xMax = inputImage |> Map.keys |> Seq.map (fun (x, _) -> x) |> Seq.max
    let yMin = inputImage |> Map.keys |> Seq.map (fun (_, y) -> y) |> Seq.min
    let yMax = inputImage |> Map.keys |> Seq.map (fun (_, y) -> y) |> Seq.max
    
    let neighbors (x, y) = 
        [
            (-1, -1) // top left
            ( 0, -1) // top
            ( 1, -1) // top right
            (-1,  0) // left
            ( 0,  0) // same
            ( 1,  0) // right
            (-1,  1) // bottom left
            ( 0,  1) // down
            ( 1,  1) // bottom right
        ]
        |> List.map (fun (x', y') -> x + x', y + y')
    
    let lookup (x, y) = 
        inputImage
        |> Map.tryFind (x, y)
        |> fun pixel -> 
            match pixel with
            | Some p -> p
            | None -> infinitePixelValue

    let enhancePixel (x, y) =
        (x, y)
        |> neighbors
        |> Seq.map lookup
        |> Seq.map (fun c -> 
            match c with
            | '.' -> '0'
            | '#' -> '1'
            | _ -> failwith "Invalid pixel value.")
        |> String.Concat
        |> fun bstr -> Convert.ToInt32(bstr, 2)
        |> fun index -> imageEnhancementAlgorithm |> Seq.item index
    
    Seq.allPairs [xMin - 1 .. xMax + 1] [yMin - 1 .. yMax + 1]
    |> Seq.map (fun (x, y) -> (x, y), enhancePixel (x, y))
    |> Map

let infinitePixelPattern (imageEnhancementAlgorithm: string) = 
    match imageEnhancementAlgorithm[0] with
    | '#' -> Seq.initInfinite (fun i -> if i % 2 = 0 then '.' else '#')
    | _ -> Seq.initInfinite (fun i -> '.')

let execute =
    let path = "day20/day20_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let (imageEnhancementAlgorithm, inputImage) = parseContent content
    let pattern = infinitePixelPattern imageEnhancementAlgorithm

    pattern
    |> Seq.truncate 50
    |> Seq.fold (fun image infinitePixelValue -> enhance infinitePixelValue imageEnhancementAlgorithm image) inputImage
    |> Map.values
    |> Seq.filter (fun pixel -> pixel = '#')
    |> Seq.length