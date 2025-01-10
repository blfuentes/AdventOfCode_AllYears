module day04_part02
let secretkey = "yzbqklnj"

let generateHash (key: string) (number: int) =
    use md5 = System.Security.Cryptography.MD5.Create()
    ((key + (string number).PadLeft(5, '0')))
      |> System.Text.Encoding.ASCII.GetBytes
      |> md5.ComputeHash
      |> Seq.map (fun c -> c.ToString("X2"))
      |> Seq.reduce (+)

let isValidHash (hash: string) =
    hash.StartsWith("000000")

let rec findValidHash (key: string) (n: int) =
    match (isValidHash (generateHash key n)) with
    | true -> n
    | false -> findValidHash key (n + 1)

let execute =
    findValidHash secretkey 0
