module day22_part01

open AdventOfCode_2021.Modules

open System

type Cube = { 
    on: bool
    min: int * int * int
    max: int * int * int 
}

let (|Int|_|) x =
    match Int32.TryParse(x: string) with
    | true, i -> Some i
    | false, _ -> None

let parseContent(lines: string array) =
    lines
    |> Array.map (fun line ->
        match line.Split([| ','; ' '; '='; '.'; 'x'; 'y'; 'z' |], StringSplitOptions.RemoveEmptyEntries) with
        | [| onOff; Int xMin; Int xMax; Int yMin; Int yMax; Int zMin; Int zMax |] when onOff = "on" || onOff = "off" ->
            { 
                on = (onOff = "on")
                min = (xMin, yMin, zMin)
                max = (xMax, yMax, zMax)
            }
        | _ -> failwith "Invalid input")

let apply f (a1, b1, c1) (a2, b2, c2) = (f a1 a2, f b1 b2, f c1 c2)

let disjoint (c1: Cube) (c2: Cube) =
    let anyLess (x1, y1, z1) (x2, y2, z2) = x1 < x2 || y1 < y2 || z1 < z2
    anyLess c1.max c2.min || anyLess c2.max c1.min

let intersect (c1: Cube) (c2: Cube) on =
    if disjoint c1 c2 then
        None
    else
        { 
            on = on
            min = apply max c1.min c2.min
            max = apply min c1.max c2.max 
        }
        |> Some

let addCubes =
    Seq.fold
        (fun cubes c ->
            cubes
            |> List.choose (fun otherCube -> intersect c otherCube (not otherCube.on))
            |> fun newCubes -> if c.on then c :: newCubes else newCubes
            |> List.append cubes)
        []

let volume (cube: Cube) =
    apply (fun a b -> (int64) a - (int64) b + 1L) cube.max cube.min
    |> fun (x, y, z) -> x * y * z

let finalVolume (cubes: Cube seq) =
    Seq.sumBy (fun c -> volume c * (if c.on then 1L else -1L)) cubes

let findFinalVolume(cubes: Cube array) =
    cubes
    |> Seq.choose (fun c ->
        intersect
            c
            { 
                    on = true
                    min = (-50, -50, -50)
                    max = (50, 50, 50)
                }
            c.on)
    |> addCubes
    |> finalVolume

let execute =
    let path = "day22/day22_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let cubes = parseContent content
    findFinalVolume cubes