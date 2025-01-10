module day20_part01

open AdventOfCode_2018.Modules

type Direction = North | South | East | West
type Term = Start | End | Move of Direction | StartGroup | EndGroup | Alternative
let lexTerm = function
    | '^' -> Start | '$' -> End
    | 'N' -> Move North | 'S' -> Move South | 'E' -> Move East | 'W' -> Move West
    | '(' -> StartGroup | ')' -> EndGroup | '|' -> Alternative
    | c -> failwithf "Unexpected term %c" c

let parseRegex (input: string) 
    = input.ToCharArray() |> Seq.map lexTerm

type Position = int * int
let moveDir (x, y) = function
    | North -> (x, y - 1)
    | South -> (x, y + 1)
    | East -> (x + 1, y)
    | West -> (x - 1, y)

type ParseState = {posWithDist: Position * int; dists: Map<int * int, int>; posStack: (Position * int) list}
let updateDists (pos, dist) dists =
    match Map.tryFind pos dists with
    | Some d when d < dist -> dists
    | _ -> Map.add pos dist dists

let parseMove {posWithDist=(pos, dist); dists=dists; posStack=posStack} dir =
    let newPosWithDist = moveDir pos dir, dist + 1
    let dists = updateDists newPosWithDist dists
    {posWithDist=newPosWithDist; dists=dists; posStack=posStack}

let parseTerm state = function
    | Start | End -> state
    | Move dir -> parseMove state dir
    | StartGroup -> {state with posStack=state.posWithDist::state.posStack}
    | EndGroup -> {state with posStack=List.tail state.posStack}
    | Alternative -> {state with posWithDist=List.head state.posStack}

let largestNumber regex =
    let initState = {posWithDist=((0, 0), 0); dists=Map.empty; posStack=[]}
    let finalState = Seq.fold parseTerm initState regex
    finalState.dists |> Map.toArray |> Array.map snd |> Array.max

let execute =
    let path = "day20/day20_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let terms = parseRegex content
    largestNumber terms