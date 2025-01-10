module day25_part01

open AdventOfCode_Utilities
open AdventOfCode_2020.Modules

let applyLoop subjectNumber previousValue =
    positiveModuloInt64 (previousValue * subjectNumber) 20201227L

let getLoopResults subjectNumber =
    Seq.unfold (applyLoop subjectNumber >> duplicate >> Some) 1L

let findEncryptionKey (cardPubKey, doorPubKey) =
    let loopResults = getLoopResults 7L
    let cardLoopNumber = (loopResults |> Seq.findIndex ((=) cardPubKey))
    getLoopResults doorPubKey |> Seq.item cardLoopNumber

let execute =
    let path = "day25/day25_input.txt"
    let content = LocalHelper.GetLinesFromFile path |> Array.map int64
    let (first, second) = (content[0], content[1])
    findEncryptionKey (first, second)