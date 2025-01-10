open System.IO

let filepath = __SOURCE_DIRECTORY__ + @"../../day01_part01_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test02.txt"
let lines = File.ReadLines(filepath)

let mutable duplicated = false

let mutable calculatedFrecuencies = Set.empty<int>
let mutable value = 0

let calculateDuplicatedFrecuency = 
    while not duplicated do
        lines
            |> Seq.map int
            |> Seq.takeWhile (fun _ -> not duplicated)
            |> Seq.iter (fun element -> 
                value <- value + element
                if calculatedFrecuencies.Contains value then
                    duplicated <- true 
                else 
                    calculatedFrecuencies <- calculatedFrecuencies.Add(value)
            )
    value

printfn "duplicated frecuency: %i" value