namespace AdventOfCode_2020.Modules

open System

open AdventOfCode_Utilities
open CustomDataTypes

[<AutoOpenAttribute>]
module Helpers =
    // DAY 03
    let getCollisionsBasic (currentForest: list<int[]>) initX initY right down maxwidth maxheight =
        let positions = [initY..down..maxheight]
        seq {
            for pos in initY..down..maxheight do
                let currentPos = positions |> List.findIndex (fun x -> x = pos)
                let point = [|((initX + right) * (currentPos + 1)) % maxwidth; pos + down|]
                match currentForest |> List.exists (fun t -> t.[0] = point.[0] && t.[1] = point.[1]) with 
                | true -> yield point
                | _ -> ()
        } |> Seq.length

    // DAY 04
    let byrValid (elem:string) =
        elem.Length = 4 && (elem |> int) >= 1920 && (elem |> int) <= 2002

    let iyrValid (elem:string) =
        elem.Length = 4 && (elem |> int) >= 2010 && (elem |> int) <= 2020

    let eyrValid (elem:string)=
        elem.Length = 4 && (elem |> int) >= 2020 && (elem |> int) <= 2030

    let hgtValid (elem:string)=
        let parts =
            match elem with
            | Regex @"(?<height>\d+)(?<unittype>\w+)" [m; M] -> Some { height= m |> int; unittype = M }
            | _ -> None
        match parts with
        | Some { HeightType.height = height; HeightType.unittype = unittype; } when unittype = "cm" -> height >= 150 && height <= 193
        | Some { HeightType.height = height; HeightType.unittype = unittype; } when unittype = "in" -> height >= 59 && height <= 76
        | _ -> false

    let hclValid (elem:string)=
        match elem with
        | Regex @"#[0-9a-f]{6}" result -> true
        | _ -> false


    let eclValid (elem:string)=
        ["amb"; "blu"; "brn"; "gry"; "grn"; "hzl"; "oth"] |> List.contains(elem)

    let pidValid (elem:string)=
        elem.Length = 9 && elem |> Seq.forall Char.IsDigit

    // DAY 05
    let rec calculateSeat minRowCur maxRowCur minColCur maxColCur (index:int) (seatdefinition:string)=
        match index < seatdefinition.Length with
        | true -> 
            match seatdefinition.[index] with
            | 'F' -> calculateSeat minRowCur (minRowCur + (maxRowCur - minRowCur) / 2)  minColCur maxColCur (index + 1) seatdefinition
            | 'B' -> calculateSeat (minRowCur + (maxRowCur + 1 - minRowCur) / 2) maxRowCur minColCur maxColCur (index + 1) seatdefinition
            | 'L' -> calculateSeat minRowCur maxRowCur minColCur (minColCur + (maxColCur - minColCur) / 2) (index + 1) seatdefinition
            | 'R' -> calculateSeat minRowCur maxRowCur (minColCur + (maxColCur + 1 - minColCur) / 2) maxColCur  (index + 1) seatdefinition
            | _ -> 0
        | false -> minRowCur * 8 + minColCur

    let calculateBinarySeat (seatdefinition: string) =
        Convert.ToInt32(String(seatdefinition.Replace('F', '0').Replace('B', '1').Replace('L', '0').Replace('R', '1').ToCharArray()), 2)

    // DAY 06
    let concatStringList (list:string list) =
        seq {
            for l in list do
                yield! l.ToCharArray()
        } |> List.ofSeq

    let concatStringListSeparated (list:string list) =
        seq {
            for l in list do
                yield l.ToCharArray()
        } |> List.ofSeq

    let commonElements (input: char array list) =
        let inputAsList = input |> List.map (List.ofArray)
        let inputAsSet = List.map Set.ofList inputAsList
        let elements =  Seq.reduce Set.intersect inputAsSet
        elements

    let commonElements2 (input: char array list) =
        input |> List.map (List.ofArray) |> Seq.map Set.ofList |> Set.intersectMany

    // DAY 07
    let parseBagsInput (input:string array) =
        seq {
            for line in input do
                let parts = line.Replace("bags", "").Replace("bag", "").Split([|"contain"|], StringSplitOptions.None)
                let content = 
                    seq {
                        let contentList = parts.[1].Split(',') |> Array.map (fun x -> x.Trim())
                        for content in contentList do
                            let element = 
                                match content with
                                | Regex @"(?<size>\d+)\s(?<color>\w+\s\w+)" [s; c] -> Some { Name = c.Trim(); Size = (s |> int); Content = [] }
                                | _ -> None
                            if element.IsSome then
                                yield element.Value                        
                    } |> List.ofSeq
                let element = 
                    Some {
                        Name = parts.[0].Trim();
                        Size = 1;
                        Content = content
                    }
                yield element.Value
        }

    let rec countBagContainers (originalBag: ChristmasBag) (childcontainers: ChristmasBag list) (allcontainers: ChristmasBag list) =
        match childcontainers.IsEmpty with
        | true -> false
        | false ->
            match (childcontainers |> List.exists (fun c -> c.Name = originalBag.Name)) with
            | true -> true
            | false -> 
                (childcontainers |> List.map(fun c -> countBagContainers originalBag ((allcontainers |> List.find(fun s -> s.Name = c.Name)).Content) allcontainers) |> List.exists (fun c -> c))

    let rec countBags (currentCount: int) (childcontainers: ChristmasBag list) (allcontainers: ChristmasBag list)=
        match childcontainers.IsEmpty with
        | true -> currentCount
        | false -> 
            let childrenSize = childcontainers |> List.map (fun c -> c.Size + c.Size * countBags 0 ((allcontainers |> List.find(fun s -> s.Name = c.Name)).Content) allcontainers) |> List.sum
            currentCount + childrenSize

    // Day 08
    let rec calculateAccumulator (currentValue: int) (consumedOps: int list) (newOpIdx: int) (program: HandledOperation[]) =
        if consumedOps |> List.contains(newOpIdx) then
            currentValue
        else
            let newOp = program.[newOpIdx]
            match newOp.Op with
            | HandheldOpType.ACC -> calculateAccumulator (currentValue + newOp.Offset) (consumedOps @ [newOpIdx]) (newOpIdx + 1) program
            | HandheldOpType.JMP -> calculateAccumulator currentValue (consumedOps @ [newOpIdx]) (newOpIdx + newOp.Offset) program
            | HandheldOpType.NOP -> calculateAccumulator currentValue (consumedOps @ [newOpIdx]) (newOpIdx + 1) program
            | _ -> currentValue

    let rec calculateAccumulatorComplex (currentValue: int) (consumedOps: int list) (newOpIdx: int) (program: HandledOperation[]) =
        if newOpIdx = program.Length then
            (true, currentValue)
        else
            match consumedOps |> List.contains(newOpIdx) with
            | true -> (false, currentValue)
            | false -> 
                let newOp = program.[newOpIdx]
                match newOp.Op with
                | HandheldOpType.ACC -> calculateAccumulatorComplex (currentValue + newOp.Offset) (consumedOps @ [newOpIdx]) (newOpIdx + 1) program
                | HandheldOpType.JMP -> calculateAccumulatorComplex currentValue (consumedOps @ [newOpIdx]) (newOpIdx + newOp.Offset) program
                | HandheldOpType.NOP -> calculateAccumulatorComplex currentValue (consumedOps @ [newOpIdx]) (newOpIdx + 1) program
                | _ -> (false, currentValue)


