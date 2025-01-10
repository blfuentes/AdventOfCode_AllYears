open System
open System.IO
open System.Text.RegularExpressions
open System.Collections.Generic
open System.Globalization

let rev str =
    StringInfo.ParseCombiningCharacters(str) 
    |> Array.rev
    |> Seq.map (fun i -> StringInfo.GetNextTextElement(str, i))
    |> String.concat ""

let hexToBin(hex: string) =
    match hex with
    | "0" -> "0000"
    | "1" -> "0001"
    | "2" -> "0010"
    | "3" -> "0011"
    | "4" -> "0100"
    | "5" -> "0101"
    | "6" -> "0110"
    | "7" -> "0111"
    | "8" -> "1000"
    | "9" -> "1001"
    | "A" -> "1010"
    | "B" -> "1011"
    | "C" -> "1100"
    | "D" -> "1101"
    | "E" -> "1110"
    | "F" -> "1111"
    | _ -> "0000"
let binToDec(bin: string) =
    Convert.ToInt64(bin, 2)

let rec parseMessage(mymessage: string, versions: int64 list) =
    let version = binToDec(mymessage.Substring(0, 3))
    let typeid = binToDec(mymessage.Substring(3, 3))
    match (int)typeid with
    | 4 -> 
        let number = mymessage.Substring(6)
        let leadingzeros = if (4 - (number.Length % 4)) = 4 then 0 else (4 - (number.Length % 4))
        let parts = number.ToCharArray() |> Array.chunkBySize(5) |> Array.map(fun chunk -> String.Join("", chunk))
        let splitIdx = parts |> Array.findIndex(fun p -> p.Substring(0, 1) = "0")
        let usedParts = parts |> Array.take(splitIdx + 1)
        let usedPartsLength = usedParts |> Array.sumBy(fun l -> l.Length)
        let remainingPart = mymessage.Substring(6 + usedPartsLength)//String.Join("", (parts |> Array.skip(splitIdx + 1)))
        let finalnumber = String.Join("", usedParts |> Array.map(fun p -> p.Substring(1, 4)))
        printf "literal value of %i", binToDec(finalnumber)
        versions @ [version] @ if (remainingPart.Contains("1") && remainingPart.Length >= 7) then parseMessage(remainingPart, []) else []
    | _ ->
        let lenghtypeid = mymessage.Substring(6, 1)
        match lenghtypeid with
        | "0" ->
            let adjustedMymessage = 
                if(22 - mymessage.Length > 0) then
                    mymessage + (new String('0', 22 - mymessage.Length))
                else
                    mymessage
            let totallength = binToDec(adjustedMymessage.Substring(7, 15))
            let subpacket = adjustedMymessage.Substring(22)
            [version] @ if (subpacket.Contains("1") && subpacket.Length >= 7) then parseMessage(subpacket, []) else []
        | "1" ->
            let adjustedMymessage = 
                if (18 - mymessage.Length > 0) then
                    mymessage + (new String('0', 18 - mymessage.Length))
                else
                    mymessage
            let numberofpackets = binToDec(adjustedMymessage.Substring(7, 11))
            let subpackets = adjustedMymessage.Substring(18)
            [version] @ if (subpackets.Contains("1") && subpackets.Length >= 7) then parseMessage(subpackets, []) else []

let path = "day16_input.txt"
//let path = "test_input_all.txt"
//let path = "test_input_00.txt" // (6)
//let path = "test_input_01.txt"   // (9)
//let path = "test_input_02.txt" // (14)
//let path = "test_input_03.txt" // (16)
//let path = "test_input_04.txt" // (12)
//let path = "test_input_05.txt" // (23)
//let path = "test_input_06.txt" // (31)          
 
let message = 
    File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> 
        Seq.map(fun l -> String.Join("", l.ToCharArray() |> Array.map(fun c -> hexToBin((string)c))))
let tmpmessage = message |> Seq.exactlyOne 

parseMessage(tmpmessage, []) |> List.sum