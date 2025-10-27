// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open AdventOfCode_Utilities

[<EntryPoint>]
let main argv =
    let timer = new System.Diagnostics.Stopwatch()
    timer.Start()

    // DAY 01
    let (resultday01Part1, time01_1) = Utilities.duration day01_part01.execute
    printfn "Final result Day 01 part 1: %A in %s" resultday01Part1 (Utilities.ms time01_1)
    let (resultday01Part2, time01_2) = Utilities.duration day01_part02.execute
    printfn "Final result Day 01 part 2: %A in %s" resultday01Part2 (Utilities.ms time01_2)

    // DAY 02
    let (resultday02Part1, time02_1) = Utilities.duration day02_part01.execute
    printfn "Final result Day 02 part 1: %A in %s" resultday02Part1 (Utilities.ms time02_1)
    let (resultday02Part2, time02_2) = Utilities.duration day02_part02.execute
    printfn "Final result Day 02 part 2: %A in %s" resultday02Part2 (Utilities.ms time02_2)

    // DAY 03
    let (resultday03Part1, time03_1) = Utilities.duration day03_part01.execute
    printfn "Final result Day 03 part 1: %A in %s" resultday03Part1 (Utilities.ms time03_1)
    let (resultday03Part2, time03_2) = Utilities.duration day03_part02.execute
    printfn "Final result Day 03 part 2: %A in %s" resultday03Part2 (Utilities.ms time03_2)

    // DAY 04
    let (resultday04Part1, time04_1) = Utilities.duration day04_part01.execute
    printfn "Final result Day 04 part 1: %A in %s" resultday04Part1 (Utilities.ms time04_1)
    let (resultday04Part2, time04_2) = Utilities.duration day04_part02.execute
    printfn "Final result Day 04 part 2: %A in %s" resultday04Part2 (Utilities.ms time04_2)

    // DAY 05
    let (resultday05Part1, time05_1) = Utilities.duration day05_part01.execute
    printfn "Final result Day 05 part 1: %A in %s" resultday05Part1 (Utilities.ms time05_1)
    let (resultday05Part2, time05_2) = Utilities.duration day05_part02.execute
    printfn "Final result Day 05 part 2: %A in %s" resultday05Part2 (Utilities.ms time05_1)

    // DAY 06
    let (resultday06Part1, time06_1) = Utilities.duration day06_part01.execute
    printfn "Final result Day 06 part 1: %A in %s" resultday06Part1 (Utilities.ms time06_1)
    let (resultday06Part2, time06_2) = Utilities.duration day06_part02.execute
    printfn "Final result Day 06 part 2: %A in %s" resultday06Part2 (Utilities.ms time06_2)

    // DAY 07
    let (resultday07Part1, time07_1) = Utilities.duration day07_part01.execute
    printfn "Final result Day 07 part 1: %d in %s" resultday07Part1 (Utilities.ms time07_1)
    let (resultday07Part2, timer07_2) = Utilities.duration day07_part02.execute
    printfn "Final result Day 07 part 2: %d in %s" resultday07Part2 (Utilities.ms timer07_2)

    // DAY 08
    let (resultday08Part1, time08_1) = Utilities.duration day08_part01.execute
    printfn "Final result Day 08 part 1: %A in %s" resultday08Part1 (Utilities.ms time08_1)
    let (resultday08Part2, timer08_2) = Utilities.duration day08_part02.execute
    printfn "Final result Day 08 part 2: %A in %s" resultday08Part2 (Utilities.ms timer08_2)

    // DAY 09
    let (resultday09Part1, time09_1) = Utilities.duration day09_part01.execute
    printfn "Final result Day 09 part 1: %A in %s" resultday09Part1 (Utilities.ms time09_1)
    let (resultday09Part2, timer09_2) = Utilities.duration day09_part02.execute
    printfn "Final result Day 09 part 2: %d in %s" resultday09Part2 (Utilities.ms timer09_2)

    // DAY 10
    let (resultday10Part1, time10_1) = Utilities.duration day10_part01.execute
    printfn "Final result Day 10 part 1: %A in %s" resultday10Part1 (Utilities.ms time10_1)
    let (resultday10Part2, timer10_2) = Utilities.duration day10_part02.execute
    printfn "Final result Day 10 part 2: %A in %s" resultday10Part2 (Utilities.ms timer10_2)

    // DAY 11
    let (resultday11Part1, time11_1) = Utilities.duration day11_part01.execute
    printfn "Final result Day 11 part 1: %A in %s" resultday11Part1 (Utilities.ms time11_1)
    let (resultday11Part2, timer11_2) = Utilities.duration day11_part02.execute
    printfn "Final result Day 11 part 2: %d in %s" resultday11Part2 (Utilities.ms timer11_2)

    // DAY 12
    let (resultday12Part1, time12_1) = Utilities.duration day12_part01.execute
    printfn "Final result Day 12 part 1: %A in %s" resultday12Part1 (Utilities.ms time12_1)
    let (resultday12Part2, timer12_2) = Utilities.duration day12_part02.execute
    printfn "Final result Day 12 part 2: %A in %s" resultday12Part2 (Utilities.ms timer12_2)

    timer.Stop()
    let timespan = (TimeSpan.FromTicks timer.ElapsedTicks)
    printfn "Total execution: %02i:%02i.%03i" timespan.Minutes timespan.Seconds timespan.Milliseconds

    //
    let endprogram = Console.ReadLine()
    0 // return an integer exit code