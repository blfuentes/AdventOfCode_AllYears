module day15_part01

open AdventOfCode_2017.Modules

type Generator = {
    Factor: int64
    CurrentValue: int64
}

let parseContent(lines: string array) =
    let (startA, startB) = ((int64)(lines[0].Split(" ")[4]), (int64)(lines[1].Split(" ")[4]))
    (startA, startB)

let nextValue(generator: Generator) =
    { generator with CurrentValue = (generator.CurrentValue * generator.Factor) % 2147483647L }
   
let judgeValues(counter: int) ((generatorA, generatorB) : Generator * Generator) =
    let mutable total = 0
    let rec generate c gA gB =
        match c = counter with
        | true -> (generatorA, generatorB)
        | false ->
            let newA = nextValue gA
            let newB = nextValue gB
            if (newA.CurrentValue &&& 0xFFFF) = (newB.CurrentValue &&& 0xFFFF) then
                total <- total + 1
            generate (c + 1) newA newB

    let _ = generate 0 generatorA generatorB
    total

let execute() =
    let path = "day15/day15_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let (startA, startB) = parseContent content

    let comparisons = 40000000

    let(factorA, factorB) = (16807L, 48271L)
    let generatorA = { Factor = factorA; CurrentValue = startA }
    let generatorB = { Factor = factorB; CurrentValue = startB }
    judgeValues comparisons (generatorA, generatorB)
    