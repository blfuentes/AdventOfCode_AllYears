module day22_part02

open AdventOfCode_Utilities
open AdventOfCode_2020.Modules

let parseContent (input: string) =
  input
  |> splitByString $"{System.Environment.NewLine}{System.Environment.NewLine}"
  |> List.map (splitByString System.Environment.NewLine >> List.tail >> List.map int)
  |> unpack2

type Player = P1 | P2

let rec playRecursiveGame previousHands (hand1: int list) (hand2: int list) =
    if previousHands |> List.contains (hand1, hand2) then
        P1, hand1
    else
        let nextPreviousHands = (hand1, hand2) :: previousHands
        match List.tryHead hand1, List.tryHead hand2 with
        | Some card1, Some card2 ->
            if List.length hand1 > card1 && List.length hand2 > card2 then
                match playRecursiveGame [] (hand1 |> List.tail |> List.take card1) (hand2 |> List.tail |> List.take card2) with
                | P1, _ -> playRecursiveGame nextPreviousHands ((List.tail hand1) @ [card1; card2]) (List.tail hand2) 
                | P2, _ -> playRecursiveGame nextPreviousHands (List.tail hand1) ((List.tail hand2) @ [card2; card1])
            elif card1 > card2 then 
                playRecursiveGame nextPreviousHands ((List.tail hand1) @ [card1; card2]) (List.tail hand2)
            else
                playRecursiveGame nextPreviousHands (List.tail hand1) ((List.tail hand2) @ [card2; card1])
        | Some _, None -> P1, hand1
        | None, Some _ -> P2, hand2
        | None, None -> failwithf "Both hands are empty.. something has gone wrong!"

let winningRecursive (player1Hand,player2Hand) =
    playRecursiveGame [] player1Hand player2Hand
    |> snd
    |> List.rev
    |> List.indexed
    |> List.sumBy (fun (i, card) -> (i + 1) * card)

let execute =
    let path = "day22/day22_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let players = parseContent content
    winningRecursive players
