open System
open System.IO
open System.Text.RegularExpressions

let thelist = [1; 2; 3; 0; 2; 4; 0; 5; 6; 0; 7]

let calculateBinarySeat (seatdefinition: string) =
    //let xxx = String(seatdefinition.Replace('F', '0').Replace('B', '1').Replace('L', '0').Replace('R', '1').ToCharArray() |> Array.rev)
    //let newSeatDefinition = String(seatdefinition.Replace('F', '0').Replace('B', '1').Replace('L', '0').Replace('R', '1').ToCharArray() |> Array.rev)
    Convert.ToInt32(String(seatdefinition.Replace('F', '0').Replace('B', '1').Replace('L', '0').Replace('R', '1').ToCharArray()), 2)

calculateBinarySeat "FBFBBFFRLR"
calculateBinarySeat "BFFFBBFRRR"
calculateBinarySeat "FFFBBBFRRR"
calculateBinarySeat "BBFFBBFRLL"

let folder (a) (cur, acc) = 
    match a with
    | _ when a <> 0 -> a::cur, acc
    | _ -> [], cur::acc

let split lst =
    let result = List.foldBack folder (lst) ([], [])
    (fst result)::(snd result)

printfn "%A" (split thelist)

List.fold (fun acc x -> x :: acc) [] thelist
List.foldBack (fun x acc -> x :: acc) thelist []


let split2 lst =
    let folder (a, b) (cur, acc) = 
        match a with
        | _ when a < b -> a::cur, acc
        | _ -> [a], cur::acc

    let result = List.foldBack folder (List.pairwise lst) ([List.last lst], []) 
    (fst result)::(snd result)

printfn "%A" (split2 thelist)


let list = [1; 2; 3; 0; 2; 4; 0; 0; 5; 6; 0; 7]

list
|> Seq.fold (fun state number ->
    match number with
    | 0 -> []::state
    | x -> 
        match state with
        | [] -> [] // never here, actually
        | h::t -> [x::h]@t
    )
    [[]]
|> List.map List.rev
|> List.rev
|> List.filter (List.isEmpty >> not)