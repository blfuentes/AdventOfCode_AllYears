module day04_part02

open System.Text.RegularExpressions

open AdventOfCode_2023.Modules.LocalHelper

let path = "day04/day04_input.txt"

type CardSet = {
    CardId: int
    WinningCards: int Set
    MyCards: int Set
}

let buildCardSet (input: string) =
    let parts = input.Split(':')
    let cardId = Regex.Match(parts.[0], @"\d+").Value |> int
    let winningCards = Regex.Matches(parts.[1].Split('|').[0], @"\d+") |> Seq.map _.Value |> Seq.map int |> Set.ofSeq
    let myCards = Regex.Matches(parts.[1].Split('|').[1], @"\d+") |> Seq.map _.Value |> Seq.map int |> Set.ofSeq
    { CardId = cardId; MyCards = myCards; WinningCards = winningCards }

let getMatchingCards (cardSet: CardSet) =
    Set.intersect cardSet.WinningCards cardSet.MyCards

let returnMatchingCards (cardSet: CardSet) (originalCardSet: CardSet list) =
    let matchingCards = (getMatchingCards cardSet).Count
    let copyCardsIds = if matchingCards > 0 then [cardSet.CardId + 1 .. cardSet.CardId + matchingCards] else []
    let copyCards = originalCardSet |> Seq.filter (fun x -> copyCardsIds |> Seq.contains x.CardId) |> Seq.toList
    copyCards

let cardSet (input: string array) =
    input |> Seq.map buildCardSet |> Seq.toList

let rec playCardSet (initialCards: int array) (remainingCards: CardSet list)=
    match remainingCards with
    | [] -> initialCards |> Array.sum
    | card :: tail ->
        let copyCards = returnMatchingCards card remainingCards
        let numOfCurrentCards = initialCards.[card.CardId - 1]
        let newCards =
            seq {
                for idx in 0..initialCards.Length - 1 do
                    if (copyCards |> List.exists (fun e -> e.CardId = idx + 1)) then
                        yield (initialCards.[idx] + numOfCurrentCards)
                    else
                        yield initialCards.[idx]                   
            } |> Array.ofSeq
        playCardSet newCards tail

let execute =
    let lines = GetLinesFromFile path
    let initialCards = Array.create lines.Length 1
    playCardSet initialCards (cardSet lines)