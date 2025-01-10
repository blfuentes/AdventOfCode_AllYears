open System
open System.IO
open System.Text.RegularExpressions

let path = "day09_input.txt"
//let path = "test_input.txt"
//let path = "test_input_00.txt"

let inputPartsCollection = 
    File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.map(fun l -> l.ToCharArray() |> Array.map string |> Array.map int) |> Seq.toArray

let columns = inputPartsCollection.[0].Length
let rows = inputPartsCollection.Length

let topbottomArray = [|Array.init(columns + 2)(fun i -> 9)|]
let adaptedinput = inputPartsCollection |> Array.map(fun r -> Array.concat[[|9|]; r; [|9|]])
let paddedArray = Array.concat([topbottomArray; adaptedinput; topbottomArray])

let valuesArray = Array2D.zeroCreate<int> (rows) (columns)

[1 .. rows] 
|> Seq.iter(fun i -> [1 .. columns] |> Seq.iter(fun j -> valuesArray[i - 1, j - 1] <- -1)    
)

let isSmallest(row: int, col: int, values:int[][]) =
    let valueToCheck = values.[row].[col]
    let neighboors = [values.[row - 1].[col]; values.[row].[col - 1]; values.[row].[col + 1]; values.[row + 1].[col]]
    neighboors |> List.forall(fun n -> valueToCheck < n)

[1 .. rows] 
|> Seq.iter(fun i -> [1 .. columns] |> Seq.iter(fun j ->  if isSmallest(i, j, paddedArray) then valuesArray[i - 1, j - 1] <- paddedArray.[i].[j])    
)
let elements = valuesArray |> Seq.cast<int> |> Seq.filter (fun x -> x <> -1) |> Seq.toArray

let result = (elements  |> Seq.sum) + elements.Length
result