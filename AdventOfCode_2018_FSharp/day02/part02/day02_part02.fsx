open System.IO

let filepath = __SOURCE_DIRECTORY__ + @"../../day02_input.txt"
let filepath = __SOURCE_DIRECTORY__ + @"../../test01.txt"
let lines = File.ReadLines(filepath)

let DiffStrings (s1 : string) (s2 : string) =
   let s1', s2' = s1.PadRight(s2.Length), s2.PadRight(s1.Length)
   let distance = 
      (s1', s2')
      ||> Seq.zip
      |> Seq.map (fun (c1, c2) -> c1 = c2)
      |> Seq.filter (fun _c -> not _c) |> Seq.length
   distance

let DiffStrings2 (s1 : string) (s2 : string) =
   let s1', s2' = s1.PadRight(s2.Length), s2.PadRight(s1.Length)
   let matchedString = 
      (s1', s2')
      ||> Seq.zip
      |> Seq.map (fun (c1, c2) -> if c1 = c2 then c1 else ' ')
      |> Seq.filter (fun _c -> _c <> ' ') 
   matchedString |> List.ofSeq

let findSimilarString element inputList =
    let output = inputList |> Seq.filter (fun e -> e <> element) |> Seq.tryFind (fun e -> DiffStrings element e = 1) |> Option.map (fun e -> e) |> Option.defaultValue ""
    output

let findSimilarString2 element inputList =
    let output = 
        inputList 
        |> Seq.filter (fun e -> e <> element) 
        |> Seq.tryFind (fun e -> (DiffStrings2 element e).Length = (e.Length - 1)) 
        |> Option.map (fun e -> DiffStrings2 e element) |> Option.defaultValue List.Empty
    output

let resolve inputList : string =
    let mystring = inputList |> Seq.ofList |> Seq.find (fun (e) -> findSimilarString e inputList <> "")
    mystring

let resolve2 inputList =
    let mystring = 
        inputList |> Seq.ofList |> Seq.map (fun (e) -> 
            let foundResult = findSimilarString2 e inputList
            foundResult
        )
    mystring |> Seq.find (fun x -> x.Length > 0) |> List.map (fun x -> string x) |> List.fold(+) ""

DiffStrings2 "abcde" "axcye"
DiffStrings2 "fghij" "fguij"

printfn "solution %s" (findSimilarString2 "abcde" (lines |> List.ofSeq))

printfn "solution %A" (findSimilarString2 "fghij" (lines |> List.ofSeq))

printfn "solution %A" (resolve2 (lines |> List.ofSeq))

printfn "solution %i" (DiffStrings "abcde" "axcye")
printfn "solution %i" (DiffStrings "fghij" "fguij")


let findSimilarString2 element inputList : string =
    let output =  inputList |> Seq.filter (fun (e) -> e <> element) |> Seq.find (fun (e) -> DiffStrings element e = 1)
    output

let findSimilarString2 (element:string, inputList:List<string>) : string =
    let output = inputList |> Seq.filter (fun (e) -> e <> element) |> Seq.head
    output

let findSimilarString (element:string, inputList:List<string>) =
    let output = inputList |> Seq.find (fun (e) -> e <> element && DiffStrings element e = 1)
    output.Length, output

let findSimilarString3 (element:string, inputList:List<string>) =
    let output = inputList |> Seq.head
    output





//let result = (inputList:List<string>) : string =
//    let mystring = inputList |> Seq.ofList |> Seq.find (fun (e) -> findSimilarString e inputList match _,_)
//    mystring

let result = findSimilarString3 "abcde" lines


let distance = DiffStrings "abcde" "axcye"






findSimilarString 

type Counter(value: string) =
    let description = value
    member this.numberOfAppearances = 0

type Entry(value: string) =
    let entryValue = value
    member this.description = Array.empty<Counter>
    member this.candidatesTwo = Array.empty<Counter>
    member this.candidatesThree = Array.empty<Counter>

let mutable counterTwo = 0
let mutable counterThree = 0
let mutable other = 0

let teststring = "aabbcccdef"
//let result =  
//    teststring |> Seq.countBy id 
//        |> List.ofSeq |> List.iter (fun (index, value) -> 
//                match (index, value) with
//                | (_, 2) -> counterTwo <- counterTwo + 1
//                | (_, 3) -> counterThree <- counterThree + 1
//                | (_, _) -> other <- other + 1)
//    counterTwo * counterThree
let teststring2 = "aabbccdef"
counterTwo <- 0
counterThree <- 0
let result =  
    teststring2 |> Seq.countBy id 
        |> List.ofSeq |> List.iter (fun (index, value) -> 
                match (index, value) with
                | (_, 2) -> counterTwo <- counterTwo + 1
                | (_, 3) -> counterThree <- counterThree + 1
                | (_, _) -> other <- other + 1)
    (counterTwo > 0 , counterThree > 0)

let getTupleComposition(input: string) =  
    let counted = input |> Seq.countBy id 
    let existsTwo = counted |> Seq.exists (fun (x, y) -> y = 2)
    let existsThree = counted |> Seq.exists (fun (x, y) -> y = 3)
    let tupleresult = (existsTwo, existsThree)
    tupleresult

let finalValue =
    let tmplist = lines |> Seq.map (fun line -> getTupleComposition line) |> List.ofSeq
    let numberOfTwos = tmplist |> List.filter (fun (two, three) -> two = true) |> List.length
    let numberOfThrees = tmplist |> List.filter (fun (two, three) -> three = true) |> List.length
    numberOfTwos * numberOfThrees


let input = "aabbccdef"  
getTupleComposition input
let teststring2 = "aabbccdef"
let counted = teststring2 |> Seq.countBy id
let existsTwo = counted |> Seq.exists (fun (x, y) -> y = 2)
let existsThree = counted |> Seq.exists (fun (x, y) -> y = 3)

//let entries = 
//    lines |> Seq.iter (Seq.iter (fun character -> printfn "%c" character)); 

let entries = 
    lines |> Seq.iter (fun line -> 
        line |> Seq.countBy id |> List.ofSeq |> List.iter (fun (index, value) ->
            match (index, value) with
            | (_, 2) -> counterTwo <- counterTwo + 1
            | (_, 3) -> counterThree <- counterThree + 1
            | (_, _) -> other <- other + 1)
    )
    counterTwo * counterThree