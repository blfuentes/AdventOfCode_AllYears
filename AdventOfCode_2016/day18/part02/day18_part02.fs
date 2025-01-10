module day18_part02

open AdventOfCode_2016.Modules

open System.Collections.Generic

type TileKind =
    | SAFE
    | TRAP

let parseContent (content: string) =
    content.ToCharArray()
    |> Array.map(fun c -> if c = '.' then SAFE else TRAP)

let isTrap((left, center, right): (TileKind*TileKind*TileKind)) =
    match (left, center, right) with
    | (l, c, r) when l.IsTRAP && c.IsTRAP && r.IsSAFE -> true
    | (l, c, r) when l.IsSAFE && c.IsTRAP && r.IsTRAP -> true
    | (l, c, r) when l.IsTRAP && c.IsSAFE && r.IsSAFE -> true
    | (l, c, r) when l.IsSAFE && c.IsSAFE && r.IsTRAP -> true
    | _ -> false

let nextState (floor: TileKind array) =
    let floorchecker = Array.concat[[|SAFE|]; floor; [|SAFE|]]
    floor
    |> Array.mapi(fun idx _ -> 
        if isTrap (floorchecker[idx-1+1], floorchecker[idx+1], floorchecker[idx+1+1]) then TRAP else SAFE 
    )

let printState(floor: TileKind array) =
     String.concat "" (floor |> Array.map(fun t -> if t.IsSAFE then "." else "^"))

let countSafes(floor: TileKind array) =
    floor |> Array.filter _.IsSAFE |> Array.length

let walk (floor: TileKind array) (rows: int) (memo: Dictionary<TileKind array, TileKind array>)=
    let mutable currentRow = 1
    let mutable currentCount = 0
    let mutable newstate = floor
    currentCount <- currentCount + countSafes newstate
    
    while currentRow < rows do
        newstate <-
            if memo.ContainsKey(newstate) then
                memo[newstate]
            else
                let t' = nextState newstate
                memo.Add(newstate, t')
                t'
        currentCount <- currentCount + countSafes newstate
        currentRow <- currentRow + 1

    currentCount   

let execute =
    let path = "day18/day18_input.txt"
    let content = LocalHelper.GetContentFromFile path

    let floor = parseContent content
    let mapSize = 400000
    let memo = Dictionary<TileKind array, TileKind array>()

    walk floor mapSize memo