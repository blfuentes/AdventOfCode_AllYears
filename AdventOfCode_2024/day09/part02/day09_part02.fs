module day09_part02

open AdventOfCode_2024.Modules

type DiskFile = {
    mutable FileId: int;
    mutable Space: int;
}

let parseContent2(line: string) =
    let mutable fileIdx = 0
    line.ToCharArray()
    |> Array.mapi (fun cIdx v ->
        let size = System.Int32.Parse(string(v))
        if cIdx % 2 = 0 then
            let discfile = { FileId = fileIdx; Space = size }
            fileIdx <- fileIdx + 1
            discfile
        else
            { FileId = -1; Space = size }
    )

let compactFiles (files: DiskFile array) =
    let mutable fileIdx = files.Length - 1
    let mutable gapIdx = 0
    let mutable firstGap = 0
    let mutable tmpfiles = files
    while fileIdx > gapIdx do
        if tmpfiles[fileIdx].FileId <> -1 then
            while gapIdx < fileIdx && (tmpfiles[gapIdx].FileId <> -1 || tmpfiles[gapIdx].Space < tmpfiles[fileIdx].Space) do
                gapIdx <- gapIdx + 1
            if gapIdx < fileIdx then
                let insertedFile = { FileId = tmpfiles[fileIdx].FileId; Space = tmpfiles[fileIdx].Space }
                tmpfiles[gapIdx].Space <- tmpfiles[gapIdx].Space - insertedFile.Space
                
                let before, after = Array.splitAt gapIdx tmpfiles
                tmpfiles <- Array.concat [before; [|insertedFile|]; after]
                
                tmpfiles[fileIdx+1].FileId <- -1
            
                let delPos = gapIdx + 1
                if tmpfiles[delPos].Space = 0 then
                    tmpfiles <- tmpfiles |> Array.removeAt delPos
        fileIdx <- fileIdx - 1
        if gapIdx <> 0 then
            gapIdx <- firstGap
            let mutable exit = false
            while not exit && gapIdx < fileIdx do
                if files[gapIdx].FileId = -1 && files[gapIdx].Space > 0 then
                    firstGap <- gapIdx
                    exit <- true
                if not exit then
                    gapIdx <- gapIdx + 1
    tmpfiles

let buildChecksum(files: DiskFile array) =
    files
    |> Array.filter(fun f -> f.Space > 0)
    |> Array.collect(fun sub ->
        Array.init sub.Space (fun _ -> sub.FileId)
    )  
    |> Array.indexed
    |> Array.sumBy(fun (idx, v) -> 
        if v > 0 then
            (int64)(idx * v)
        else
            (int64)0
    )

let execute() =
    let path = "day09/day09_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let files = parseContent2 content
    let compacted = compactFiles files
    buildChecksum compacted
