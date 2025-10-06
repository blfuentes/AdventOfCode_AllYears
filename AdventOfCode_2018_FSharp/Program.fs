// Learn more about F# at http://fsharp.org

open System

[<EntryPoint>]
let main argv =

    // DAY 01
    let resultday01Part1 = day01_part01.displayValue
    printfn "Final result Day 01 part 1: %i" resultday01Part1
    let resultday01Part2 = day01_part02.calculateDuplicatedFrecuency
    printfn "Final result Day 01 part 2: %i" resultday01Part2

    // DAY 02
    let resultday02Part1 = day02_part01.finalValue
    printfn "Final result Day 02 part 1: %i" resultday02Part1
    let resultday02Part2 = day02_part02.resolve
    printfn "Final result Day 02 part 2: %s" resultday02Part2

    // DAY 06
    let resultday06Part1 = day06_part01.execute
    printfn "Final result Day 06 part 1: %i" resultday06Part1
    let resultday06Part2 = day06_part02.execute
    printfn "Final result Day 06 part 2: %i" resultday06Part2

    // DAY 07
    let resultday07Part1 = day07_part01.execute
    printfn "Final result Day 07 part 1: %i" resultday07Part1
    let resultday07Part2 = day07_part02.execute
    printfn "Final result Day 07 part 2: %i" resultday07Part2

    // DAY 17
    let resultday17Part1 = day17_part01.execute
    printfn "Final result Day 17 part 1: %i" resultday17Part1
    let resultday17Part2 = day17_part02.execute
    printfn "Final result Day 17 part 2: %i" resultday17Part2

    // DAY 18
    let resultday18Part1 = day18_part01.execute
    printfn "Final result Day 18 part 1: %i" resultday18Part1
    let resultday18Part2 = day18_part02.execute
    printfn "Final result Day 18 part 2: %i" resultday18Part2

    // DAY 19
    let resultday19Part1 = day19_part01.execute
    printfn "Final result Day 19 part 1: %i" resultday19Part1
    let resultday19Part2 = day19_part02.execute
    printfn "Final result Day 19 part 2: %i" resultday19Part2

    // DAY 20
    let resultday20Part1 = day20_part01.execute
    printfn "Final result Day 20 part 1: %A" resultday20Part1
    let resultday20Part2 = day20_part02.execute
    printfn "Final result Day 20 part 2: %i" resultday20Part2

    // DAY 21
    let resultday21Part1 = day21_part01.execute
    printfn "Final result Day 21 part 1: %i" resultday21Part1
    let resultday21Part2 = day21_part02.execute
    printfn "Final result Day 21 part 2: %i" resultday21Part2

    // DAY 22
    let resultday22Part1 = day22_part01.execute
    printfn "Final result Day 22 part 1: %i" resultday22Part1
    let resultday22Part2 = day22_part02.execute
    printfn "Final result Day 22 part 2: %i" resultday22Part2

    // DAY 23
    let resultday23Part1 = day23_part01.execute
    printfn "Final result Day 23 part 1: %i" resultday23Part1
    let resultday23Part2 = day23_part02.execute
    printfn "Final result Day 23 part 2: %i" resultday23Part2

    // DAY 24
    let resultday24Part1 = day24_part01.execute
    printfn "Final result Day 24 part 1: %i" resultday24Part1
    let resultday24Part2 = day24_part02.execute
    printfn "Final result Day 24 part 2: %i" resultday24Part2

    // DAY 25
    let resultday25Part1 = day25_part01.execute
    printfn "Final result Day 25 part 1: %i" resultday25Part1

    //
    let endprogram = Console.ReadLine()
    0 // return an integer exit code
