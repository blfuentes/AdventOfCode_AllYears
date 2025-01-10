module day20_part01

open AdventOfCode_2015.Modules

let factors number = seq {
    for divisor in 1 .. (float >> sqrt >> int) number do
    if number % divisor = 0 then
        yield (number, divisor)
        yield (number, number / divisor) }

let find filt presentsSize expected = 
    Seq.initInfinite (fun i -> 
        (factors i) 
        |> Seq.distinctBy snd 
        |> Seq.filter filt
        |> Seq.sumBy snd 
        |> (*) presentsSize 
    )
    |> Seq.findIndex (fun sum -> sum >= expected)

let execute =
    let input = "./day20/day20_input.txt"
    let content = LocalHelper.GetContentFromFile input |> int
    (find (fun _ -> true) 10 content)