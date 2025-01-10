// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open AdventOfCode_Utilities

// Define a function to construct a message to print
let from whom =
    sprintf "from %s" whom

let ms ticks =
    let timespan = (TimeSpan.FromTicks ticks)
    sprintf "%02i:%02i.%03i" timespan.Minutes timespan.Seconds timespan.Milliseconds

[<EntryPoint>]
let main argv =
    let timer = new System.Diagnostics.Stopwatch()
    timer.Start()

    // DAY 01
    let (resultday01Part1, time01_1) = Utilities.duration day01_part01.execute
    printfn "Final result Day 01 part 1: %A in %s" resultday01Part1 (ms time01_1)
    let (resultday02Part2, time01_2) = Utilities.duration day01_part02.execute
    printfn "Final result Day 01 part 2: %A in %s" resultday02Part2 (ms time01_2)

    // DAY 02
    let (resultday02Part1, time02_1) = Utilities.duration day02_part01.execute
    printfn "Final result Day 02 part 1: %A in %s" resultday02Part1 (ms time02_1)
    let (resultday02Part2, time02_2) = Utilities.duration day02_part02.execute
    printfn "Final result Day 02 part 2: %A in %s" resultday02Part2 (ms time02_2)

    // DAY 03
    let (resultday03Part1, time03_1) = Utilities.duration day03_part01.execute
    printfn "Final result Day 03 part 1: %A in %s" resultday03Part1 (ms time03_1)
    let (resultday03Part2, time03_2) = Utilities.duration day03_part02.execute
    printfn "Final result Day 03 part 2: %A in %s" resultday03Part2 (ms time03_2)

    // DAY 04
    let (resultday04Part1, time04_1) = Utilities.duration day04_part01.execute
    printfn "Final result Day 04 part 1: %A in %s" resultday04Part1 (ms time04_1)
    let (resultday04Part2, time04_2) = Utilities.duration day04_part02.execute
    printfn "Final result Day 04 part 2: %A in %s" resultday04Part2 (ms time04_2)

    // DAY 05
    let (resultday05Part1, time05_1) = Utilities.duration day05_part01.execute
    printfn "Final result Day 05 part 1: %A in %s" resultday05Part1 (ms time05_1)
    let (resultday05Part2, time05_2) = Utilities.duration day05_part02.execute
    printfn "Final result Day 05 part 2: %A in %s" resultday05Part2 (ms time05_1)

    // DAY 06
    let (resultday06Part1, time06_1) = Utilities.duration day06_part01.execute
    printfn "Final result Day 06 part 1: %A in %s" resultday06Part1 (ms time06_1)
    let (resultday06Part2, time06_2) = Utilities.duration day06_part02.execute
    printfn "Final result Day 06 part 2: %A in %s" resultday06Part2 (ms time06_2)

    // DAY 07
    let (resultday07Part1, time07_1) = Utilities.duration day07_part01.execute
    printfn "Final result Day 07 part 1: %d in %s" resultday07Part1 (ms time07_1)
    let (resultday07Part2, timer07_2) = Utilities.duration day07_part02.execute
    printfn "Final result Day 07 part 2: %d in %s" resultday07Part2 (ms timer07_2)

    // DAY 08
    let (resultday08Part1, time08_1) = Utilities.duration day08_part01.execute
    printfn "Final result Day 08 part 1: %A in %s" resultday08Part1 (ms time08_1)
    let (resultday08Part2, timer08_2) = Utilities.duration day08_part02.execute
    printfn "Final result Day 08 part 2: %A in %s" resultday08Part2 (ms timer08_2)

    // DAY 09
    let (resultday09Part1, time09_1) = Utilities.duration day09_part01.execute
    printfn "Final result Day 09 part 1: %A in %s" resultday09Part1 (ms time09_1)
    let (resultday09Part2, timer09_2) = Utilities.duration day09_part02.execute
    printfn "Final result Day 09 part 2: %d in %s" resultday09Part2 (ms timer09_2)

    // DAY 10
    let (resultday10Part1, time10_1) = Utilities.duration day10_part01.execute
    printfn "Final result Day 10 part 1: %A in %s" resultday10Part1 (ms time10_1)
    let (resultday10Part2, timer10_2) = Utilities.duration day10_part02.execute
    printfn "Final result Day 10 part 2: %A in %s" resultday10Part2 (ms timer10_2)

    // DAY 11
    let (resultday11Part1, time11_1) = Utilities.duration day11_part01.execute
    printfn "Final result Day 11 part 1: %A in %s" resultday11Part1 (ms time11_1)
    let (resultday11Part2, timer11_2) = Utilities.duration day11_part02.execute
    printfn "Final result Day 11 part 2: %d in %s" resultday11Part2 (ms timer11_2)

    // DAY 12
    let (resultday12Part1, time12_1) = Utilities.duration day12_part01.execute
    printfn "Final result Day 12 part 1: %A in %s" resultday12Part1 (ms time12_1)
    let (resultday12Part2, timer12_2) = Utilities.duration day12_part02.execute
    printfn "Final result Day 12 part 2: %A in %s" resultday12Part2 (ms timer12_2)

    // DAY 13
    let (resultday13Part1, time13_1) = Utilities.duration day13_part01.execute
    printfn "Final result Day 13 part 1: %A in %s" resultday13Part1 (ms time13_1)
    let (resultday13Part2, timer13_2) = Utilities.duration day13_part02.execute
    printfn "Final result Day 13 part 2: %d in %s" resultday13Part2 (ms timer13_2)

    // DAY 14
    let (resultday14Part1, time14_1) = Utilities.duration day14_part01.execute
    printfn "Final result Day 14 part 1: %A in %s" resultday14Part1 (ms time14_1)
    let (resultday14Part2, timer14_2) = Utilities.duration day14_part02.execute
    printfn "Final result Day 14 part 2: %A in %s" resultday14Part2 (ms timer14_2)

    // DAY 15
    let (resultday15Part1, time15_1) = Utilities.duration day15_part01.execute
    printfn "Final result Day 15 part 1: %A in %s" resultday15Part1 (ms time15_1)
    let (resultday15Part2, timer15_2) = Utilities.duration day15_part02.execute
    printfn "Final result Day 15 part 2: %A in %s" resultday15Part2 (ms timer15_2)

    // DAY 16
    let (resultday16Part1, time16_1) = Utilities.duration day16_part01.execute
    printfn "Final result Day 16 part 1: %A in %s" resultday16Part1 (ms time16_1)
    let (resultday16Part2, timer16_2) = Utilities.duration day16_part02.execute
    printfn "Final result Day 16 part 2: %A in %s" resultday16Part2 (ms timer16_2)

    // DAY 17
    let (resultday17Part1, time17_1) = Utilities.duration day17_part01.execute
    printfn "Final result Day 17 part 1: %A in %s" resultday17Part1 (ms time17_1)
    let (resultday17Part2, timer17_2) = Utilities.duration day17_part02.execute
    printfn "Final result Day 17 part 2: %d in %s" resultday17Part2 (ms timer17_2)

    // DAY 18
    let (resultday18Part1, time18_1) = Utilities.duration day18_part01.execute
    printfn "Final result Day 18 part 1: %A in %s" resultday18Part1 (ms time18_1)
    let (resultday18Part2, timer18_2) = Utilities.duration day18_part02.execute
    printfn "Final result Day 18 part 2: %A in %s" resultday18Part2 (ms timer18_2)

    // DAY 19
    let (resultday19Part1, time19_1) = Utilities.duration day19_part01.execute
    printfn "Final result Day 19 part 1: %A in %s" resultday19Part1 (ms time19_1)
    let (resultday19Part2, timer19_2) = Utilities.duration day19_part02.execute
    printfn "Final result Day 19 part 2: %d in %s" resultday19Part2 (ms timer19_2)

    // DAY 20
    let (resultday20Part1, time20_1) = Utilities.duration day20_part01.execute
    printfn "Final result Day 20 part 1: %A in %s" resultday20Part1 (ms time20_1)
    let (resultday20Part2, timer20_2) = Utilities.duration day20_part02.execute
    printfn "Final result Day 20 part 2: %A in %s" resultday20Part2 (ms timer20_2)

    // DAY 21
    let (resultday21Part1, time21_1) = Utilities.duration day21_part01.execute
    printfn "Final result Day 21 part 1: %A in %s" resultday21Part1 (ms time21_1)
    let (resultday21Part2, timer21_2) = Utilities.duration day21_part02.execute
    printfn "Final result Day 21 part 2: %d in %s" resultday21Part2 (ms timer21_2)

    // DAY 22
    let (resultday22Part1, time22_1) = Utilities.duration day22_part01.execute
    printfn "Final result Day 22 part 1: %d in %s" resultday22Part1 (ms time22_1)
    let (resultday22Part2, timer22_2) = Utilities.duration day22_part02.execute
    printfn "Final result Day 22 part 2: %d in %s" resultday22Part2 (ms timer22_2)

    // DAY 23
    let (resultday23Part1, time23_1) = Utilities.duration day23_part01.execute
    printfn "Final result Day 23 part 1: %A in %s" resultday23Part1 (ms time23_1)
    let (resultday23Part2, timer23_2) = Utilities.duration day23_part02.execute
    printfn "Final result Day 23 part 2: %A in %s" resultday23Part2 (ms timer23_2)

    // DAY 24
    let (resultday24Part1, time24_1) = Utilities.duration day24_part01.execute
    printfn "Final result Day 24 part 1: %d in %s" resultday24Part1 (ms time24_1)
    let (resultday24Part2, timer24_2) = Utilities.duration day24_part02.execute
    printfn "Final result Day 24 part 2: %A in %s" resultday24Part2 (ms timer24_2)

    // DAY 25
    let (resultday25Part1, time25_1) = Utilities.duration day25_part01.execute
    printfn "Final result Day 25 part 1: %A in %s" resultday25Part1 (ms time25_1)

    timer.Stop()
    let timespan = (TimeSpan.FromTicks timer.ElapsedTicks)
    printfn "Total execution: %02i:%02i.%03i" timespan.Minutes timespan.Seconds timespan.Milliseconds

    //
    let endprogram = Console.ReadLine()
    0 // return an integer exit code