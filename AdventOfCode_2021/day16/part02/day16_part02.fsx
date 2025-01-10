open System
open System.IO
open System.Text.RegularExpressions
open System.Collections.Generic
open System.Globalization  

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

let typeToOp(typeid: int64) =
    match int32(typeid) with
    | 4 -> "lit"
    | 0 -> "sum"
    | 1 -> "prod"
    | 2 -> "min"
    | 3 -> "max"
    | 5 -> "greater"
    | 6 -> "lesser"
    | 7 -> "equal"
    | _ -> ""

let performOp(myOp: string, values: int64 list) =
    match myOp with
    | "sum" -> values |> List.sum
    | "prod" -> values |> List.fold (*) 1
    | "min" -> values |> List.min
    | "max" -> values |> List.max
    | "lesser" -> 
        if values.Item(0) < values.Item(1) then 1 else 0
    | "greater" -> 
        if values.Item(0) > values.Item(1) then 1 else 0
    | "equal" -> if (values.Item(1) = values.Item(0)) then 1 else 0
    | _ -> int64(0)

let rec parseMessage(mymessage: string, length: int64): (int64[] * string) =
    let rec parseType0(myremaining: string, mysublength: int64, mytotal: int64, myvalues: int64 list) =
        match mytotal = mysublength with
        | false -> 
            let (values, newremaining) = parseMessage(myremaining, mysublength)
            parseType0(newremaining, values.[1], mytotal, myvalues @ [values.[0]])
        | true ->
            (mysublength, myremaining, myvalues)
    let rec parseType1(myremaining: string, counter: int64, subpacketlength:int64, mytotal: int64, myvalues: int64 list) =
        match mytotal = counter with
        | false -> 
            let (values, newremaining) = parseMessage(myremaining, subpacketlength)
            parseType1(newremaining, counter + int64(1), values.[1], mytotal, myvalues @ [values.[0]])
        | true ->
            (subpacketlength, myremaining, myvalues)
    let rec parseType4(myremaining: string, mypacketvalue:string, mylength: int64)=
        match myremaining.Substring(0, 1) with
        | "0" ->
            let newpacketValue = mypacketvalue + myremaining.Substring(1, 4)
            let newremaining = myremaining.Substring(5)
            let newlength = mylength + int64(5)
            ([|binToDec(newpacketValue); newlength|], newremaining)
        | _ ->
            let newpacketValue = mypacketvalue + myremaining.Substring(1, 4)
            let newremaining = myremaining.Substring(5)
            let newlength = mylength + int64(5)
            parseType4(newremaining, newpacketValue, newlength)

    let version = binToDec(mymessage.Substring(0, 3))
    let packettype = binToDec(mymessage.Substring(3, 3))
    let newlength = length + int64(6)
    let (values, newremaining) = 
        match int32(packettype) with
        | 4 ->
            let remaining = mymessage.Substring(6)
            parseType4(remaining, "", newlength)
        |_ -> 
            let typeid = mymessage.Substring(6, 1)
            let remaining = mymessage.Substring(7)
            let newlength2 = newlength + int64(1)
            let (subvalues, newsublength, newremaining) =
                match typeid with
                | "0" ->
                    let totallength = binToDec(remaining.Substring(0, 15))
                    let newremaining = remaining.Substring(15)
                    let tmplength = newlength2 + int64(15)
                    let subpacketlength = int64(0)
                    let (sl, my, mv) = parseType0(newremaining, subpacketlength, totallength, [])
                    (mv, tmplength + sl, my)
                | _ ->
                    let totalcount = binToDec(remaining.Substring(0, 11))
                    let newremaining = remaining.Substring(11)
                    let tmplength = newlength2 + int64(11)
                    let subpacketlength = int64(0)
                    let counter = int64(0)
                    let (sl, my, mv) = parseType1(newremaining, counter, subpacketlength, totalcount, [])
                    (mv, tmplength + sl, my)
            let newpacketvalue = performOp(typeToOp(packettype), subvalues)
            ([|newpacketvalue; newsublength|], newremaining)
    (values, newremaining)


let path = "day16_input.txt"   // (871)
//let path = "test_input_00.txt" // (6)
//let path = "test_input_01.txt" // (9)
//let path = "test_input_02.txt" // (14)
//let path = "test_input_03.txt" // (16)
//let path = "test_input_04.txt" // (12)
//let path = "test_input_05.txt" // (23)
//let path = "test_input_06.txt" // (31)          
 
let message = 
    File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> 
        Seq.map(fun l -> String.Join("", l.ToCharArray() |> Array.map(fun c -> hexToBin((string)c))))

let tmpmessage = message |> Seq.exactlyOne 
parseMessage(tmpmessage, 0)

let samples = [|"C200B40A82";                   // "2"; "1"; "sum" -> 3
                "04005AC33890";                 // "9"; "6"; "prod" -> 54
                "880086C3E88112";               // "9"; "8"; "7"; "min" -> 7
                "CE00C43D881120";               // "9"; "8"; "7"; "max" -> 9
                "D8005AC2A8F0";                 // "15"; "5"; "lesser" -> 1
                "F600BC2D8F";                   // "15"; "5"; "greater" -> 0
                "9C005AC2F8F0";                 // "15"; "5"; "equal" -> 0
                "9C0141080250320F1802104A08"|]  // "2"; "2"; "prod"; "3"; "1"; "sum"; "equal" -> 1
let tmpmessage2 = String.Join("", samples.[7].ToCharArray() |> Array.map(fun c -> hexToBin((string)c)))

parseMessage(tmpmessage2, 0)