module day04_part02

open System.IO

let filepath = __SOURCE_DIRECTORY__ + @"../../day04_input.txt"
let input = File.ReadAllText(filepath).Split('-') |> Array.map int

let isSixDigitNumber(pass: int) : bool =
    pass.ToString().Length = 6

let isInRange(range:int array, pass: int) : bool =
    pass >= range.[0] && pass <= range.[1]

// Criteria Part 1
let hasDuplicatedAdjacentDigit(pass: int) : bool =
    pass.ToString().ToCharArray() |> Array.map (fun _c -> int(System.Char.GetNumericValue(_c)))
    |> Array.toSeq |> Seq.pairwise |> Seq.exists (fun (a, b) -> a = b)

// Criteria Part 2
let splitAt f list =
  let rec splitAtAux acc list = 
    match list with
    | x::y::ys when f x y -> List.rev (x::acc), y::ys
    | x::xs -> splitAtAux (x::acc) xs
    | [] -> (List.rev acc), []
  splitAtAux [] list

let foldUntilEmpty f list = 
  let rec foldUntilEmptyAux acc list =
    match f list with
    | l, [] -> l::acc |> List.rev
    | l, rest -> foldUntilEmptyAux (l::acc) rest
  foldUntilEmptyAux [] list

let splitAtEvery f list = foldUntilEmpty (splitAt f) list

let hasMax2DuplicatedAdjacentDigit(pass: int) : bool =
    pass.ToString().ToCharArray() |> Array.map (fun _c -> int(System.Char.GetNumericValue(_c)))
    |> Array.toList |> splitAtEvery (<>) |> List.exists (fun _l -> _l.Length = 2)

let neverDecrease(pass: int) : bool =
    pass.ToString().ToCharArray() |> Array.map (fun _c -> int(System.Char.GetNumericValue(_c)))
    |> Array.toSeq |> Seq.pairwise |> Seq.forall (fun (a, b) -> a <= b)

let isValidPassword(pass: int) : bool =
    isSixDigitNumber pass && isInRange(input, pass) && hasMax2DuplicatedAdjacentDigit pass && neverDecrease pass

let validPasswords = 
    [input.[0]..input.[1]]
    |> List.filter (fun pass -> isValidPassword pass)

let execute =
    validPasswords.Length