module day22_part01

open AdventOfCode_Utilities
open AdventOfCode_2020.Modules

let parseContent (input: string) =
  input
  |> splitByString $"{System.Environment.NewLine}{System.Environment.NewLine}"
  |> List.map (splitByString System.Environment.NewLine >> List.tail >> List.map int)
  |> unpack2

let rec playGame (hand1: int list) (hand2: int list) =
    match List.tryHead hand1, List.tryHead hand2 with
    | Some card1, Some card2 when card1 > card2 ->
        playGame ((List.tail hand1) @ [card1; card2]) (List.tail hand2)
    | Some card1, Some card2 when card2 > card1 ->
        playGame (List.tail hand1) ((List.tail hand2) @ [card2; card1])
    | Some _, None -> hand1
    | None, Some _ -> hand2
    | Some card1, Some card2 -> failwithf "Unexpected identical cards found %i %i" card1 card2
    | None, None -> failwithf "Both hands are empty.. something has gone wrong!"

let winningScore (player1Hand,player2Hand) =
    playGame player1Hand player2Hand
    |> List.rev
    |> List.indexed
    |> List.sumBy (fun (i, card) -> (i + 1) * card)

let execute =
    let path = "day22/day22_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let players = parseContent content
    winningScore players