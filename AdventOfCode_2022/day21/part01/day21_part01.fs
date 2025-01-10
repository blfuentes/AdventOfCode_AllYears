module day21_part01

open AdventOfCode_2022.Modules
open AdventOfCode_Utilities

type Operation =
    | Add
    | Sub
    | Mul
    | Div
    | Eq

type Monkey =
    | NumberMonkey of int64
    | OperationMonkey of Operation * string * string

type HalfMonkey =
    | Int of int64
    | Source of string

let parseOp =
    function
    | "*" -> Mul
    | "+" -> Add
    | "-" -> Sub
    | "/" -> Div
    | "=" -> Eq
    | _ -> failwith "Invalid operation"

let parse line =
    match line with
    | Regex @"^([a-z]{4}): (\d+)" [ mId; n ] -> mId, NumberMonkey(int64 n)
    | Regex @"^([a-z]{4}): ([a-z]{4}) (.) ([a-z]{4})" [ mId; mA; op; mB ] -> mId, OperationMonkey(parseOp op, mA, mB)

let calculate op (a: int64) (b: int64) =
    match op with
    | Add -> (+) a b
    | Mul -> (*) a b
    | Sub -> (-) a b
    | Div -> (/%?) a b
    | Eq ->
        (=) a b
        |> function
            | false -> 0L
            | true -> 1L

let revOp op (a: int64) (b: int64) =
    match op with
    | Sub -> (+) a b
    | Div -> (*) a b
    | Add -> (-) a b
    | Mul -> (/) a b

let revOpName =
    function
    | Div -> "*"
    | Add -> "-"
    | Mul -> "/"
    | Sub -> "+"

let doMonkey (numberMonkeys: Map<string, Monkey>) opMonkey =
    let n =
        match opMonkey with
        | OperationMonkey (o, ma, mb) ->
            let (NumberMonkey a) = numberMonkeys.[ma]
            let (NumberMonkey b) = numberMonkeys.[mb]
            calculate o a b
        | _ -> failwith "Invalid monkey"

    NumberMonkey n

let solvePart2 numberMonkeys opMonkeys =
    let monkeyChain =
        opMonkeys
        |> Seq.map (fun (i, m) ->
            let (OperationMonkey (op, ma, mb)) = m
            let am = numberMonkeys |> Map.tryFind ma
            let bm = numberMonkeys |> Map.tryFind mb
            (*
            
            match am with
            | None -> ()
            | Some x -> printfn $"%A{op} %A{x} %A{mb}"

            match bm with
            | None -> ()
            | Some x -> printfn $"%A{op} %A{ma} %A{x}"
            *)
            match am, bm with
            | Some (NumberMonkey n), None -> i, (op, Source mb, Int n)
            | None, Some (NumberMonkey n) -> i, (op, Int n, Source ma))

    let monkeys = monkeyChain |> Map.ofSeq
    printfn "%A" monkeys
    let _, Int final, Source next = monkeys.["root"]
    printfn "%A" monkeys.["root"]

    let rec unChain (chain: Map<string, Operation * HalfMonkey * HalfMonkey>) acc next =
        match next with
        | "humn" -> acc
        | _ ->
            let next, acc' =
                match chain.[next] with
                | Div, Source other, Int n ->
                    printfn $"%s{other} %d{n} / %d{acc}"
                    other, n / acc
                | Div, Int n, Source other ->
                    printfn $"%s{other} %d{n} * %d{acc}"
                    other, n * acc
                | Sub, Int n, Source other ->
                    printfn $"%s{other} %d{n} + %d{acc}"
                    other, acc + n
                | Sub, Source other, Int n ->
                    printfn $"%s{other} %d{n} - %d{acc}"
                    other, acc - n
                | o, Int n, Source other -> other, (revOp o) n acc
                | o, Source other, Int n -> other, (revOp o) acc n
                | _ -> failwith "Problem with half-monkeys"

            // printfn $"%d{left} %s{revOpName op} %d{right} = %d{acc'} (%s{next})"
            unChain chain acc' next

    unChain monkeys final next

let rec shout (numberMonkeys: Map<string, Monkey>) (opMonkeys: Set<string * Monkey>) =

    let readyMonkeys =
        opMonkeys
        |> Set.filter (fun (_, m) ->
            match m with
            | NumberMonkey _ -> false
            | OperationMonkey (_, a, b) ->
                Map.containsKey a numberMonkeys
                && Map.containsKey b numberMonkeys)

    // readyMonkeys |> printfn "%A"
    let opMonkeys' = Set.difference opMonkeys readyMonkeys

    let newNumberMonkeys =
        readyMonkeys
        |> Set.map (fun (i, m) -> i, doMonkey numberMonkeys m)
        |> Map.ofSeq

    let numberMonkeys' =
        Map.fold (fun acc k v -> Map.add k v acc) numberMonkeys newNumberMonkeys

    match readyMonkeys |> Set.isEmpty with
    | true ->
        match Map.tryFind "root" numberMonkeys' with
        | Some (NumberMonkey m) -> m
        | None -> solvePart2 numberMonkeys' opMonkeys'
        | _ -> failwith "Invalid root monkey"
    | false -> shout numberMonkeys' opMonkeys'

type MonkeyType =
    | Number
    | Operation

let parseContent(lines: string array) =
    let input = lines |> Seq.map parse

    let monkeyMap =
        input
        |> Seq.groupBy (fun (_, m) ->
            match m with
            | NumberMonkey _ -> Number
            | OperationMonkey _ -> Operation)
        |> Map.ofSeq

    Map.ofSeq monkeyMap.[Number], Set.ofSeq monkeyMap.[Operation]

let part1 (numberMonkeys, opMonkeys) =
    shout numberMonkeys opMonkeys |> int64

let execute =
    let path = "day21/day21_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let (numberMonkeys, opMonkeys) = parseContent content
    part1 (numberMonkeys, opMonkeys)