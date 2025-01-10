module day13_part01

open AdventOfCode_2024.Modules
open AdventOfCode_Utilities
open System.Text.RegularExpressions

type Push = {
    Name: string;
    X : int;
    Y : int;
}

type Combination = {
    ButtonA : Push;
    ButtonB : Push;
    ResultX : int;
    ResultY : int;
}

let getValues(line: string) =
    let regexp = @"(\d+)[^\d]+(\d+)"
    let regex = Regex(regexp)
    match regex.Match(line) with
    | m -> ((int)m.Groups[1].Value, (int)m.Groups[2].Value)
    | _ -> failwith "no matching"

let parseContent(lines: string array) =
    let groups = Utilities.getGroupsOnSeparator (lines |> List.ofArray) ""
    groups
    |> List.map(fun g ->
        
        let (pushAX, pushAY) = getValues(g.Item(0))
        let (pushBX, pushBY) = getValues(g.Item(1))
        let (resultX, resultY) = getValues (g.Item(2))
        { ButtonA = { Name = "A"; X = pushAX; Y = pushAY }; ButtonB = { Name = "B"; X = pushBX; Y = pushBY }; ResultX = resultX; ResultY = resultY }
    )
    
let solveEcuation(ecuation: Combination) (limit: int)  =
    let isValidSolution (aValue: int) =
        let bValue = (ecuation.ResultX - ecuation.ButtonA.X * aValue) / ecuation.ButtonB.X
        if bValue * ecuation.ButtonB.X + aValue * ecuation.ButtonA.X = ecuation.ResultX &&
            bValue * ecuation.ButtonB.Y + aValue * ecuation.ButtonA.Y = ecuation.ResultY &&
            bValue >= 0 then
            Some (aValue, bValue)
        else None

    seq { 0 .. limit }
    |> Seq.tryPick isValidSolution

let execute() =
    let path = "day13/day13_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let ecuations = parseContent content
    ecuations
    |> List.sumBy(fun e ->
        match solveEcuation e 100 with
        | Some((aValue, bValue)) -> aValue*3 + bValue*1
        | _ -> 0
    )