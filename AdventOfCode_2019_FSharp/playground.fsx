// 4a
let strToList = Seq.toList

let rec hasAdjacentChars l =
    match l with
    | x::y::_ when x = y -> true
    | _::rest -> hasAdjacentChars rest
    | _ -> false

let rec ascendingChars l =
    match l with
    | x::y::rest when x <= y -> ascendingChars (y::rest)
    | x::y::_ when x > y -> false
    | _ -> true

// 4b
let rec atLeastOneGroupExactlyTwo l =
    l
    |> List.countBy (fun c -> c)
    |> List.exists (fun (_, n) -> n = 2)

//let tmp = sprintf "%i" 123444 |> strToList
//tmp |> List.countBy (fun c -> c)


let tmp = sprintf "%i" 111233 |> strToList
tmp |> List.countBy (fun c -> c)

let rules = [
    hasAdjacentChars
    ascendingChars
    atLeastOneGroupExactlyTwo
    ]

let isValid l =
    [ for rule in rules -> rule l ]
    |> List.forall id

let res =
    let lower = 273025
    let upper = 767253
    let nums = [ lower..upper ] |> List.map (sprintf "%i") |> List.map strToList

    nums
    |> List.filter isValid
    |> List.length
