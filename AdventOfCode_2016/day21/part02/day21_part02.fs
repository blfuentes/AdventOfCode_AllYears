module day21_part02

open AdventOfCode_2016.Modules
open AdventOfCode_Utilities

type DirKind =
    | Left
    | Right

type OpKind =
    | SwapPosition of position1: int * position2: int
    | SwapLetter of letter1: string * letter2: string
    | Reverse of position: int * through: int
    | Rotate of dir: DirKind * steps: int
    | RotateBased of letter: string
    | Move of position1: int * position2: int

let parseContent(lines: string array) =
    lines
    |> Array.map(fun l ->
        let parts = l.Split(" ")
        match parts[0] with
        | "swap" when parts[1] = "position" ->
            SwapPosition((int)(parts[2]), (int)(parts[5]))
        | "swap" when parts[1] = "letter" ->
            SwapLetter(parts[2], parts[5])
        | "reverse" ->
            Reverse((int)(parts[2]), (int)(parts[4]))
        | "rotate" when parts[1] = "left" || parts[1] = "right" ->
            let dir = if parts[1] = "left" then Left else Right
            Rotate(dir, (int)(parts[2]))
        | "rotate" when parts[1] = "based" ->
            RotateBased(parts[6])
        | "move" ->
            Move((int)(parts[2]), (int)(parts[5]))
        | _ -> failwith "error"
    )

let performOp(op: OpKind)(state: string array) =
    match op with
    | SwapPosition(pos1, pos2) ->
        let tmp = state[pos1]
        state[pos1]<-state[pos2]
        state[pos2]<-tmp
        
        state
    | SwapLetter(letter1, letter2) ->
        state |> Array.iteri(fun i s ->
            if s = letter1 then state[i] <- letter2
            elif s = letter2 then state[i] <- letter1
        )

        state
    | Reverse(pos, th) ->
        let initpart = state |> Array.take(pos)
        let midpart = Array.sub state pos ((th - pos) + 1) |> Array.rev
        let endpart = state |> Array.skip(th + 1)

        Array.concat [|initpart; midpart; endpart|]
    | Rotate(dir, steps) ->
        if dir.IsRight then
            let rotatedpart = Array.sub state 0 (state.Length - steps)
            let initpart = Array.sub state (state.Length - steps) steps
            
            Array.concat [| initpart; rotatedpart |]
        else
            let rotatedpart = Array.sub state 0 steps
            let initpart = Array.sub state steps (state.Length - steps)
            
            Array.concat [| initpart; rotatedpart |]
    | RotateBased(letter) ->
        let index = state |> Array.findIndex(fun s -> s = letter)
        let steps = if index < 4 then 1 + index else 1 + index + 1

        let rotatedpart = Array.sub state 0 (state.Length - (steps % state.Length))
        let initpart = Array.sub state (state.Length - (steps % state.Length)) (steps % state.Length)

        Array.concat [| initpart; rotatedpart |]
    | Move(position1, position2) ->
        let element = state[position1]

        let withoutX = 
            state 
            |> Array.mapi (fun i el -> if i <> position1 then Some el else None) 
            |> Array.choose id

        let result =
            if position2 < withoutX.Length then
                Array.concat [|withoutX[0..position2-1]; [|element|]; withoutX[position2..] |]
            else
                Array.concat[|withoutX; [|element|] |]

        result

let runOperations(start: string)(ops: OpKind list) =
    let rec runOps(state: string array) (remaining: OpKind list) =
        match remaining with
        | [] -> String.concat "" state
        | op :: rest ->
            let newstate = performOp op state
            runOps newstate rest

    runOps (start.ToCharArray() |> Array.map string) ops

let findUnscrambled(start: string) (expected: string) (ops: OpKind list) =
    let allpossible = Utilities.permutations (start.ToCharArray() |> List.ofArray)
    let found =
        allpossible
        |> List.find(fun s ->
            let s' = String.concat "" (s |> List.map string)
            let result = runOperations s' ops
            result = expected
        )
    Some(String.concat "" (found |> List.map string))
 
let execute =
    let path = "day21/day21_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let start = "abcdefgh"
    let expected = "fbgdceah"

    let operations = parseContent content |> List.ofArray
    findUnscrambled start expected operations