module day17_part01

open AdventOfCode_2015.Modules

let parseContent(lines: string array) =
    lines |> Array.map int

let findCombinationsMemoized target numbers =
    let memo = System.Collections.Generic.Dictionary<_, _>()
    let rec inner target numbers =
        match target, numbers with
        | 0, _ -> [ [] ]
        | _, [] -> []
        | _, x :: xs ->
            if x > target then
                inner target xs
            else
                let key = (target, numbers)
                if memo.ContainsKey(key) then
                    memo[key]
                else
                    let withX = inner (target - x) xs |> List.map (fun subset -> x :: subset)
                    let withoutX = inner target xs
                    let result = withX @ withoutX
                    memo[key] <- result
                    result

    inner target numbers
    
let execute =
    let input = "./day17/day17_input.txt"
    let content = LocalHelper.GetLinesFromFile input
    let containers = parseContent content |> List.ofArray
    (findCombinationsMemoized 150 containers).Length