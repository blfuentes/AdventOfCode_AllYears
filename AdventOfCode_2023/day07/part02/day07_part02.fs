module day07_part02

open System

open AdventOfCode_2023.Modules.LocalHelper

let bigint = System.Numerics.BigInteger.Parse

type HandType = FiveOfKind | FourOfKind | FullHouse | ThreeOfKind | TwoPairs | OnePair | HighCard

let strengthHandTypes = dict[
    FiveOfKind, 6
    FourOfKind, 5
    FullHouse, 4
    ThreeOfKind, 3
    TwoPairs, 2
    OnePair, 1
    HighCard, 0
]

let strengthCards = dict[
    "2", 2
    "3", 3
    "4", 4
    "5", 5
    "6", 6
    "7", 7
    "8", 8
    "9", 9
    "T", 10
    "J", 1
    "Q", 12
    "K", 13
    "A", 14
]

type Hand = {
    HandType: HandType
    Cards: string list
    Bid: bigint
}

let rec sortByCard (card1: string list) (card2: string list) =
    match card1, card2 with
    | [], [] -> 0
    | head1::_ , head2::_ when strengthCards.[head1] > strengthCards.[head2] -> 1
    | head1::_ , head2::_ when strengthCards.[head1] < strengthCards.[head2] -> -1
    | head1::tail1 , head2::tail2 when strengthCards.[head1] = strengthCards.[head2] -> sortByCard tail1 tail2
    | _ -> failwith "Unexpected"
    

let sortByHandType (hand1: Hand) (hand2: Hand) =
    if strengthHandTypes.[hand1.HandType] > strengthHandTypes.[hand2.HandType] then 1
    elif strengthHandTypes.[hand1.HandType] < strengthHandTypes.[hand2.HandType] then -1
    else
        sortByCard hand1.Cards hand2.Cards

let getRemainingHandType (cards: char array) =
    let handType = 
        let groups = cards |> Array.groupBy id
        match cards.Length with
        | 5 ->
            match groups.Length with
            | 5 -> HighCard
            | 4 -> OnePair
            | 3 -> 
                match groups |> Array.map (fun (key, value) -> value.Length) |> Array.sort with                
                | [| 1; 1; 3 |] -> ThreeOfKind
                | [| 1; 2; 2 |] -> TwoPairs
                | _ -> failwith "Unexpected"
            | 2 ->
                match groups |> Array.map (fun (key, value) -> value.Length) |> Array.sort with
                | [| 1; 4 |] -> FourOfKind
                | [| 2; 3 |] -> FullHouse
                | _ -> failwith "Unexpected"
            | 1 -> FiveOfKind
            | _ -> failwith "Unexpected"  
        | 4 ->
            match groups.Length with
            | 4 -> HighCard
            | 3 -> OnePair
            | 2 -> 
                match groups |> Array.map (fun (key, value) -> value.Length) |> Array.sort with
                | [| 1; 3 |] -> ThreeOfKind
                | [| 2; 2 |] -> TwoPairs
                | _ -> failwith "Unexpected"
            | 1 -> FourOfKind
            | _ -> failwith "Unexpected"
        | 3 ->
            match groups.Length with
            | 3 -> HighCard
            | 2 -> OnePair
            | 1 -> ThreeOfKind
            | _ -> failwith "Unexpected"
        | 2 ->
            match groups.Length with
            | 1 -> OnePair
            | _ -> HighCard
        | 1 -> HighCard
        | _ -> HighCard                           
    handType
        
let calculateHandType (cards: char array) =
    let numberOfJokers = cards |> Array.filter(fun c -> c = 'J') |> Array.length
    let remainingHandType = getRemainingHandType (cards |> Array.filter (fun c -> c <> 'J'))
    let newHandType =
        match numberOfJokers with
        | 1 -> 
            match remainingHandType with
            | HighCard -> OnePair
            | OnePair -> ThreeOfKind
            | TwoPairs -> FullHouse
            | ThreeOfKind -> FourOfKind
            | FourOfKind -> FiveOfKind
            | _ -> remainingHandType
        | 2 ->
            match remainingHandType with
            | HighCard -> ThreeOfKind
            | OnePair -> FourOfKind
            | ThreeOfKind -> FiveOfKind
            | _ -> remainingHandType
        | 3 ->
            match remainingHandType with
            | HighCard -> FourOfKind
            | OnePair -> FiveOfKind
            | _ -> remainingHandType
        | 4 -> FiveOfKind
        | 5 -> FiveOfKind
        | _ -> remainingHandType
    newHandType

let parseHand (hand: string) =
    let cards = hand.Split(' ')
    let bid = cards.[1] |> bigint
    let cards = cards.[0].ToCharArray() 
    { HandType = calculateHandType cards; Cards = cards |> Array.map (string) |>List.ofArray; Bid = bid }

let execute =
    let path = "day07/day07_input.txt"
    let lines = GetLinesFromFile path
    let hands = lines |> Array.map parseHand
    let sortedHands = hands |> Array.sortWith sortByHandType
    let rankedHands = sortedHands |> Array.mapi (fun index hand -> hand.Bid * (bigint(index.ToString()) + 1I))
    rankedHands |> Array.sum