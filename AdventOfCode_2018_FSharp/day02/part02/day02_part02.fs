module day02_part02

open System.IO

let filepath = __SOURCE_DIRECTORY__ + @"../../day02_input.txt"
let lines = File.ReadLines(filepath)

let DiffStrings (s1 : string) (s2 : string) =
   let s1', s2' = s1.PadRight(s2.Length), s2.PadRight(s1.Length)
   let matchedString = 
      (s1', s2')
      ||> Seq.zip
      |> Seq.map (fun (c1, c2) -> if c1 = c2 then c1 else ' ')
      |> Seq.filter (fun _c -> _c <> ' ') 
   matchedString |> List.ofSeq

let findSimilarString element inputList =
    let output = 
        inputList 
        |> Seq.filter (fun e -> e <> element) 
        |> Seq.tryFind (fun e -> (DiffStrings element e).Length = (e.Length - 1)) 
        |> Option.map (fun e -> DiffStrings e element) |> Option.defaultValue List.Empty
    output

let getSolution inputList =
    let mystring = 
        inputList |> Seq.ofList |> Seq.map (fun (e) -> 
            let foundResult = findSimilarString e inputList
            foundResult
        )
    mystring |> Seq.find (fun x -> x.Length > 0) |> List.map (fun x -> string x) |> List.fold(+) ""

let resolve = getSolution (lines |> List.ofSeq)