module day14_part01

open AdventOfCode_2017.Modules

let hexToBinary hexDigit =
    let hexCharToInt c =
        match c with
        | '0' -> 0 | '1' -> 1 | '2' -> 2 | '3' -> 3
        | '4' -> 4 | '5' -> 5 | '6' -> 6 | '7' -> 7
        | '8' -> 8 | '9' -> 9
        | 'a' | 'A' -> 10 | 'b' | 'B' -> 11
        | 'c' | 'C' -> 12 | 'd' | 'D' -> 13
        | 'e' | 'E' -> 14 | 'f' | 'F' -> 15
        | _ -> failwith "Invalid hexadecimal digit"
    let value = hexCharToInt hexDigit
    System.Convert.ToString(value, 2).PadLeft(4, '0')

let buildRows(input: string) =
    let sum =
        [0..127]
        |> List.sumBy(fun row ->
            let knothash = String.concat "" ((LocalHelper.knotHasher $"{input}-{row}").ToCharArray() |> Array.map(fun c -> hexToBinary c))
            let hashedrow = knothash.ToCharArray() |> Array.map(fun c -> if c = '1' then 1 else 0)
            hashedrow |> Array.sum
        )
    sum

let execute() =
    let path = "day14/day14_input.txt"
    let content = LocalHelper.GetContentFromFile path

    buildRows content