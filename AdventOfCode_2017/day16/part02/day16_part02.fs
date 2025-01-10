module day16_part02

open AdventOfCode_2017.Modules
open System.Collections.Generic

type MoveKind =
    | Spin of steps: int
    | Exchange of placeA: int * placeB: int
    | Partner of placeA: string * placeB: string

let parseContent(input: string) =
    let instructions = input.Split(",")
    instructions
    |> Array.map(fun ins ->
        match ins[0] with
        | 's' ->
            Spin((int)(ins.Substring(1)))
        | 'x' ->
            let a = (int)(ins.Substring(1).Split("/")[0])
            let b = (int)(ins.Substring(1).Split("/")[1])

            Exchange(a, b)
        | 'p' ->
            let a = ins.Substring(1).Split("/")[0]
            let b = ins.Substring(1).Split("/")[1]

            Partner(a, b)
        | _ -> failwith "error"
    )

let performMove(op: MoveKind) (state: string array) =
    match op with
    | Spin(steps) ->
        let initpart = Array.sub state (state.Length - steps) steps
        let endpart = Array.sub state 0 (state.Length - steps)
        Array.concat [|initpart; endpart|]
    | Exchange(placeA, placeB) ->
        state
        |> Array.mapi(fun i v ->
            if i = placeA then
                state[placeB]
            elif i = placeB then
                state[placeA]
            else
                v
        )
    | Partner(placeA, placeB) ->
        state
        |> Array.map(fun v ->
            if v = placeA then
                placeB
            elif v = placeB then
                placeA
            else
                v
        )

let rec performDance(dance: MoveKind list) (state: string array) =
    match dance with
    | [] -> String.concat "" state
    | move :: remaining ->
        let newstate = performMove move state
        performDance remaining newstate

let generateSequenceOfDances(dance: MoveKind list) (state: string) (target: int)=
    let memostates = HashSet<string>()
    let _ = memostates.Add(state)
    let mutable currentDance = 0
    let mutable repeated = false
    let mutable newstate = state
    while not repeated do
        newstate <- performDance dance (newstate.ToCharArray() |> Array.map string)
        if memostates.Contains(newstate) then
            repeated <- true
        else
            let _ = memostates.Add(newstate)
            currentDance <- currentDance + 1
    
    let reducedIdx = target % memostates.Count
    let value = (memostates |> List.ofSeq).Item(reducedIdx)
    value

let execute() =
    let path = "day16/day16_input.txt"
    let content = LocalHelper.GetContentFromFile path

    let order = "abcdefghijklmnop"
    let numOfDances = 1000000000

    let dance = parseContent content |> List.ofArray
    generateSequenceOfDances dance order numOfDances