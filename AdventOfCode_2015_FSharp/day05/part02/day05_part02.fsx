open System.IO

// let path = "test_input_01.txt"
let path = "day05_input.txt"

let inputLines = File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |>Seq.toList

let rec comb n l =
  match (n,l) with
  | (0,_) -> [[]]
  | (_,[]) -> []
  | (n,x::xs) ->
      let useX = List.map (fun l -> x::l) (comb (n-1) xs)
      let noX = comb n xs
      (useX @ noX)

let convertToStrings (l: list<char>) = 
    l |> List.map string |> List.reduce (+)

let count needle haystack =
    [ for i in 0..String.length haystack - 1 -> haystack.[i..] ]
    |> Seq.filter (Seq.forall2 (=) needle)
    |> Seq.length
// (count "qj" "qjhvhtzxzqqjkmpb")

let areConsecutive (i: List<int>) =
    i |> List.pairwise |> List.forall(fun a -> (snd a) - (fst a) = 1)

// areConsecutive [1; 2; 3]

let input = "aaa"
let combinations = input.ToCharArray() |> Array.toList |> List.pairwise |> List.map(fun p -> string (fst p) + string (snd p))
let filtered = combinations |> List.mapi(fun idx c -> (c, idx)) |> List.groupBy(fun g -> fst g) |> List.filter(fun g -> (snd g).Length > 1)
let filteredReduced = filtered |> List.map(fun l -> (fst l, (snd l |> List.map snd))) |> List.map snd
// let testReduced = [[0; 2; 4]]
(filteredReduced |> List.filter(fun e -> (comb 2 e) |> List.exists(fun n -> not (areConsecutive n)))).Length > 0

let pairIsRepeatedNoOverlapping (input: string) =
    let combinations = input.ToCharArray() |> Array.toList |> List.pairwise |> List.map(fun p -> string (fst p) + string (snd p))
    let filtered = combinations |> List.mapi(fun idx c -> (c, idx)) |> List.groupBy(fun g -> fst g) |> List.filter(fun g -> (snd g).Length > 1)
    let filteredReduced = filtered |> List.map(fun l -> (fst l, (snd l |> List.map snd))) |> List.map snd
    (filteredReduced |> List.filter(fun e -> (comb 2 e) |> List.exists(fun n -> not (areConsecutive n)))).Length > 0

let mirroredLetter (input: string) =
    let listOfThrees = [0..input.Length - 3] |> List.map(fun i -> input.Substring(i).ToCharArray() |> Array.take(3))
    (listOfThrees |> List.filter (fun l -> l.[0] = l.[2])).Length > 0

let isNiceStringNewRules (input: string) =
    pairIsRepeatedNoOverlapping input && mirroredLetter input

// let isNiceStringNewRules2 (input: string) =
//     pairIsRepeatedNoOverlapping input //&& mirroredLetter input

isNiceStringNewRules "qjhvhtzxzqqjkmpb"
isNiceStringNewRules "xxyxx"
isNiceStringNewRules "uurcxstgmygtbstg"
isNiceStringNewRules "ieodomkazucvgmuy"

(inputLines |> List.filter(fun l -> isNiceStringNewRules l)).Length
