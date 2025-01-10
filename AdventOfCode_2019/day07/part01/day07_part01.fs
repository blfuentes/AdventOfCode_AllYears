module day07_part01

open AoC_2019.Modules

let filepath = __SOURCE_DIRECTORY__ + @"../../day07_input.txt"

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
                let result1 = IntCodeModule.getOutput (IntCodeModule.getInput filepath) 0I 0I [perm.Item(0); 0I] false 0I
                let result2 = IntCodeModule.getOutput (IntCodeModule.getInput filepath) 0I 0I [perm.Item(1); result1.Output] false 0I
                let result3 = IntCodeModule.getOutput (IntCodeModule.getInput filepath) 0I 0I [perm.Item(2); result2.Output] false 0I
                let result4 = IntCodeModule.getOutput (IntCodeModule.getInput filepath) 0I 0I [perm.Item(3); result3.Output] false 0I
                let result5 = IntCodeModule.getOutput (IntCodeModule.getInput filepath) 0I 0I [perm.Item(4); result4.Output] false 0I
                (perm, result5)
            ) |>Seq.maxBy snd
    results

let execute =
    (snd permutation).Output