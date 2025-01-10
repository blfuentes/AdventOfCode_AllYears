open System.IO
open System.Collections.Generic

#load @"../../Modules/IntcodeComputerModule.fs"
open AoC_2019.Modules

let filepath = __SOURCE_DIRECTORY__ + @"../../day09_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_01.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_02.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_03.txt"

let values = IntcodeComputerModule.getInput filepath

let dataContainer = new Dictionary<bigint, bigint>()
dataContainer.Add(10000I, 5000I)
let value = dataContainer.[10000I]
for idx in [|0..values.Length - 1|] do
    dataContainer.Add(bigint idx, bigint values.[idx])

let result = IntcodeComputerModule.executeBigData(filepath, 1I)