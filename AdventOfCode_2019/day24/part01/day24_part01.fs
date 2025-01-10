module day24_part01

open AdventOfCode_2019.Modules
open AdventOfCode_Utilities
open System.Numerics

let lineToCells = Seq.map (fun c -> if c = '#' then 1 else 0)
let parseContent (lines: string array) =
    lines |> Seq.map lineToCells |> Seq.collect id |> Seq.rev |> Seq.reduce (fun i j -> (i <<< 1) + j)

// Gets the number of 1s in the binary representation of x
let inline popcount x = BitOperations.PopCount(uint32 x)

let neighbours (x, y) =
    [| (x - 1, y)
       (x + 1, y)
       (x, y - 1)
       (x, y + 1) |]

// pre-compute the bitmasks to get the neighbours for a given cell
let neighbourBitmasks =
    [| for i = 0 to 24 do
        neighbours (i % 5, i / 5)
        |> Array.filter (fun (x, y) -> x >= 0 && x < 5 && y >= 0 && y < 5)
        |> Array.sumBy (fun (x, y) -> 1 <<< (y * 5 + x)) |]

// loops over the bits of the bugs and map them to their new state using a callback to get the neighbouring bug counts
let inline nextState getNeighbourBugCount bugs =
    let rec nextState' c newBugs =
        if c < 0 then newBugs
        else
            let cur = (bugs >>> c) &&& 1
            let neighbourBugs = getNeighbourBugCount c
            let bit = if (neighbourBugs = 1) || (cur = 0 && neighbourBugs = 2) then 1 else 0
            nextState' (c - 1) ((newBugs <<< 1) ||| bit)
    nextState' 24 0

let biodiversityRating =
    let getNeighbourBugCount bugs c = popcount (neighbourBitmasks.[c] &&& bugs)
    let step bugs = nextState (getNeighbourBugCount bugs) bugs
        
    repeatUntilDuplicate step

let execute =
    let path = "day24/day24_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    biodiversityRating (parseContent content)