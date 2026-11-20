module day04_part01

open System.IO

let filepath = __SOURCE_DIRECTORY__ + @"../../day04_input.txt"
let input = File.ReadAllText(filepath).Split('-') |> Array.map int

let isSixDigitNumber(pass: int) : bool =
    pass.ToString().Length = 6

let isInRange(range:int array, pass: int) : bool =
    pass >= range.[0] && pass <= range.[1]

let hasDuplicatedAdjacentDigit(pass: int) : bool =
    pass.ToString().ToCharArray() |> Array.map (fun _c -> int(System.Char.GetNumericValue(_c)))
    |> Array.toSeq |> Seq.pairwise |> Seq.exists (fun (a, b) -> a = b)

let neverDecrease(pass: int) : bool =
    pass.ToString().ToCharArray() |> Array.map (fun _c -> int(System.Char.GetNumericValue(_c)))
    |> Array.toSeq |> Seq.pairwise |> Seq.forall (fun (a, b) -> a <= b)

let isValidPassword(pass: int) : bool =
    isSixDigitNumber pass && isInRange(input, pass) && hasDuplicatedAdjacentDigit pass && neverDecrease pass

let validPasswords = 
    [input.[0]..input.[1]]
    |> List.filter (fun pass -> isValidPassword pass)

let execute =
    validPasswords.Length