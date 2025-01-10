module day17_part02

open AdventOfCode_2022.Modules
open System.Collections.Generic

type JetsState = { Pattern: char array; Index: int }
with member this.Next () = 
        { this with Index = (this.Index + 1) % this.Pattern.Length }
     member this.Current = this.Pattern[this.Index]

type Figure = uint8 list

let inline (>>) figure shift = List.map (fun y -> y >>> shift) figure
let inline (<<) figure shift = List.map (fun y -> y <<< shift) figure

let figures = [|
    [ 
        0b0011110uy
    ]
    [
        0b0001000uy
        0b0011100uy
        0b0001000uy
    ]
    [
        0b0000100uy
        0b0000100uy
        0b0011100uy
    ]
    [
        0b0010000uy
        0b0010000uy
        0b0010000uy
        0b0010000uy
    ]
    [
        0b0011000uy
        0b0011000uy
    ]
|] 


type FiguresState = { Figures: Figure array; Index : int }
with member this.Next() = 
        { this with Index = (this.Index + 1) % this.Figures.Length }
     member this.Current = this.Figures[this.Index]

type FallingState = { Figure : Figure; Position: int }

type State = { 
    RestedFiguresCount: int64
    TowerHeight: int64
    Rested: Figure; 
    Figures: FiguresState
    Jets: JetsState;
    Falling: FallingState
}

let jet (state : State) = 
    let { Falling = falling } = state
    let newFalling = 
        match state.Jets.Current with
        | '>' -> 
            if (List.exists (fun x -> x &&& 0b0000001uy <> 0uy) falling.Figure)
            then falling
            else {falling with Figure = falling.Figure >> 1}
        | '<' -> 
            if (List.exists (fun x -> x &&& 0b1000000uy <> 0uy) falling.Figure)
            then falling
            else {falling with Figure = falling.Figure << 1}
    { state with Falling = newFalling }

let gravity (state : State) = 
    { state with Falling = { state.Falling with Position = state.Falling.Position + 1} }

let private revertIfCollision (state : State) (requestedState : State) = 
    let { Falling = { Figure = figure; Position = pos } } = requestedState
    let figureHeight = List.length figure
    if (List.length state.Rested - pos < figureHeight)
    then state
    else 
        let collision = 
            state.Rested |> List.skip pos |> List.take figureHeight
            |> List.zip figure
            |> List.map (fun (x,y) -> x &&& y)
            |> List.exists ((<>)0uy)
        if collision then state
        else requestedState

let checkCollision (f : State -> State) = 
    fun state ->
        let newState = f state
        revertIfCollision state newState
let fix state =
    let { Falling = { Figure = figure; Position = pos } } = state
    let figureHeight = List.length figure
    let (before, rest1) = List.splitAt pos state.Rested
    let (atFalling, rest) = List.splitAt figureHeight rest1

    let newRested = 
        atFalling
        |> List.zip figure
        |> List.map (fun (x,y) -> x ||| y)

    let heightIncrease = 
        atFalling |> List.filter ((=)0uy) |> List.length

    {
        state with Rested = before @ newRested @ rest; 
                TowerHeight = state.TowerHeight + int64 heightIncrease;
                RestedFiguresCount = state.RestedFiguresCount + 1L; 
    }

let newFigure s = 
    let figures = s.Figures.Next();
    
    let newRested = 
        s.Rested
        |> List.skipWhile ((=)0uy)
        |> List.append (List.replicate (figures.Current.Length + 3) 0uy)
    { s with Rested = newRested; Figures = figures; Falling = { Figure = figures.Current; Position = 0 }}

let nextState state = 
    let afterJet = state |> checkCollision jet
    let afterGravity = afterJet |> checkCollision gravity
    
    if (afterJet.Falling = afterGravity.Falling) 
    then 
        { (afterJet |> fix |> newFigure) with Jets = state.Jets.Next(); }
    else
        { afterGravity with Jets = state.Jets.Next()}

let infiniteStates initial =
    Seq.unfold (fun s -> let newS = nextState s in Some(newS, newS)) initial

let stateDefaultActual jets = 
    {
        Rested = [
            0uy
            0uy
            0uy
            0uy
        ]
        RestedFiguresCount = 0
        TowerHeight = 0
        Figures = { Figures = figures; Index = 0}
        Jets = { Pattern = jets; Index = 0}
        Falling = { Figure = figures[0]; Position = 0 }
    }

type RepetitionState = { Rested: Figure; JetsIndex: int; FiguresIndex: int}

let private repetitionsState (state : State) = 
    { Rested = state.Rested |> List.truncate 50; JetsIndex = state.Jets.Index; FiguresIndex = state.Figures.Index }

type Repetition = { Start: int; StartState: State; Period: int; NextState: State}

let findRepetition (states: seq<State>) = 
    let cache = Dictionary<_, _>()
    states
    |> Seq.distinctBy (fun s -> s.RestedFiguresCount)
    |> Seq.choose (fun state -> 
        let key = repetitionsState state
        match cache.TryGetValue(key) with
        | false, _ -> 
            cache.Add(key, state)
            None
        | true, value -> 
            Some { Start = int value.RestedFiguresCount; StartState = value; Period = int state.RestedFiguresCount - int value.RestedFiguresCount; NextState = state }
    ) |> Seq.head

let stateAt initState repetitionState restedFiguresCount = 
    if (restedFiguresCount <= repetitionState.NextState.RestedFiguresCount)
    then 
        infiniteStates initState
        |> Seq.find (fun p -> p.RestedFiguresCount = restedFiguresCount)
    else 
        let heightInPeriod = repetitionState.NextState.TowerHeight - repetitionState.StartState.TowerHeight
        let periods = (restedFiguresCount - repetitionState.StartState.RestedFiguresCount) / (int64 repetitionState.Period)
        let s = {
            repetitionState.StartState with 
                RestedFiguresCount = periods * (int64 repetitionState.Period) + repetitionState.StartState.RestedFiguresCount
                TowerHeight = periods * heightInPeriod + repetitionState.StartState.TowerHeight
        }
        infiniteStates s
        |> Seq.find (fun p -> p.RestedFiguresCount = restedFiguresCount)

//stateAt stateDefaultSample (infiniteStates stateDefaultSample |> findRepetition) 1000000000000L

let execute =
    let path = "day17/day17_input.txt"
    let jets = (LocalHelper.GetContentFromFile path).ToCharArray()
    let state = stateDefaultActual jets
    (stateAt state (infiniteStates state |> findRepetition) 1000000000000L).TowerHeight
    