open System.IO

let filepath = __SOURCE_DIRECTORY__ + @"../../day02_input.txt"
let filepath = __SOURCE_DIRECTORY__ + @"../../test01.txt"
let lines = File.ReadLines(filepath)

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