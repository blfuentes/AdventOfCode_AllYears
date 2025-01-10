module day14_part02

open AdventOfCode_2017.Modules
open System.Collections.Generic

let hexToBinary hexDigit =
    let hexCharToInt c =
        match c with
        | '0' -> 0 | '1' -> 1 | '2' -> 2 | '3' -> 3
        | '4' -> 4 | '5' -> 5 | '6' -> 6 | '7' -> 7
        | '8' -> 8 | '9' -> 9
        | 'a' | 'A' -> 10 | 'b' | 'B' -> 11
        | 'c' | 'C' -> 12 | 'd' | 'D' -> 13
        | 'e' | 'E' -> 14 | 'f' | 'F' -> 15
        | _ -> failwith "Invalid hexadecimal digit"
    let value = hexCharToInt hexDigit
    System.Convert.ToString(value, 2).PadLeft(4, '0')

let buildRows(input: string) =
    let map = Array2D.create 128 128 0
    let used = HashSet<(int*int)>()
    [0..127]
    |> List.iter(fun row ->
        let knothash = String.concat "" ((LocalHelper.knotHasher $"{input}-{row}").ToCharArray() |> Array.map(fun c -> hexToBinary c))
        knothash.ToCharArray() 
        |> Array.iteri(fun col v -> 
            if v = '1' then 
                let _ = used.Add((row, col))
                ()
            else
                ()
        )
    )
    (map, used)

let countRegions(map: int[,]) (used: HashSet<(int*int)>)=
    let mutable rId = 1
    let (maxrows, maxcols) = (map.GetLength(0), map.GetLength(1))

    let visited = HashSet<(int*int)>()

    let floodfill startRow startCol =
        let stack = System.Collections.Generic.Stack<int * int>()
        stack.Push(startRow, startCol)
    
        while stack.Count > 0 do
            let r, c = stack.Pop()
    
            if r >= 0 && c >= 0 && r < maxrows && c < maxcols && used.Contains((r,c)) && not(visited.Contains(r, c)) then
                map[r, c] <- rId
                let _ = visited.Add((r, c))
                // Push neighbors onto the stack
                stack.Push(r - 1, c) // Up
                stack.Push(r + 1, c) // Down
                stack.Push(r, c - 1) // Left
                stack.Push(r, c + 1) // Right

        rId <- rId + 1

    for row in 0..maxrows-1 do
        for col in 0..maxcols-1 do
            if used.Contains((row, col)) && not (visited.Contains(row, col)) then
                floodfill row col

    rId - 1
            
let execute() =
    let path = "day14/day14_input.txt"
    let content = LocalHelper.GetContentFromFile path

    let (map, used) = buildRows content
    countRegions map used