namespace AdventOfCode_2025.Modules

open System.IO

module LocalHelper =
    let GetLinesFromFile(path: string) =
        File.ReadAllLines(__SOURCE_DIRECTORY__ + @"../../" + path)

    let GetContentFromFile(path: string) =
        File.ReadAllText(__SOURCE_DIRECTORY__ + @"../../" + path)

    let ReadLines(path: string) =
        File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path)