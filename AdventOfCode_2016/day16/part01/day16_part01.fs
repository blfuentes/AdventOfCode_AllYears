module day16_part01

open AdventOfCode_2016.Modules

let dragonCurve(input: char array) =
    let replaced =
        input
        |> Array.rev
        |> Array.map(fun c -> if c = '1' then '0' else '1')
    Array.concat [input; [|'0'|]; (replaced)]

let checksum(input: char array) =
    let mutable repeat = true
    let mutable checksum = input
    while repeat do
        checksum <-
            checksum
            |> Array.chunkBySize 2
            |> Array.map(fun r -> if r[0] = r[1] then '1' else '0')
        if checksum.Length % 2 <> 0 then
            repeat <- false
    checksum
        
let fillDisk(size: int) (input: char array) =
    let mutable dragonc = dragonCurve input
    while dragonc.Length < size do
        dragonc <- dragonCurve dragonc
    checksum (dragonc |> Array.take(size))


let execute =
    let path = "day16/day16_input.txt"
    let content = (LocalHelper.GetContentFromFile path).ToCharArray()
    let size = 272

    String.concat "" ((fillDisk size content)|> Array.map string)