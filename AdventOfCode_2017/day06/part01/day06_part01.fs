module day06_part01



let hasBeenSeen (banks: int[]) (seen: int array list) =
    seen |> List.exists(fun s -> Array.compareWith(fun x y -> x - y) s banks = 0)

let rec distributeBlocks(banks: int[]) (index: int) (blocks: int) (empty: bool) =
    if blocks = 0 then banks
    else
        let mutable numberOfBlocksLeft = blocks
        let banksCopy = Array.copy banks
        if empty then banksCopy.[index - 1] <- 0 else ()
        for idx = index to banks.Length - 1 do
            if numberOfBlocksLeft > 0 then
                banksCopy.[idx] <- banksCopy.[idx] + 1
                numberOfBlocksLeft <- numberOfBlocksLeft - 1
            else
                ()
            
        distributeBlocks banksCopy 0 numberOfBlocksLeft false

let rec doCycle (banks: int[]) (cycles: int) (seen: int array list) =
    let max = banks |> Array.max
    let maxIndex = banks |> Array.findIndex (fun x -> x = max)
    let newBanks = distributeBlocks banks (maxIndex + 1) max true
    if hasBeenSeen newBanks seen then (cycles + 1)
    else
        let newSeen = seen @ [newBanks]
        doCycle newBanks (cycles + 1) newSeen

let execute() =
    let path = "day06/day06_input.txt"
    let inputBanks = (System.IO.File.ReadLines path|> Seq.toArray |> Array.head).Split('\t') |> Array.map int    
    doCycle inputBanks 0 []