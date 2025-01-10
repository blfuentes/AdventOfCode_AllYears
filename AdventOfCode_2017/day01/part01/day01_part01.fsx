open AdventOfCode_2017.Modules

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities
//let path = "day01/test_input_01.txt"
let path = "day01/day01_input.txt"

let value0 = "1122"
let value1 = "1111"
let value2 = "1234"
let value3 = "91212129"

let calculateCaptcha (captcha: string) =
    let values = 
        seq {
            for idx in [0..captcha.Length - 1] do
                if idx < captcha.Length - 1 && captcha.Substring(idx, 1) = captcha.Substring(idx + 1, 1) then
                    yield int(captcha.Substring(idx, 1))
            if captcha.Substring(captcha.Length - 1, 1) = captcha.Substring(0, 1) then
                yield int(captcha.Substring(0, 1))
        }
    values |> Seq.sum

calculateCaptcha value0
calculateCaptcha value1
calculateCaptcha value2
calculateCaptcha value3

calculateCaptcha (LocalHelper.GetContentFromFile(path))