#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

//let path = "day05/test_input_01.txt"
let input = "wtnhxymk"

let calculateMD5Hash (input: string) =
    let md5 = System.Security.Cryptography.MD5.Create()
    let inputBytes = System.Text.Encoding.ASCII.GetBytes(input)
    let hashBytes = md5.ComputeHash(inputBytes)
    let sb = System.Text.StringBuilder()
    for i = 0 to hashBytes.Length - 1 do
        sb.Append(hashBytes.[i].ToString("X2")) |> ignore
    sb.ToString()

let rec findPassword (input: string) (index: int) (password: string) =
    let hash = calculateMD5Hash (input + (index.ToString()))
    if hash.StartsWith("00000") then
        let newPassword = password + (hash.[5].ToString())
        if String.length newPassword = 8 then
            newPassword
        else
            findPassword input (index + 1) newPassword
    else
        findPassword input (index + 1) password

let execute = findPassword input 0 ""