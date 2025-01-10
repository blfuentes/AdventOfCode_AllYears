module day04_part01

open System
open System.Text.RegularExpressions

open AdventOfCode_2023.Modules.LocalHelper

let path = "day04/day04_input.txt"

type CardSet = {
    CardId: int
    MyCards: int array
    WinningCards: int array
}

let buildCardSet (input: string) =
    let parts = input.Split(':')
    let cardId = Regex.Match(parts.[0], @"\d+").Value |> int
    let myCards = Regex.Matches(parts.[1].Split('|').[0], @"\d+") |> Seq.map _.Value |> Seq.map int |> Seq.toArray
    let winningCards = Regex.Matches(parts.[1].Split('|').[1], @"\d+") |> Seq.map _.Value |> Seq.map int |> Seq.toArray
    { CardId = cardId; MyCards = myCards; WinningCards = winningCards}

let countMatchingCards (cardSet: CardSet) =
    cardSet.MyCards |> Seq.filter (fun x -> cardSet.WinningCards |> Seq.contains x) |> Seq.length

let getPoints (input: int) =
    match input with
    | v when v > 0 -> Math.Pow(2.0, float (v - 1)) |> int
    | _ -> 0

let cardSet (input: string array) =
    let cardSets = input |> Seq.map buildCardSet |> Seq.toArray
    cardSets |> Seq.map countMatchingCards

let execute =
    let lines = GetLinesFromFile path
    let winningsBySet = cardSet lines
    winningsBySet |> Seq.map getPoints |> Seq.sum