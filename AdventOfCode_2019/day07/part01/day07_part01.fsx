open System.IO

//#load @"../../Modules/IntcodeComputerModule.fs"
#load @"../../Modules/IntCodeModule.fs"
open AoC_2019.Modules

//let filepath = __SOURCE_DIRECTORY__ + @"../../day07_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../../day05/day05_input.txt"
let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_01.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_02.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_03.txt"

let values = IntCodeModule.getInput filepath
//let result0 = IntcodeComputerModule.execute(filepath, 5I)


let distrib e L =
    let rec aux pre post = 
        seq {
            match post with
            | [] -> yield (L @ [e])
            | h::t -> yield (List.rev pre @ [e] @ post)
                      yield! aux (h::pre) t 
        }
    aux [] L

let rec perms = function 
| [] -> Seq.singleton []
| h::t -> Seq.collect (distrib h) (perms t)

let permutation = 
    let results = 
        perms [0I .. 4I] |> Seq.map
            (fun perm -> 
                let result1 =  IntCodeModule.getOutput (IntCodeModule.getInput filepath)  0I 0I(perm |> List.toArray).[0] 0I 2I true false 0I
                let result2 = IntCodeModule.getOutput (IntCodeModule.getInput filepath) 0I 0I (perm |> List.toArray).[1] result1 2I true false 0I
                let result3 = IntCodeModule.getOutput (IntCodeModule.getInput filepath) 0I 0I (perm |> List.toArray).[2] result2 2I true false 0I
                let result4 = IntCodeModule.getOutput (IntCodeModule.getInput filepath) 0I 0I (perm |> List.toArray).[3] result3 2I true false 0I
                let result5 = IntCodeModule.getOutput (IntCodeModule.getInput filepath) 0I 0I (perm |> List.toArray).[4] result4 2I true false 0I
                (perm, result5)
            ) |>Seq.maxBy snd
    results

// 4,3,2,1,0
let result1 = IntCodeModule.getOutput (IntCodeModule.getInput filepath) 0I 0I 4I 0I 2I true false 0I
let result2 = IntCodeModule.getOutput (IntCodeModule.getInput filepath) 0I 0I 3I result1 2I true false 0I
let result3 = IntCodeModule.getOutput (IntCodeModule.getInput filepath) 0I 0I 2I result2 2I true false 0I
let result4 = IntCodeModule.getOutput (IntCodeModule.getInput filepath) 0I 0I 1I result3 2I true false 0I
let result5 = IntCodeModule.getOutput (IntCodeModule.getInput filepath) 0I 0I 0I result4 2I true false 0I

// 0,1,2,3,4
//let result1 = IntCodeModule.getOutput (IntCodeModule.getInput filepath) 0I 0I 0I 0I 2I true false 0I
//let result2 = IntCodeModule.getOutput (IntCodeModule.getInput filepath) 0I 0I 1I result1 2I true false 0I
//let result3 = IntCodeModule.getOutput (IntCodeModule.getInput filepath) 0I 0I 2I result2 2I true false 0I
//let result4 = IntCodeModule.getOutput (IntCodeModule.getInput filepath) 0I 0I 3I result3 2I true false 0I
//let result5 = IntCodeModule.getOutput (IntCodeModule.getInput filepath) 0I 0I 4I result4 2I true false 0I

// 1,0,4,3,2
//let result1 = IntCodeModule.getOutput (IntCodeModule.getInput filepath) 0I 0I 1I 0I 2I true false 0I
//let result2 = IntCodeModule.getOutput (IntCodeModule.getInput filepath) 0I 0I 0I result1 2I true false 0I
//let result3 = IntCodeModule.getOutput (IntCodeModule.getInput filepath) 0I 0I 4I result2 2I true false 0I
//let result4 = IntCodeModule.getOutput (IntCodeModule.getInput filepath) 0I 0I 3I result3 2I true false 0I
//let result5 = IntCodeModule.getOutput (IntCodeModule.getInput filepath) 0I 0I 2I result4 2I true false 0I