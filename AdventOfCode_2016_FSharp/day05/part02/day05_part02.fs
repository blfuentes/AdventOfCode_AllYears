module day05_part02



let calculateMD5Hash (input: string) =
    let md5 = System.Security.Cryptography.MD5.Create()
    let inputBytes = System.Text.Encoding.ASCII.GetBytes(input)
    let hashBytes = md5.ComputeHash(inputBytes)
    let sb = System.Text.StringBuilder()
    for i = 0 to hashBytes.Length - 1 do
        sb.Append(hashBytes.[i].ToString("X2")) |> ignore
    sb.ToString()

let rec findPassword (input: string) (index: int) (password: (string*bool)[]) =
    match password |> Array.forall(fun b -> snd b) with
    | true -> password |> Array.map(fun b -> fst b) |> String.concat ""
    | false ->
        let hash = calculateMD5Hash (input + (index.ToString()))
        if hash.StartsWith("00000") then
            let position = hash.[5].ToString()
            let character = hash.[6].ToString()
            if position >= "0" && position <= "7" then
                let position = int position
                if not (snd password.[position]) then
                    password.[position] <- (character, true)
                    findPassword input (index + 1) password
                else
                    findPassword input (index + 1) password
            else
                findPassword input (index + 1) password
        else
            findPassword input (index + 1) password

let execute =
    let input = "wtnhxymk"
    findPassword input 0 [|("", false); ("", false); ("", false); ("", false); ("", false); ("", false); ("", false); ("", false)|]