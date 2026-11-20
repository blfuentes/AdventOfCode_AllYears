module day14_part02
open System.IO
open System.Collections.Generic
open System.Numerics

type reactElement = { amount: bigint; name: string }

let bigint (x:int) = bigint(x)

let bigint2 (x:decimal) = 
    BigInteger.Parse(System.Math.Round(x, 0).ToString())

let extractTransformation(reaction: string) =
    let operators = reaction.Split('=')
    let componentsPart = operators.[0].Split(',')
    let resultPart = operators.[1].Replace("> ", "").Split(' ')
    let result = { amount = BigInteger.Parse(resultPart.[0]); name = resultPart.[1] }
    let components = seq {
        for comp in componentsPart do
            yield { amount = BigInteger.Parse(comp.Trim().Split(' ').[0]); name = comp.Trim().Split(' ').[1] }
    }
    (components |> Seq.toList, result)

let getORECorrespondent(transformations: seq<list<reactElement>*reactElement>) =
    let oREReactions = 
            transformations |> Seq.filter (fun (reactions, element) ->
                reactions.Length = 1 && reactions.Head.name = "ORE"
            ) |> Seq.map (fun (reactions, element) -> (reactions.Head, element))
    oREReactions

let resetAvailableComponents(transformations: seq<list<reactElement>*reactElement>) =
    let result = new Dictionary<string, bigint>()
    seq {
        for (l, e) in transformations do
            yield (e.name, 0I)
    } |> Seq.iter result.Add
    result

let rec isEnough(transformations: seq<list<reactElement>*reactElement>, consumedElements: Dictionary<string, bigint>, element: reactElement) =
    let isok =
        match consumedElements.[element.name] >= element.amount with
        | true -> true
        | false ->
            match element.name with
            | "ORE" -> false
            | _ ->
                let mutable subOk = true
                let (l, f) = transformations |> Seq.find (fun (a, b) -> b.name = element.name)
                let (fe, cc) = consumedElements.TryGetValue element.name
                let amount = 
                    match fe with
                    | true -> cc
                    | false -> 0I

                let numberOfElements = bigint2(System.Math.Ceiling(((decimal)element.amount - (decimal)(cc)) / (decimal)f.amount))
                for el in l do
                    subOk <- subOk && isEnough(transformations, consumedElements, { amount = numberOfElements * el.amount; name=el.name})
                    consumedElements.[el.name] <- consumedElements.[el.name] - numberOfElements * el.amount
                match subOk with
                | true -> consumedElements.[element.name] <- consumedElements.[element.name] + numberOfElements * f.amount
                | false -> ()
                subOk

    isok

let calculate(transformations: seq<list<reactElement>*reactElement>, availableORE: bigint) =
    let mutable min = 0I
    let mutable top = availableORE
    let mutable consumedElements = new Dictionary<string, bigint>()
    while min < (top - 1I) do
        let midPoint = min + (top - min) / 2I
        consumedElements <- resetAvailableComponents(transformations)
        let (f, ore) = consumedElements.TryGetValue "ORE"
        match f with
        | true -> consumedElements.["ORE"] <- availableORE
        | false -> consumedElements.Add("ORE", availableORE)
        let checkElement = { amount= midPoint; name= "FUEL" }

        match isEnough(transformations, consumedElements, checkElement) with
        | true -> min <- midPoint
        | false -> top <- midPoint - 1I

    match isEnough(transformations, consumedElements, { amount= top; name= "FUEL" }) with
    | true -> top
    | false -> min

let execute =
    let filepath = __SOURCE_DIRECTORY__ + @"../../day14_input.txt"
    //let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_00.txt"
    //let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_01.txt"
    //let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_02.txt"
    //let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_03.txt"
    //let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_04.txt"

    let transformations = File.ReadAllLines(filepath) |> Seq.map extractTransformation
    let amountOfORE = calculate(transformations, 1000000000000I)

    amountOfORE