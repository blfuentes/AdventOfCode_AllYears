module day17_part01

open AdventOfCode_2022.Modules
open AdventOfCode_Utilities

open AdventOfCode_2022.Modules
open AdventOfCode_Utilities
open System
open System.Collections.Generic
open FSharp.Collections.ParallelSeq

open AdventOfCode_2022.Modules
open AdventOfCode_Utilities
open System
open System.Collections.Generic
open FSharp.Collections.ParallelSeq

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

let totalHeight jets =
    let state = stateDefaultActual jets
    (infiniteStates state |> Seq.find (fun s -> s.RestedFiguresCount = 2022L)).TowerHeight

let execute =
    let path = "day17/day17_input.txt"
    let jets = (LocalHelper.GetContentFromFile path).ToCharArray()
    totalHeight jets