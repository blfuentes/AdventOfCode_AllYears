module day23_part01

open AdventOfCode_2021.Modules
open System.Collections.Generic

type Amphipod = A | B | C | D

type Space = Amphipod option

type State = { Corridor: Space list; Rooms: Space list list; EnergyExpended: int }

let parseAmphipod c =
    match c with
    | 'A' -> Some A
    | 'B' -> Some B
    | 'C' -> Some C
    | 'D' -> Some D
    | _ -> None

let parseInput input =
    input
    |> Seq.skip 2
    |> Seq.take 2
    |> Seq.map (Seq.choose parseAmphipod >> Seq.map Some >> Seq.toList)
    |> Seq.toList
    |> List.transpose

let amphipodMoveEnergy amphipod =
    match amphipod with
    | A -> 1
    | B -> 10
    | C -> 100
    | D -> 1000

let roomPositionInCorridor roomNumber = 2 * (roomNumber + 1)

let countSpaces roomNumber positionInRoom corridorPosition =
    let toCorridor = positionInRoom + 1
    let roomPosition = roomPositionInCorridor roomNumber
    toCorridor + abs (roomPosition - corridorPosition)

let tryGetAmphipodAndRoomPosition (room: Amphipod option list) =
    room
    |> List.indexed
    |> List.tryPick (fun (position, a) ->
        match a with
        | Some a -> Some (position, a)
        | None -> None)

let invalidCorridorPositions = [ 2; 4; 6; 8 ]

let isValidCorridorPosition p =
    invalidCorridorPositions |> List.contains p |> not

let getMovesFromRoom corridor roomNumber room =
    match tryGetAmphipodAndRoomPosition room with
        | None -> room, []
        | Some (roomPosition, amphipod) ->
            let roomCorridorPosition = roomPositionInCorridor roomNumber
            let possibleSpacesLeft =
                corridor
                |> List.indexed
                |> List.take (roomCorridorPosition)
                |> List.rev
                |> List.takeWhile (snd >> Option.isNone)
                |> List.map fst
            let possibleSpacesRight =
                corridor
                |> List.indexed
                |> List.skip roomCorridorPosition
                |> List.takeWhile (snd >> Option.isNone)
                |> List.map fst
            let possibleNewCorridorsAndEnergy =
                possibleSpacesLeft @ possibleSpacesRight
                |> List.filter isValidCorridorPosition
                |> List.map (fun p ->
                    corridor |> List.updateAt p (Some amphipod),
                    (countSpaces roomNumber roomPosition p) * (amphipodMoveEnergy amphipod)
                    )
            let updatedRoom = room |> List.updateAt roomPosition None
            updatedRoom, possibleNewCorridorsAndEnergy

let amphipodToTargetRoomNumber amphipod =
    match amphipod with A -> 0 | B -> 1 | C -> 2 | D -> 3
    
let roomNumberToTargetAmphipod roomNumber =
    match roomNumber with 0 -> A | 1 -> B | 2 -> C | 3 -> D | _ -> failwithf $"Invalid room {roomNumber}"

let isRoomComplete room expectedAmphipod =
    room
    |> List.forall ((=) (Some expectedAmphipod))

let roomShouldBeMovedOutOf room expectedAmphipod =
    room
    |> List.exists (fun space ->
        match space with
        | Some a when a <> expectedAmphipod -> true
        | _ -> false)

let getPossibleMovesIntoCorridor state =
    state.Rooms
    |> List.indexed
    |> List.filter (fun (roomNumber, room) -> (roomShouldBeMovedOutOf room (roomNumberToTargetAmphipod roomNumber)))
    |> List.map (fun (roomNumber, room) ->
        let updatedRoom, possibleNewCorridors = getMovesFromRoom state.Corridor roomNumber room
        let updatedRoomList = state.Rooms |> List.updateAt roomNumber updatedRoom
        possibleNewCorridors
        |> List.map (fun (corridor, energy) ->
            { Corridor = corridor; Rooms = updatedRoomList; EnergyExpended = state.EnergyExpended + energy })
        )
    |> List.concat

let doesRoomContainOnlySelectAmphipodType room amphipod =
    room
    |> List.exists (fun r ->
        match r with
        | Some a when a <> amphipod -> true
        | _ -> false)
    |> not

let canGetToRoom (corridor: Space list) fromPosition roomNumber =
    let roomCorridorPosition = roomPositionInCorridor roomNumber
    let inBetweenSpaces =
        if roomCorridorPosition > fromPosition then
            corridor.[(fromPosition + 1)..roomCorridorPosition]
        else
            corridor.[roomCorridorPosition..(fromPosition - 1)]
    inBetweenSpaces |> List.forall ((=) None)

let moveAmphipodIntoRoom state corridorPosition =
    let amphipod =
        match state.Corridor.[corridorPosition] with
        | Some a -> a
        | None -> failwithf $"No amphipod in position {corridorPosition}"
    let targetRoomNumber = amphipodToTargetRoomNumber amphipod
    if (
        doesRoomContainOnlySelectAmphipodType state.Rooms.[targetRoomNumber] amphipod
        && canGetToRoom state.Corridor corridorPosition targetRoomNumber
        ) then
        let roomPosition =
            tryGetAmphipodAndRoomPosition state.Rooms.[targetRoomNumber]
            |> Option.map fst
            |> Option.defaultValue (state.Rooms.[targetRoomNumber] |> List.length)
            |> fun i -> i - 1
        let updatedRoom = state.Rooms.[targetRoomNumber] |> List.updateAt roomPosition (Some amphipod)
        let energy = (countSpaces targetRoomNumber roomPosition corridorPosition) * (amphipodMoveEnergy amphipod)
        Some { state with
                Rooms = state.Rooms |> List.updateAt targetRoomNumber updatedRoom
                Corridor = state.Corridor |> List.updateAt corridorPosition None
                EnergyExpended = state.EnergyExpended + energy
        }
    else
        None

let rec applyAllMovesIntoRooms state =
    let amphipodPositions =
        state.Corridor
        |> List.indexed
        |> List.choose (fun (i, a) ->
            match a with
            | Some _ -> Some i
            | None -> None)
    match amphipodPositions |> List.tryPick (moveAmphipodIntoRoom state) with
    | Some updatedState -> applyAllMovesIntoRooms updatedState
    | None -> state

let isSolved state =
    state.Rooms
    |> List.indexed
    |> List.forall (fun (i, room) ->
        isRoomComplete room (roomNumberToTargetAmphipod i))

let solve initialState =
    let cache = Dictionary<State, int option>()
    let rec solveStepsCached state =
        match cache.TryGetValue state with
        | true, v -> v
        | false, _ ->
            let v = solveNextSteps state
            cache.Add (state, v)
            v
    and solveNextSteps startingState =
        let stateAfterMovingIntoRooms = applyAllMovesIntoRooms startingState
        if isSolved stateAfterMovingIntoRooms then
            Some stateAfterMovingIntoRooms.EnergyExpended
        else
            let nextPossibleSteps = getPossibleMovesIntoCorridor stateAfterMovingIntoRooms
            match nextPossibleSteps |> List.choose solveStepsCached with
            | [] -> None
            | nextStates -> nextStates |> List.min |> Some
    solveNextSteps initialState

let solveA input =
    let rooms = parseInput input
    let corridor = List.replicate 11 None
    let initialState = {
        Rooms = rooms
        Corridor = corridor
        EnergyExpended = 0
    }
    match solve initialState with
    | Some result -> result
    | None -> failwithf "No result found!"

let execute =
    let path = "day23/day23_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    solveA content