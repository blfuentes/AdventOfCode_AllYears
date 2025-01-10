module day02_part01

open AoC_2019.Modules

let execute =
    let filepath = __SOURCE_DIRECTORY__ + @"../../day02_input.txt"
    //let filepath = __SOURCE_DIRECTORY__ + @"../../test_input.txt"
    //let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_yavuz.txt"
    let values = IntCodeModule.getInput filepath

    // prepare the tranche input 
    values.[1I] <- 12I
    values.[2I] <- 2I

    let result = IntCodeModule.getOutput values 0I 0I [0I] false 0I
    values.[0I]