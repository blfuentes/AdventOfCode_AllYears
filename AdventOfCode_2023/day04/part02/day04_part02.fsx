open AdventOfCode_2023.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

type CardSet = {
    CardId: int
    WinningCards: int array
    MyCards: int array
    IsOriginal: bool
}

//let path = "day04/test_input_01.txt"
let path = "day04/day04_input.txt"

let lines = GetLinesFromFile path

let buildCardSet (input: string) =
    let parts = input.Split(':')
    let cardId = Regex.Match(parts.[0], @"\d+").Value |> int
    let winningCards = Regex.Matches(parts.[1].Split('|').[0], @"\d+") |> Seq.map _.Value |> Seq.map int |> Seq.distinct |> Seq.toArray
    let myCards = Regex.Matches(parts.[1].Split('|').[1], @"\d+") |> Seq.map _.Value |> Seq.map int |> Seq.distinct |> Seq.toArray
    { CardId = cardId; MyCards = myCards; WinningCards = winningCards; IsOriginal = true}

let countMatchingCards (cardSet: CardSet) =
    cardSet.MyCards |> Seq.filter (fun x -> cardSet.WinningCards |> Seq.contains x) |> Seq.length

let returnMatchingCards (cardSet: CardSet) (originalCardSet: CardSet list) =
    let matchingCards = countMatchingCards cardSet
    let copyCardsIds = if matchingCards > 0 then [cardSet.CardId + 1 .. cardSet.CardId + matchingCards] else []
    let copyCards = originalCardSet |> Seq.filter (fun x -> copyCardsIds |> Seq.contains x.CardId) 
                    |> Seq.distinctBy _.CardId |> Seq.toList
    copyCards

let buildMemo (originalCardSet: CardSet list) =
    originalCardSet |> List.map (fun c -> (c.CardId, returnMatchingCards c originalCardSet)) |> Array.ofList

let cardSet (input: string array) =
    input |> Seq.map buildCardSet |> Seq.toList

let rec playCardSet (initialCards: int array) (remainingCards: CardSet list) (memo: (int * CardSet list) array)=
    match remainingCards with
    | [] -> initialCards
    | card :: tail ->
        let copyCards = memo.[card.CardId - 1] |> snd
        let numOfCurrentCards = initialCards.[card.CardId - 1]
        for copy in copyCards do
            initialCards.[copy.CardId - 1] <- initialCards.[copy.CardId - 1] + numOfCurrentCards
        playCardSet initialCards tail memo

let memo = buildMemo (cardSet lines)
let initialCards = Array.create lines.Length 1
let ended = playCardSet initialCards (cardSet lines) memo
ended |> Array.sum