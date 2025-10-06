namespace AdventOfCode_Utilities

open System
open System.Text.RegularExpressions
open System.Globalization
open System.Diagnostics
open System.Collections.Generic

[<AutoOpen>]
module Utilities = 
    
    module Seq =
        let repeatForever s =
            let c = Seq.cache s
    
            seq {
                while true do
                    yield! c
            }
    
        let filteri f =
            Seq.mapi (fun i v -> (i, v))
            >> Seq.filter (fun v -> f (fst v) (snd v))
            >> Seq.map snd
    
        let takeWhilePlus1 predicate s =
            seq {
                yield! Seq.takeWhile predicate s
                yield! s |> Seq.skipWhile predicate |> Seq.truncate 1
            }

    ///////////////////////////////////////////////////////////////////////////////////
    let parseRegex regex map s =  
        Regex.Match(s,regex) |> fun m -> m.Groups
        |> Seq.skip 1 // ignore first group
        |> Seq.map (fun a -> a.Value) |> Array.ofSeq |> map

    let both f g x = (f x, g x)

    let manhattan (x,y) (x',y') = abs (x-x') + abs (y-y')

    let flip f a b = f b a

    let mapSnd f (a,b) = (a, f b)

    let mapFst f (a,b) = (f a, b)

    let toGrid2d (xs:#seq<#seq<'a>>) : ((int * int) * 'a) list = 
        let ri y = Seq.mapi (fun x a -> ((x,y),a)) >> List.ofSeq
        xs |> Seq.mapi ri |> Seq.concat |> List.ofSeq

    let inline succ n = n+1
    let inline pred n = n-1
    let inline (%!) a b = (a % b + b) % b
    let inline (/%) (x: int64) = fun y -> Math.DivRem(x, y)
    let inline (/%?) (x: int64) = fun y -> 
        let d, r = Math.DivRem(x, y)
        match r with
        | 0L -> d
        | _ -> failwith $"Division ended with remainder %d{r}"

    let times n f i = Seq.fold (fun s _ -> f s) i [1..n]
    let rec until p f i = match (p i) with
                          | true -> i
                          | false -> until p f (f i)

    let splitBy (separator: char) (inputString: string): string list = 
      inputString.Split [|separator|] |> Array.toList   

    let splitByString (separator: string) (inputString: string): string list = 
      inputString.Split([|separator|], StringSplitOptions.None) |> Array.toList
    
    let splitByFn (c : string) f (str : string) = 
        str.Split([| c |], StringSplitOptions.None) |> f

    let (|ParseRegex|_|) regex str =
       let m = Regex(regex).Match(str)
       if m.Success
       then Some (List.tail [ for x in m.Groups -> x.Value ])
       else None

    let duplicate v = v, v

    let unpack2 l =
        match l with
        | [a; b] -> a, b
        | _ -> failwithf "Tried to unpack2 list without exactly 2 elements: %A" l

    let joinStrings (strings: string list) = String.Concat(strings)
    
    let positiveModulo value divisor = 
        match value % divisor with
        | positiveResult when positiveResult >= 0 -> positiveResult
        | negativeResult -> divisor + negativeResult

    let positiveModuloInt64 value divisor = 
        match value % divisor with
        | positiveResult when positiveResult >= 0L -> positiveResult
        | negativeResult -> divisor + negativeResult

    let add2d (ax, ay) (bx, by) =
        (ax + bx), (ay + by)

    let charsToStr (chars : char seq) = chars |> Seq.map string |> String.concat ""


    let inline repeatUntilDuplicate f x =
        let seen = new HashSet<_>()
        let rec aux x =
            if seen.Contains(x) then x
            else
                seen.Add(x) |> ignore
                f x |> aux
        aux x

    let inline repeatN n f x =
        let rec aux n x =
            if n = 0 then x
            else f x |> aux (n - 1)
        aux n x

    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    /// Returns if string is a number
    let isNumeric (s: string) =
        match Int32.TryParse(s) with
        | true, _ -> true
        | _ -> false
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    /// Returns the zig zag position at specific point for a length
    let zigzagPosition (at: int) (maxValue: int) =
        let cycleLength = 2 * (maxValue: int)
        let posInCycle = (at: int) % cycleLength
        if posInCycle <= (maxValue: int) then
            posInCycle
        else
            cycleLength - posInCycle
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    /// Calculates factorial with memoization
    let factorialWithMemoization =
        let memo = System.Collections.Generic.Dictionary<int, int>()
        let rec factorial n =
            if memo.ContainsKey(n) then
                memo[n]
            elif n <= 1 then
                1
            else
                let result = n * factorial (n - 1)
                memo[n] <- result
                result
        factorial
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    /// Calculates time of execution of fucntion
    let measureTime f =
        let stopwatch = Stopwatch.StartNew()
        let result = f
        stopwatch.Stop()
        (result, stopwatch.ElapsedTicks)
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    /// distance for a grid
    let chebyshevDistance (x1, y1) (x2, y2) =
        max (abs (x2 - x1)) (abs (y2 - y1))
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    /// distance for a cube system
    let cubeDistance (coord1: int array) (coord2: int array) = 
        max (abs (coord1.[0] - coord2.[0])) (max (abs (coord1.[1] - coord2.[1])) (abs (coord1.[2] - coord2.[2])))
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    /// manhattan distance
    let manhattanDistance (x1, y1) (x2, y2) =
        abs(x1 - x2) + abs(y1 - y2)
    let distance = manhattanDistance (2, 3) (5, 1)
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    /// Splits a string in two parts
    let splitString (input: string) =
        let indexedArray = input.ToCharArray() |> Array.mapi(fun idx e -> (idx, e))
        let firstPart = indexedArray |> Array.filter(fun e -> (fst e) < indexedArray.Length / 2) |> Array.map snd
        let secondPart = indexedArray |> Array.filter(fun e -> (fst e) >= indexedArray.Length / 2) |> Array.map snd
        [firstPart; secondPart]
    ///////////////////////////////////////////////////////////////////////////////////
    let splitStringInTwo (str: string) =
        let len = str.Length
        let half = len / 2
        let (left, right) = 
            if len % 2 = 0 then
                // If the length is even, return left and right halves of equal length
                (str.Substring(0, half), str.Substring(half))
            else
                // If the length is odd, return left half with one more character
                (str.Substring(0, half + 1), str.Substring(half + 1))
        [left; right]
    ///////////////////////////////////////////////////////////////////////////////////
    // Returns elements every nth position
    let getElementsInIndex (n: int) (l: 'a list) = 
        l |> List.mapi (fun i el -> el, i)                // Add index to element
            |> List.filter (fun (el, i) -> i % n = n - 1)   // Take every nth element
            |> List.map fst                                 // Drop index from the result

    let testgetElementsInIndex = getElementsInIndex 2 [1; 3; 4; 5; 3]
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    // Returns two lists with elements on even and odd positions
    let splitEvenOddList (l: 'a list) = 
        let listwithIdx =
            l |> List.mapi (fun i el -> el, i)                // Add index to element
        ((listwithIdx |> List.filter (fun (el, i) -> i % 2 = 0)) 
            |> List.map fst, (listwithIdx |> List.filter (fun (el, i) -> i % 2 <> 0)) |> List.map fst)
    let testsplitEvenOddList = splitEvenOddList [1; 2; 3; 4]
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    // Returns all possible combinations of all elements of the list
    let rec permutations list =
        match list with
        | [] -> [[]]
        | _ -> 
            list 
            |> List.collect (fun x -> 
                permutations (List.filter ((<>) x) list)
                |> List.map (fun perm -> x :: perm))
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    // Returns lists formed by all possible combinations of n numbers from a list
    let rec comb n l =
        match (n,l) with
        | (0,_) -> [[]]
        | (_,[]) -> []
        | (n,x::xs) ->
            let useX = List.map (fun l -> x::l) (comb (n-1) xs)
            let noX = comb n xs
            (useX @ noX)
    let testcomb = comb 3 [1; 2; 3; 4; 5]
    ///////////////////////////////////////////////////////////////////////////////////
    
    ///////////////////////////////////////////////////////////////////////////////////
    // Returns lists formed by all possible combinations of two elements from a list
    let generateCombinations (list1: 'a list) (list2: 'a list) =
        [for x in list1 do
            for y in list2 do
                yield (x, y)]
    let testgenerateCombinations = generateCombinations [1; 2; 3; 4; 5] [6; 7; 8; 9; 0]
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    let rec combination (num:int) (list: 'a list) : 'a list list = 
        match num, list with
        | 0, _ -> [[]]
        | _, [] -> []
        | k, (x::xs) -> List.map ((@) [x]) (combination (k-1) xs) @ (combination k xs)
    let testcombination =  combination 3 [1; 2; 3; 4; 5]
    ///////////////////////////////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////////////////////////////// 
    let possibleCombinations (combSize: int) (mainList: uint64 list) =
        seq {
            for init in mainList do
                let index = mainList |> List.findIndex(fun e -> e = init)
                if mainList.Length - combSize > index then
                    yield mainList |> List.skip(index) |> List.take(combSize)
        } |> List.ofSeq
    /////////////////////////////////////////////////////////////////////////////////// 

    /////////////////////////////////////////////////////////////////////////////////// 
    /// Returns if all elements of the list are consecutive
    let areConsecutive (i: int list) =
        i |> List.pairwise |> List.forall(fun a -> ((snd a) - (fst a)) = 1)   
    let testareConsecutive = areConsecutive [1; 2; 3; 4; 6]
    ///////////////////////////////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////////////////////////////// 
    /// Returns if all elements of the list verifies the check
    let collectionVerifies (i: 'a list) (verify: 'a*'a -> bool) =
        i |> List.pairwise |> List.forall verify  
    let testcollectionVerifies = collectionVerifies [1; 2; 3; 4; 6] (fun (a, b) -> a < b)
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    /// Returns duration of the execution of the function
    let duration f = 
        let timer = new System.Diagnostics.Stopwatch()
        timer.Start()
        let returnValue = f()
        //printfn "Elapsed Time: %i" timer.ElapsedMilliseconds
        (returnValue, timer.ElapsedTicks)
    ///////////////////////////////////////////////////////////////////////////////////    

    ///////////////////////////////////////////////////////////////////////////////////
    // Returns a sublists list of size num: [1;2;3;4;5;6] -> (3) [[1;2;3];[2;3;4];[3;4;5];[4;5;6]]
    let rec getSubListBySize (num: int) (list: 'a list) : 'a list list= 
        match num <= list.Length with
        | true -> [(list |> List.take num)] @ (getSubListBySize num list.Tail)
        | false -> []
    let testgetSubListBySize = getSubListBySize 3 [1;2;3;4;5;6]
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    /// Returns a collection of lists after splitting the input by a separator
    let getGroupsOnSeparator (inputLines: 'a list) (separator: 'a) =
        let folder (a) (cur, acc) = 
            match a with
            | _ when a <> separator -> a::cur, acc
            | _ -> [], cur::acc 
        let result = List.foldBack folder (inputLines) ([], [])
        (fst result)::(snd result)
    let testgetGroupsOnSeparator = getGroupsOnSeparator ["a "; "b"; " "; "c"; " "; "d"; "e"] " "
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    let printArray (board: int[][]) =
        let maxCol = board.[0].Length - 1
        let maxRow = board.Length - 1
        for r in [0 .. maxRow] do
            for c in [0 .. maxCol] do
                printf "%3i" board.[r].[c]
            printfn "%s" System.Environment.NewLine
    ///////////////////////////////////////////////////////////////////////////////////        

    ///////////////////////////////////////////////////////////////////////////////////
    let getLinesGroupBySeparatorWithEmptySpaceFixReplace (inputLines: string list) (separator: string) =
        let validlines = inputLines |> List.map(fun e -> if e = " " then "§" else e)
        let validsplitter =
            match separator = " " with
            | true -> "§"
            | false -> separator
        let complete = 
            seq {
                for line in validlines do
                    yield! line.Split(' ')
            } |> List.ofSeq
        let folder (a) (cur, acc) = 
            match a with
            | _ when a <> validsplitter -> a::cur, acc
            | _ -> [], cur::acc
                
        let result = List.foldBack folder (complete) ([], [])
        (fst result)::(snd result)
    let testgetLinesGroupBySeparatorWithEmptySpaceFixReplace = getLinesGroupBySeparatorWithEmptySpaceFixReplace ["abceadfapaq "; " "; "asdasqyhgahfg asdsadasda"] " "
    ///////////////////////////////////////////////////////////////////////////////////

    let getLinesGroupBySeparator2 (lines: string seq) (splitter: string) = 
        lines |> Seq.fold (fun (groups, currentGroup) line ->
            if line.Trim() = splitter then
                (currentGroup :: groups, [])
            else
                (groups, line :: currentGroup)
        ) ([], [])
        |> fst
        |> List.rev
    let testgetLinesGroupBySeparator2 = getLinesGroupBySeparator2 ["abceadfapaq"; "asdqwedasca"; " "; "asdasqyhgahfg asdsadasda"] " "
    ///////////////////////////////////////////////////////////////////////////////////
    /// Returns a collection of lists after splitting the input by a separator
    let getgroups (inputLines: 'a list) (separator: 'a) =
        let folder (a) (cur, acc) = 
            match a with
            | _ when a <> separator -> a::cur, acc
            | _ -> [], cur::acc 
        let result = List.foldBack folder (inputLines) ([], [])
        (fst result)::(snd result)
    let testgetgroups = getgroups ["a "; "b"; " "; "c"; " "; "d"; "e"] " "
    ///////////////////////////////////////////////////////////////////////////////////
    let splitWhen predicate list =
        let folder state t =
            if predicate t then
                [] :: state
            else
                (t :: state.Head) :: state. Tail

        list 
        |> List.fold folder [ [] ] 
        |> List.map List.rev
        |> List.rev

    let testsplitWhen = ["a"; "b"; " "; "c "; " "; "d"; "e"] |> splitWhen(fun x -> x = " ")

    let splitStringBySeparator (content: string) (separator: string) =
        let subcontent = content.Split([|separator|], StringSplitOptions.None)
        subcontent |> Array.filter(fun c -> c <> "")
    let testsplitStringBySeparator = splitStringBySeparator "bfasdaaasdqwewqwd" "a"
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    /// Returns array of strings result of split the input by separator
    let splitstring separator (s:string) =
        let values = ResizeArray<_>()
        let rec gather start i =
            let add () = s.Substring(start,i-start) |> values.Add
            if i = s.Length then add()
            elif s.[i] = '"' then inQuotes start (i+1) 
            elif s.[i] = separator then add(); gather (i+1) (i+1) 
            else gather start (i+1)
        and inQuotes start i =
            if s.[i] = '"' then gather start (i+1)
            else inQuotes start (i+1)
        gather 0 0
        values.ToArray() |> Array.filter(fun c -> c <> "")
    let testsplitstring = splitstring 'a' "bfasdaaasdqwewqwd" 
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    /// Returns collection of lists with the first element on every position
    let distrib e L =
        let rec aux pre post = 
            seq {
                match post with
                | [] -> yield (L @ [e])
                | h::t -> 
                    yield (List.rev pre @ [e] @ post)
                    yield! aux (h::pre) t 
            }
        aux [] L
    let testdistrib =  distrib 2 [1; 2; 3]
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    let rec perms = function 
        | [] -> Seq.singleton []
        | h::t -> Seq.collect (distrib h) (perms t)
    ///////////////////////////////////////////////////////////////////////////////////

    let rec combinationsOfLists lists =
        match lists with
        | [] -> [[]]
        | headList::tailLists ->
            [ for headElement in headList do
                for tailCombination in combinationsOfLists tailLists do
                    yield headElement :: tailCombination ]

    ///////////////////////////////////////////////////////////////////////////////////
    /// Returns a collection of lists from a splitted list by 0
    let folder (a) (cur, acc) = 
        match a with
        | _ when a <> 0 -> a::cur, acc
        | _ -> [], cur::acc
    let split lst =
        let result = List.foldBack folder (lst) ([], [])
        (fst result)::(snd result)
    let testsplit = split [1; 2; 3; 0; 5; 1; 2; 3; 1; 5]
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    /// Replace the element of the list on the specific index
    let updateElement index element list = 
        list |> List.mapi (fun i v -> if i = index then element else v)

    let testupdateElement = updateElement 2 "a" ["a"; "b"; "c"]
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    // XOR OPERATOR
    let (^@) (a: bool) (b:bool) : bool =
        a <> b
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    let (|Regex|_|) pattern input =
        let m = Regex.Match(input, pattern)
        if m.Success then Some(List.tail [ for g in m.Groups -> g.Value ])
        else None
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    let getCollisionsBasic (currentForest: list<int[]>) initX initY right down maxwidth maxheight =
        let positions = [initY..down..maxheight]
        seq {
            for pos in initY..down..maxheight do
                let currentPos = positions |> List.findIndex (fun x -> x = pos)
                let point = [|((initX + right) * (currentPos + 1)) % maxwidth; pos + down|]
                match currentForest |> List.exists (fun t -> t.[0] = point.[0] && t.[1] = point.[1]) with 
                | true -> yield point
                | _ -> ()
        } |> Seq.length
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    /// Returns a list of chars from a list of strings
    let listOfStringsToListOfChars (list:string list) =
        seq {
            for l in list do
                yield! l.ToCharArray()
        } |> List.ofSeq
    let testlistOfStringsToListOfChars = listOfStringsToListOfChars ["aa"; "bbb"; "ccc"] 
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    /// Returns a collection of arrays of chars from a list of strings
    let listOfStringToListOfArrayOfChars (list:string list) =
        seq {
            for l in list do
                yield l.ToCharArray()
        } |> List.ofSeq
    let testlistOfStringToListOfArrayOfChars = listOfStringToListOfArrayOfChars ["aa"; "bbb"; "ccc"] 
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    /// Returns a set with the common elements for a collection of arrays
    let commonElements (input: 'a array list) =
        let inputAsList = input |> List.map (List.ofArray)
        let inputAsSet = List.map Set.ofList inputAsList
        let elements =  Seq.reduce Set.intersect inputAsSet
        elements
    let testcommonElements = commonElements [[|'a'; 'a'; 'b'|]; [|'a'; 'b'; 'c'|]; [|'a'; 'b'; 'c'|]]
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    /// Returns a set with the common elements for a collection of arrays
    let commonElements2 (input: 'a array list) =
        input |> List.map (List.ofArray) |> Seq.map Set.ofList |> Set.intersectMany
    let testcommonElements2 = commonElements2 [[|'a'; 'a'; 'b'|]; [|'a'; 'b'; 'c'|]; [|'a'; 'b'; 'c'|]]
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    let toJagged<'a> (arr: 'a[,]) : 'a[][] =
        [|for x in 0..Array2D.length1 arr - 1 do
            yield [| for y in 0 ..Array2D.length2 arr - 1 -> arr.[x, y] |]|]
    ///////////////////////////////////////////////////////////////////////////////////
    //divides a list L into chunks for which all elements match pred
    let divide2 pred L =
        let f x (acc,buf) =
            match pred x,buf with
            | true,buf -> (acc,x::buf)
            | false,[] -> (acc,[])
            | false,buf -> (buf::acc,[])

        let rest,remainingBuffer = List.foldBack f L ([],[])
        match remainingBuffer with
        | [] -> rest
        | buf -> buf :: rest
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    let rev str =
        StringInfo.ParseCombiningCharacters(str) 
        |> Array.rev
        |> Seq.map (fun i -> StringInfo.GetNextTextElement(str, i))
        |> String.concat ""
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    let rec gcdBig (a: bigint) (b: bigint) =
        if b = bigint.Zero then a
        else gcdBig b (a % b)
    
    let rec lcmBig (a: bigint) (b: bigint) =
        if a = bigint.Zero || b = bigint.Zero then bigint.Zero
        else (a * b) / (gcdBig a b)
    
    let rec listLcmBig (numbers: bigint list) =
        match numbers with
        | [] -> bigint.One
        | hd :: tl -> lcmBig hd (listLcmBig tl)
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    // Flatterns a 2D array into an 1D array
    let flattenArray2D (array2D: 'a[,]) =
        array2D 
        |> Seq.cast<'a> 
        |> Seq.toArray
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    // Returns all indexes of the ocurrence in a string
    let findAllIndexes (text: string) (pattern: string) =
        let patternLength = pattern.Length
        let textLength = text.Length
    
        let rec loop startIndex indexes =
            if startIndex > textLength - patternLength then
                indexes
            else
                let index = text.IndexOf(pattern, startIndex)
                if index = -1 then
                    indexes
                else
                    loop (index + 1) (index :: indexes)
        
        loop 0 [] |> List.rev
    
    // Example usage:
    let text = "the quick brown fox jumps over the lazy dog"
    let pattern = "the"
    let indexes = findAllIndexes text pattern
    ///////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////
    // Replaces the first ocurrence in a string
    let replaceFirst (input: string) (oldValue: string) (newValue: string) =
        let index = input.IndexOf(oldValue)
        if index = -1 then input
        else input.Substring(0, index) + newValue + input.Substring(index + oldValue.Length)
    ///////////////////////////////////////////////////////////////////////////////////
    
    ///////////////////////////////////////////////////////////////////////////////////
    // Formats ticks into mm:ss:ms
    let ms ticks =
        let timespan = (TimeSpan.FromTicks ticks)
        sprintf "%02i:%02i.%03i" timespan.Minutes timespan.Seconds timespan.Milliseconds
    ///////////////////////////////////////////////////////////////////////////////////
