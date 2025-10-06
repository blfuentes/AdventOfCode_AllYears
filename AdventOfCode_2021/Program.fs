// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open AdventOfCode_Utilities

[<EntryPoint>]
let main argv =
    // DAY 01
    Utilities.duration(fun () ->
        let resultday01Part1 = day01_part01.execute
        printfn "Final result Day 01 part 1: %i" resultday01Part1
    ) |> ignore
    Utilities.duration(fun () ->
        let resultDay01Part2 = day01_part02.execute
        printfn "Final result Day 01 part 2: %i" resultDay01Part2
    ) |> ignore

    // DAY 02
    Utilities.duration(fun () ->
        let resultday02Part1 = day02_part01.execute
        printfn "Final result Day 02 part 1: %i" resultday02Part1
    ) |> ignore
    Utilities.duration(fun () ->
        let resultday02Part2 = day02_part02.execute
        printfn "Final result Day 02 part 2: %i" resultday02Part2
    ) |> ignore

    // DAY 03
    Utilities.duration(fun () ->
        let resultday03Part1 = day03_part01.execute
        printfn "Final result Day 03 part 1: %i" resultday03Part1
    ) |> ignore
    Utilities.duration(fun () ->
        let resultday03Part2 = day03_part02.execute
        printfn "Final result Day 03 part 2: %i" resultday03Part2
    ) |> ignore

    // DAY 04
    Utilities.duration(fun () ->
        let resultday04Part1 = day04_part01.execute
        printfn "Final result Day 04 part 1: %i" resultday04Part1
    ) |> ignore
    Utilities.duration(fun () ->
        let resultday04Part2 = day04_part02.execute
        printfn "Final result Day 04 part 2: %i" resultday04Part2
    ) |> ignore

    // DAY 05
    Utilities.duration(fun () ->
        let resultday05Part1 = day05_part01.execute
        printfn "Final result Day 05 part 1: %i" resultday05Part1
    ) |> ignore
    Utilities.duration(fun () ->
        let resultday05Part2 = day05_part02.execute
        printfn "Final result Day 05 part 2: %i" resultday05Part2
    ) |> ignore

    // DAY 06
    Utilities.duration(fun () ->
        let resultday06Part1 = day06_part01.execute
        printfn "Final result Day 06 part 1: %i" resultday06Part1
    ) |> ignore
    Utilities.duration(fun () ->
        let resultday06Part2 = day06_part02.execute
        printfn "Final result Day 06 part 2: %A" resultday06Part2
    ) |> ignore

    // DAY 07
    Utilities.duration(fun () ->
        let resultday07Part1 = day07_part01.execute
        printfn "Final result Day 07 part 1: %i" resultday07Part1
    ) |> ignore
    Utilities.duration(fun () ->
        let resultday07Part2 = day07_part02.execute
        printfn "Final result Day 07 part 2: %i" resultday07Part2
    ) |> ignore

    // DAY 08
    Utilities.duration(fun () ->
        let resultday08Part1 = day08_part01.execute
        printfn "Final result Day 08 part 1: %i" resultday08Part1
    ) |> ignore
    Utilities.duration(fun () ->
        let resultday08Part2 = day08_part02.execute
        printfn "Final result Day 08 part 2: %i" resultday08Part2
    ) |> ignore

    // DAY 09
    Utilities.duration(fun () ->
        let resultday09Part1 = day09_part01.execute 
        printfn "Final result Day 09 part 1: %i" resultday09Part1   
    ) |> ignore
    Utilities.duration(fun () ->
        let resultday09Part2 = day09_part02.execute
        printfn "Final result Day 09 part 2: %i" resultday09Part2
    ) |> ignore

    // DAY 10
    Utilities.duration(fun () ->
        let resultday10Part1 = day10_part01.execute
        printfn "Final result Day 10 part 1: %A" resultday10Part1
    ) |> ignore
    Utilities.duration(fun () ->
        let resultday10Part2 = day10_part02.execute
        printfn "Final result Day 10 part 2: %A" resultday10Part2
    ) |> ignore

    // DAY 11
    Utilities.duration(fun () ->
        let resultday11Part1 = day11_part01.execute
        printfn "Final result Day 11 part 1: %i" resultday11Part1
    ) |> ignore
    Utilities.duration(fun () ->
        let resultday11Part2 = day11_part02.execute
        printfn "Final result Day 11 part 2: %i" resultday11Part2
    ) |> ignore

    // DAY 12
    Utilities.duration(fun () ->
        let resultday12Part1 = day12_part01.execute
        printfn "Final result Day 12 part 1: %i" resultday12Part1
    ) |> ignore
    Utilities.duration(fun () ->
        let resultday12Part2 = day12_part02.execute
        printfn "Final result Day 12 part 2: %i" resultday12Part2
    ) |> ignore

    // DAY 13
    Utilities.duration(fun () ->
        let resultday13Part1 = day13_part01.execute
        printfn "Final result Day 13 part 1: %i" resultday13Part1
    ) |> ignore
    Utilities.duration(fun () ->
        printfn "Final result Day 13 part 2:"
        day13_part02.execute
    ) |> ignore

    // DAY 14
    Utilities.duration(fun () ->
        let resultday14Part1 = day14_part01.execute
        printfn "Final result Day 14 part 1: %i" resultday14Part1
    ) |> ignore
    Utilities.duration(fun () ->
        let resultday14Part2 = day14_part02.execute
        printfn "Final result Day 14 part 2: %A" resultday14Part2
    ) |> ignore

    // DAY 15
    Utilities.duration(fun () ->
        let resultday15Part1 = day15_part01.execute
        printfn "Final result Day 15 part 1: %i" resultday15Part1
    ) |> ignore
    Utilities.duration(fun () ->
        let resultday15Part2 = day15_part02.execute
        printfn "Final result Day 15 part 2: %i" resultday15Part2
    ) |> ignore

    // DAY 16
    Utilities.duration(fun () ->
        let resultday16Part1 = day16_part01.execute
        printfn "Final result Day 16 part 1: %A" resultday16Part1
    ) |> ignore
    Utilities.duration(fun () ->
        let resultday16Part2 = day16_part02.execute
        printfn "Final result Day 16 part 2: %A" resultday16Part2
    ) |> ignore

    // DAY 17
    Utilities.duration(fun () ->
        let resultday17Part1 = day17_part01.execute
        printfn "Final result Day 17 part 1: %i" resultday17Part1
    ) |> ignore
    Utilities.duration(fun () ->
        let resultday17Part2 = day17_part02.execute
        printfn "Final result Day 17 part 2: %i" resultday17Part2
    ) |> ignore

    // DAY 18
    Utilities.duration(fun () ->
        let resultday18Part1 = day18_part01.execute
        printfn "Final result Day 18 part 1: %A" resultday18Part1
    ) |> ignore
    Utilities.duration(fun () ->
        let resultday18Part2 = day18_part02.execute
        printfn "Final result Day 18 part 2: %A" resultday18Part2
    ) |> ignore

    // DAY 19
    Utilities.duration(fun () ->
        let resultday19Part1 = day19_part01.execute
        printfn "Final result Day 19 part 1: %A" resultday19Part1
    ) |> ignore
    Utilities.duration(fun () ->
        let resultday19Part2 = day19_part02.execute
        printfn "Final result Day 19 part 2: %A" resultday19Part2
    ) |> ignore

    // DAY 20
    Utilities.duration(fun () ->
        let resultday20Part1 = day20_part01.execute
        printfn "Final result Day 20 part 1: %A" resultday20Part1
    ) |> ignore
    Utilities.duration(fun () ->
        let resultday20Part2 = day20_part02.execute
        printfn "Final result Day 20 part 2: %A" resultday20Part2
    ) |> ignore

    // DAY 21
    Utilities.duration(fun () ->
        let resultday21Part1 = day21_part01.execute
        printfn "Final result Day 21 part 1: %A" resultday21Part1
    ) |> ignore
    Utilities.duration(fun () ->
        let resultday21Part2 = day21_part02.execute
        printfn "Final result Day 21 part 2: %d" resultday21Part2
    ) |> ignore

    // DAY 22
    Utilities.duration(fun () ->
        let resultday22Part1 = day22_part01.execute
        printfn "Final result Day 22 part 1: %d" resultday22Part1
    ) |> ignore
    Utilities.duration(fun () ->
        let resultday22Part2 = day22_part02.execute
        printfn "Final result Day 22 part 2: %d" resultday22Part2
    ) |> ignore

    // DAY 23
    Utilities.duration(fun () ->
        let resultday23Part1 = day23_part01.execute
        printfn "Final result Day 23 part 1: %d" resultday23Part1
    ) |> ignore
    Utilities.duration(fun () ->
        let resultday23Part2 = day23_part02.execute
        printfn "Final result Day 23 part 2: %d" resultday23Part2
    ) |> ignore

    // DAY 24
    Utilities.duration(fun () ->
        let resultday24Part1 = day24_part01.execute
        printfn "Final result Day 24 part 1: %s" resultday24Part1
    ) |> ignore
    Utilities.duration(fun () ->
        let resultday24Part2 = day24_part02.execute
        printfn "Final result Day 24 part 2: %s" resultday24Part2
    ) |> ignore

    // DAY 25
    Utilities.duration(fun () ->
        let resultday25Part1 = day25_part01.execute
        printfn "Final result Day 25 part 1: %d" resultday25Part1
    ) |> ignore

    //
    let endprogram = Console.ReadLine()
    0 // return an integer exit code