module day01_part02


open AdventOfCode_2017.Modules.LocalHelper

let path = "day01/day01_input.txt"

let calculateCaptcha (captcha: string) (offset: int)=
    let values = 
        seq {
            for idx in [0..captcha.Length - 1] do
                let val1 = captcha.Substring(idx, 1)
                let idx2 = if idx + offset <= captcha.Length - 1 then idx + offset else offset - (captcha.Length - idx)
                let val2 = captcha.Substring(idx2, 1)
                if val1 = val2 then
                    yield int(val1)
        }
    values |> Seq.sum

let execute() =
    let value = GetContentFromFile(path)
    calculateCaptcha value (value.Length / 2)