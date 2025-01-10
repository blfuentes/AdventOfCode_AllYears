open System.IO

let filepath = __SOURCE_DIRECTORY__ + @"../../day08_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input.txt"

let width = 25
let height = 6

let values = File.ReadAllText(filepath).ToCharArray() |> Array.map string |> Array.map int (*|> Array.toList*)
let layers = values |> Array.chunkBySize(width * height)

let fewestZerosList = layers |> Array.maxBy(fun _list -> _list |> Array.countBy(fun x -> x <> 0))
let numberOfOnes = fewestZerosList |> Array.filter(fun _x -> _x = 1) |> Array.length
let numberOfTwos = fewestZerosList |> Array.filter(fun _x -> _x = 2) |> Array.length

let result = numberOfOnes * numberOfTwos
let total = width * height

let numberOfLayers = layers.Length

let findFirstColorValue(position: int) =
    let layersValues = layers |> Array.map (fun x -> x.[position])
    layersValues |> Array.find (fun v -> v <> 2)
    

let result = seq {
    for idx in [|0..total-1|] do
        yield (findFirstColorValue idx)
}
    for jdx in [|0..numberOfLayers-1|] do
                
    printfn "%s" System.Environment.NewLine

for idx in [|0..numberOfLayers-1|] do
    for jdx in [|0..layers.[idx].Length - 1|] do
        match jdx % width with
        | 0 -> printfn "%s" System.Environment.NewLine
        | _ -> printf "%d" layers.[idx].[jdx]
    printfn "%s" System.Environment.NewLine
    
let resultAsList = result |> Seq.toList
for jdx in [|0..resultAsList.Length|] do
    match jdx % width with
    | 0 -> printf "%s" System.Environment.NewLine
    | _ -> printf "%d" resultAsList.[jdx]
printfn "%s" System.Environment.NewLine