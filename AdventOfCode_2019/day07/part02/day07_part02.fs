module day07_part02

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
        perms [5I .. 9I] |> Seq.map 
            (fun perm -> 
                let values1 = IntCodeModule.getInput filepath
                let values2 = IntCodeModule.getInput filepath
                let values3 = IntCodeModule.getInput filepath
                let values4 = IntCodeModule.getInput filepath
                let values5 = IntCodeModule.getInput filepath

                let outputs= [|0I; 0I; 0I; 0I; 0I|]
                let indexes = [|0I; 0I; 0I; 0I; 0I|]
                let phases = (perm |> List.toArray)
                let runningAmp = [|true; true; true; true; true; |]

                let result1 =  IntCodeModule.getOutput values1  indexes.[0] 0I [phases.[0]; 0I] false 0I
                Array.set outputs 1 result1.Output
                Array.set indexes 0 result1.Idx

                let result2 =  IntCodeModule.getOutput values2  indexes.[1] 0I [phases.[1]; outputs.[1]] false 0I
                Array.set outputs 2 result2.Output
                Array.set indexes 1 result2.Idx

                let result3 =  IntCodeModule.getOutput values3  indexes.[2] 0I [phases.[2]; outputs.[2]] false 0I
                Array.set outputs 3 result3.Output
                Array.set indexes 2 result3.Idx

                let result4 =  IntCodeModule.getOutput values4  indexes.[3] 0I [phases.[3]; outputs.[3]] false 0I
                Array.set outputs 4 result4.Output
                Array.set indexes 3 result4.Idx

                let result5 =  IntCodeModule.getOutput values5  indexes.[4] 0I [phases.[4]; outputs.[4]] false 0I
                Array.set outputs 0 result5.Output
                Array.set indexes 4 result5.Idx
                Array.set runningAmp 4 result5.Continue

                while runningAmp.[4] do
                    let result11= IntCodeModule.getOutput values1  indexes.[0] 0I [outputs.[0]] false 0I
                    Array.set outputs 1 result11.Output
                    Array.set indexes 0 result11.Idx
                    Array.set runningAmp 0 result11.Continue

                    let result22 =  IntCodeModule.getOutput values2  indexes.[1] 0I [outputs.[1]] false 0I
                    Array.set outputs 2 result22.Output
                    Array.set indexes 1 result22.Idx
                    Array.set runningAmp 1 result22.Continue

                    let result33 = IntCodeModule.getOutput values3  indexes.[2] 0I [outputs.[2]] false 0I
                    Array.set outputs 3 result33.Output
                    Array.set indexes 2 result33.Idx
                    Array.set runningAmp 2 result33.Continue

                    let result44 = IntCodeModule.getOutput values4  indexes.[3] 0I [outputs.[3]] false 0I
                    Array.set outputs 4 result44.Output
                    Array.set indexes 3 result44.Idx
                    Array.set runningAmp 3 result44.Continue

                    let result55 = IntCodeModule.getOutput values5  indexes.[4] 0I [outputs.[4]] false 0I
                    Array.set outputs 0 result55.Output
                    Array.set indexes 4 result55.Idx
                    Array.set runningAmp 4 result55.Continue              
                (perm, outputs.[0])
            )
    let results2 = results |>Seq.maxBy snd
    results2

let execute =
    snd permutation