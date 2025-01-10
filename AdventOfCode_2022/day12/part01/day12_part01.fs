module day12_part01

open System.Collections.Generic

open AdventOfCode_2022.Modules

let path = "day12/day12_input.txt"

let findElement (element: char) (map: char [][]) =
    seq {
        for row in 0..map.GetLength(0) - 1 do
            for col in 0..map.[row].GetLength(0) - 1 do
                if map.[row].[col] = element then yield [|row; col|]
    } |> Seq.item(0)

let valueOf (el: char) = 
    match el with 
    | 'S' -> int('a')
    | 'E' -> int('z')
    | _ -> int(el)

    // U  L  R  D
let rowNums = [|-1; 0; 0; 1|];
let colNums = [|0; -1; 1; 0|];

let rec generateLeePath (map: char[][]) (dest: int[]) (visited: int[] list) (currentQueue: Queue<int[]>)=
    if currentQueue.Count = 0 then 
        -1
    else
        let current = currentQueue.Peek()
        if current.[0] = dest.[0] && current.[1] = dest.[1] then 
            current.[2]
        else
            let removed = currentQueue.Dequeue()
            let newVisited = 
                seq {
                    for i in [0..3] do
                        let checkRow = current.[0] + rowNums.[i]
                        let checkCol = current.[1] + colNums.[i]
                        let checker = [|checkRow; checkCol; current.[2] + 1|]
                        if checkRow >= 0 && checkRow < map.Length && 
                            checkCol >= 0 && checkCol < map.[0].Length && 
                            not (visited |> List.exists(fun v -> v.[0] = checker.[0] && v.[1] = checker.[1])) then
                            let fromValue = valueOf(map.[current.[0]].[current.[1]])
                            let targetValue = valueOf(map.[checker.[0]].[checker.[1]])
                            if targetValue - fromValue <= 1 then
                                currentQueue.Enqueue(checker)
                                yield checker
                } |> List.ofSeq      
            generateLeePath map dest (visited @ newVisited) currentQueue

let execute =
    let inputLines = LocalHelper.GetLinesFromFile(path) |> Array.map(fun l -> l.ToCharArray())
    let start = findElement 'S' inputLines
    let goal = findElement 'E' inputLines
    let queue = new Queue<int[]>()
    let init = [|start.[0]; start.[1]; 0|]
    queue.Enqueue(init)
    generateLeePath inputLines goal [init] queue