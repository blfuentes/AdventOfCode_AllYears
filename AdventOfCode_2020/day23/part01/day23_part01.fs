module day23_part01

open AdventOfCode_Utilities
open AdventOfCode_2020.Modules

let findDestinationCup cupsToSearch currentCupValue =
    let indexedCups = cupsToSearch |> List.indexed
    match indexedCups |> List.filter (fun (_, c) -> c < currentCupValue) with
    | [] -> indexedCups |> List.maxBy snd
    | smallerValueCups -> smallerValueCups |> List.maxBy snd

let applyMove cups =
    let currentCup = cups |> List.head
    let cupsToMove = cups |> List.tail |> List.take 3
    let remainingCups = cups |> List.skip 4
    let destinationIndex, _ = findDestinationCup remainingCups currentCup
    remainingCups.[..destinationIndex] @ cupsToMove @ remainingCups.[(destinationIndex+1)..] @ [currentCup]
  
let applyXMoves numberMoves cups =
    List.init numberMoves id
    |> List.fold (fun previousState _ -> applyMove previousState) cups

let findlabels numbers =
    let finalResult = applyXMoves 100 numbers
    let indexOf1 = finalResult |> List.findIndex (fun cup -> cup = 1)
    (finalResult |> List.skip (indexOf1 + 1)) @ (finalResult |> List.take indexOf1)
    |> List.map string
    |> joinStrings

let execute =
    let path = "day23/day23_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let numbers = content.ToCharArray() |> Array.map (string >> int) |> List.ofArray
    findlabels numbers
