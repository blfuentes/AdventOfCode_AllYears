
let result = [1;2;3] |> List.pairwise(fun (a, b) -> [a] @ [b])



let increasingForThree(value: char list) : bool =
    [0..value.Length - 3]
    |> List.exists(fun i -> 1 + int (value.Item(i)) = (int (value.Item(i+1))) && 1 + int (value.Item(i+1)) = int (value.Item(i+2)))

let notInvalidChars(value: char list) : bool =
    ((value |> Set.ofList) |> Set.intersect(Set.ofList(['i'; 'o'; 'l']))).Count = 0

let hasAtLeastTwoPairs(value: char list) : bool =
    let possiblePairs = 
        seq {
            for i in [0..value.Length - 2] do
                if value[i] = value[i+1] then
                    yield i 
        } |> List.ofSeq
    possiblePairs |> List.iter (printfn "%A")
    (possiblePairs |> List.pairwise |> List.filter(fun (a, b) -> b > a + 1)).Length >= 1
    
    //(value |> List.pairwise |> List.filter(fun (a, b) -> a = b)).Length >= 2

let isValidPassword(value: char list) : bool =
    increasingForThree(value) && notInvalidChars(value) && hasAtLeastTwoPairs(value)


let input = "abcdffaa".ToCharArray() |> List.ofArray

let input = "ghjaabcc".ToCharArray() |> List.ofArray
let input = "abbcegjk".ToCharArray() |> List.ofArray
let chars = ['a'; 'b'; 'c'; 'd'; 'f'; 'f'; 'a'; 'a']
int (chars[0] = 1 + (int chars[0+1]) && int (chars[0+1]) = 1 + int (chars[0+2])
(int (chars[0]) = (1 + (int chars[0+1])))


increasingForThree input
notInvalidChars input
hasAtLeastTwoPairs input
isValidPassword input