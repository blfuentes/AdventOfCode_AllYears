open System.IO

#load @"../../Modules/IntcodeComputerModule.fs"
open AoC_2019.Modules

let filepath = __SOURCE_DIRECTORY__ + @"../../day07_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_04.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_05.txt"


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
                let values1 = IntcodeComputerModule.getInputBigData filepath
                let values2 = IntcodeComputerModule.getInputBigData filepath
                let values3 = IntcodeComputerModule.getInputBigData filepath
                let values4 = IntcodeComputerModule.getInputBigData filepath
                let values5 = IntcodeComputerModule.getInputBigData filepath

                let outputs= [|0I; 0I; 0I; 0I; 0I|]
                let indexes = [|0I; 0I; 0I; 0I; 0I|]
                let phases = (perm |> List.toArray)
                let runningAmp = [|true; true; true; true; true; |]

                let result1 =  IntcodeComputerModule.executeWithPhaseLoopMode(values1, phases.[0], indexes.[0], outputs.[0], 2I) 
                Array.set outputs 1 (fst result1)
                Array.set indexes 0 (fst (snd result1))

                let result2 =  IntcodeComputerModule.executeWithPhaseLoopMode(values2, phases.[1], indexes.[1], outputs.[1], 2I) 
                Array.set outputs 2 (fst result2)
                Array.set indexes 1 (fst (snd result2))

                let result3 =  IntcodeComputerModule.executeWithPhaseLoopMode(values3, phases.[2], indexes.[2], outputs.[2], 2I) 
                Array.set outputs 3 (fst result3)
                Array.set indexes 2 (fst (snd result3))

                let result4 =  IntcodeComputerModule.executeWithPhaseLoopMode(values4, phases.[3], indexes.[3], outputs.[3], 2I) 
                Array.set outputs 4 (fst result4)
                Array.set indexes 3 (fst (snd result4))

                let result5 =  IntcodeComputerModule.executeWithPhaseLoopMode(values5, phases.[4], indexes.[4], outputs.[4], 2I) 
                Array.set outputs 0 (fst result5)
                Array.set indexes 4 (fst (snd result5))
                Array.set runningAmp 4 (snd (snd result5))

                Array.set phases 0 outputs.[0]
                Array.set phases 1 outputs.[1]
                Array.set phases 2 outputs.[2]
                Array.set phases 3 outputs.[3]
                Array.set phases 4 outputs.[4]

                while runningAmp.[4] do
                    let result11 =  IntcodeComputerModule.executeWithPhaseLoopMode(values1, outputs.[0], indexes.[0], outputs.[0], 1I) 
                    Array.set outputs 1 (fst result11)
                    Array.set indexes 0 (fst (snd result11))
                    Array.set runningAmp 0 (snd (snd result11))

                    let result22 =  IntcodeComputerModule.executeWithPhaseLoopMode(values2, outputs.[1], indexes.[1], outputs.[1], 1I) 
                    Array.set outputs 2 (fst result22)
                    Array.set indexes 1 (fst (snd result22))
                    Array.set runningAmp 1 (snd (snd result22))

                    let result33 =  IntcodeComputerModule.executeWithPhaseLoopMode(values3, outputs.[2], indexes.[2], outputs.[2], 1I) 
                    Array.set outputs 3 (fst result33)
                    Array.set indexes 2 (fst (snd result33))
                    Array.set runningAmp 2 (snd (snd result33))

                    let result44 =  IntcodeComputerModule.executeWithPhaseLoopMode(values4, outputs.[3], indexes.[3], outputs.[3], 1I) 
                    Array.set outputs 4 (fst result44)
                    Array.set indexes 3 (fst (snd result44))
                    Array.set runningAmp 3 (snd (snd result44))

                    let result55 =  IntcodeComputerModule.executeWithPhaseLoopMode(values5, outputs.[4], indexes.[4], outputs.[4], 1I) 
                    Array.set outputs 0 (fst result55)
                    Array.set indexes 4 (fst (snd result55))
                    Array.set runningAmp 4 (snd (snd result55))                  
                (perm, outputs.[0])
            )
    let results2 = results |>Seq.maxBy snd
    results2