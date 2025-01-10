open System
open System.IO
open System.Text.RegularExpressions
open System.Collections.Generic

#load @"../../Modules/Utilities.fs"
open AdventOfCode_Utilities

//let path = "day15_input.txt"
let path = "test_input.txt"

let cave = 
    File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.map(fun l -> l.ToCharArray() |> Array.map(fun r -> Int32.Parse((r |> string)))) |> Seq.toArray

let buildfullcave(mycave: int[][], times: int) =
    let maxX = mycave.Length * times
    let maxY = mycave.[0].Length * times
    let newCave = Utilities.toJagged(Array2D.create maxX maxY 0)
    let increments = [|[|0..4|]; [|1..5|]; [|2..6|]; [|3..7|]; [|4..8|]|];
    for row in 0..(mycave.Length - 1) do
        for col in 0..(mycave.[0].Length - 1) do
            for tr in 0..(times - 1) do
                for tc in 0..(times - 1) do
                    let newValue = if (mycave.[row].[col] + increments.[tr].[tc]) % 9 = 0 then 9 else (mycave.[row].[col] + increments.[tr].[tc]) % 9
                    newCave.[row + mycave.Length * tr].[col + mycave.Length * tc] <- newValue

    newCave

let printCave(c: int[][]) =
    for row in 0..(c.Length - 1) do
        for col in 0..(c.[0].Length - 1) do
            printf "%i" c.[row].[col]
        printfn ""

let getNeighboors(myCave: int[][], from: int[]) =
    //let left = [|from.[0]; from.[1] - 1|]
    let right = [|from.[0]; from.[1] + 1|]
    //let up = [|from.[0] - 1; from.[1]|]
    let down = [|from.[0] + 1; from.[1]|]
    //let valid = [left; right; up; down] |> 
    let valid = [right; down] |> 
                List.filter(fun n -> n.[0] >= 0 && n.[0] <= (myCave.Length - 1) &&
                                        n.[1] >= 0 && n.[1] <= (myCave.[0].Length - 1))
    valid |> List.map(fun p -> [|p.[0]; p.[1]; myCave.[p.[0]].[p.[1]]|])

//printCave cave

let heuristic(a: int[], b: int[]) =
    Math.Abs(a.[1] - b.[1]) + Math.Abs(a.[0] - b.[0])

let rec findPath(mycave: int[][], goal: int[], myfrontier: int[] list, mycamefrom: Dictionary<int*int, int*int>, mycostsofar: Dictionary<int*int, int>) =
    match (myfrontier |> List.sortBy(fun f -> f.[2])) with 
    | [] -> (mycamefrom, mycostsofar)
    | current::tail -> 
        match current.[0] = goal.[0] && current.[1] = goal.[1] with
        | true -> (mycamefrom, mycostsofar)
        | false -> 
            let neighboors = getNeighboors(mycave, current)
            let newfrontier = Array.create neighboors.Length [|-1; -1; 0|]

            for idx in 0..(neighboors.Length - 1) do
                let next = neighboors.Item(idx)
                let newcost = mycostsofar.Item((current.[0], current.[1])) + mycave.[next.[0]].[next.[1]]
                if not(mycostsofar.ContainsKey((next.[0], next.[1]))) || newcost < mycostsofar.Item((next.[0], next.[1])) then
                    mycostsofar.Item((next.[0], next.[1])) <- newcost
                    let priority = newcost + heuristic(goal, next)
                    newfrontier.[idx] <- [|next.[0]; next.[1]; priority|]
                    mycamefrom.Item((next.[0], next.[1])) <- (current.[0], current.[1])

            findPath(mycave, goal, tail @ (newfrontier |> Array.filter(fun f -> f.[0] <> -1 && f.[1] <> -1) |> Array.toList), mycamefrom, mycostsofar)

let rec findpathback(current:(int*int), fullpath: Dictionary<int*int, int*int>, result: (int*int) list) =
    match current with
    | (0, 0) -> result
    | (a, b) -> 
        [current] @ findpathback(fullpath.Item((a, b)), fullpath, result)

let calculateRisk(mypath: (int*int) list, mycave: int[][]) =
    mypath |> List.sumBy(fun p -> mycave.[fst p].[snd p])



// not working...
let findmultiplepaths(mycave: int[][], size:int) =
    let border = mycave.Length / 5
    let partials =
        seq {
            for p in 0..(size - 1) do
                let init = [|p * border; p * border|]
                let limit = if p <> size then [|init.[0] + border; init.[1] + border|] else [|init.[0] + (border - 1); init.[1] + (border - 1)|]
                let frontier = [[|init.[0]; init.[1]; 0|]]
                let camefrom = new Dictionary<int*int, int*int>()
                let costsofar = new Dictionary<int*int, int>()
                camefrom.Item((init.[0], init.[1])) <- (-1, -1)
                costsofar.Item((init.[0], init.[1])) <- 0
                let resultpath = findPath(mycave, [|limit.[0]; limit.[1]; mycave.[limit.[0]].[limit.[1]]|], frontier, camefrom, costsofar)
                let pathback = findpathback((limit.[0], limit.[1]), fst resultpath, []) |> List.rev
                yield calculateRisk(pathback, mycave)
                
        }
    partials |> Seq.sum

let extracave = buildfullcave(cave, 5)
printCave extracave
let sumresult = findmultiplepaths(extracave, 5)

let frontier = [[|0; 0; 0|]]
let camefrom = new Dictionary<int*int, int*int>()
let costsofar = new Dictionary<int*int, int>()
camefrom.Item((0, 0)) <- (-1, -1)
costsofar.Item((0, 0)) <- 0

let limit = extracave.Length - 1
let resultpath = findPath(extracave, [|limit; limit; extracave.[limit].[limit]|], frontier, camefrom, costsofar)

let xx = findpathback((limit, limit), fst resultpath, []) |> List.rev

calculateRisk(xx, extracave)
