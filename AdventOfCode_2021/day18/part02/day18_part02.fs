module day18_part02

open AdventOfCode_2021.Modules

type SnailfishNumber = 
    | Simple of int * depth:int
    | Complex of SnailfishNumber * SnailfishNumber * depth:int

let rec snailfishNumberFromLine depth (line:string) = 
    match line.[0] with
    | '[' -> 
        let (nr1, offset1) = snailfishNumberFromLine (depth+1) line.[1..]
        let (nr2, offset2) = snailfishNumberFromLine (depth+1) line.[1+offset1..]
        (Complex(nr1, nr2, depth), 1 + offset1 + offset2)
    | ']'| ',' -> 
        let (nr, offset) = snailfishNumberFromLine depth line.[1..]
        (nr, offset + 1)
    | x -> 
        (Simple(int x - int '0', depth), 1)

let rec tryAddFromLeft nr sfn = 
    match sfn with
    | Complex(sfn1, sfn2, d) -> 
        Complex(sfn1 
        |> tryAddFromLeft nr, sfn2, d)
    | Simple(x, d) -> 
        Simple(x+nr, d)

let rec tryAddFromRight nr sfn = 
    match sfn with
        | Complex(sfn1, sfn2, d) -> 
            Complex(sfn1, sfn2 |> tryAddFromRight nr, d)
        | Simple(x, d) -> 
            Simple(x+nr, d)

let rec explodeIfRequired sfn =
    match sfn with
    | Complex(Simple(x1, _), Simple(x2, _), 4) ->
        (true, Simple(0, 4), x1, x2)
    | Simple(_, _) -> 
        (false, sfn, 0, 0)
    | Complex(x1, x2, d) -> 
        let (exploded, explodedLeft, l, r) = explodeIfRequired x1
        if exploded then 
            (true, Complex(explodedLeft, x2 |> tryAddFromLeft r, d), l, 0)
        else 
            let (exploded, explodedRight, l_, r_) = explodeIfRequired x2
            if exploded then 
                (true, Complex(x1 |> tryAddFromRight l_, explodedRight, d), 0, r_) 
            else 
                (false, sfn, 0, 0)
                           
let rec splitIfRequired sfn =
    match sfn with
    | Simple(x, d) when x >= 10 -> 
        (true, Complex(Simple(x/2, d+1), Simple((if x%2 = 0 then x/2 else x/2+1), d+1), d))
    | Simple(_, _) -> 
        (false, sfn)
    | Complex(x1, x2, d) -> 
        let (splitted, splittedSfn) = splitIfRequired x1
        if splitted then 
            (true, Complex(splittedSfn, x2, d))
        else
            let (splitted, splittedSfn) = splitIfRequired x2
            if splitted then 
                (true, Complex(x1, splittedSfn, d)) 
            else 
                (false, sfn)

let rec reduceSnailFishNumber sfn =
    let (exploded, explodedSfn, _, _) = explodeIfRequired sfn
    if not exploded then 
        let (splitted, splittedSfn) = splitIfRequired sfn
        if not splitted then
            sfn
        else reduceSnailFishNumber splittedSfn
    else reduceSnailFishNumber explodedSfn

let rec withIncreasedDepth sfn = 
    match sfn with
    | Simple(x, d) -> 
        Simple(x, d+1)
    | Complex(x1, x2, d) -> 
        Complex(x1 |> withIncreasedDepth, x2 |> withIncreasedDepth, d+1)

let rec magnitude sfn = 
    match sfn with
    | Complex(sfn1, sfn2, _) -> 
        3* magnitude sfn1 + 2* magnitude sfn2
    | Simple(x, d) -> 
        x

let largestMagnitudeAnyTwoNumbers sfn =
    let allAdditions = 
        sfn 
        |> List.map (fun sfn1 -> 
            sfn 
            |> List.filter (fun sfn2 -> sfn2 <> sfn1) 
            |> List.map (fun sfn2 -> (sfn1, sfn2))
        ) 
        |> List.collect (fun t -> t)
    let reducedAdditions = 
        allAdditions 
        |> List.map (fun (sfn1, sfn2) -> 
            Complex(sfn1 |> withIncreasedDepth, sfn2 |> withIncreasedDepth, 0) |> reduceSnailFishNumber
        )
    let magnitudes = 
        reducedAdditions 
        |> List.map (fun add -> magnitude add)

    magnitudes |> List.max

let parseContent(lines: string list) =
    lines
    |> List.map (fun line -> 
        line 
        |> (snailfishNumberFromLine 0) 
        |> fst
    )

let execute =
    let path = "day18/day18_input.txt"
    let content = LocalHelper.GetLinesFromFile path |> List.ofArray
    let snailfishnumbers = parseContent content

    largestMagnitudeAnyTwoNumbers snailfishnumbers