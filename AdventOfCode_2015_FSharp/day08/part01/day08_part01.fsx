let countCharsData(input: string) =
    let old = $"\\\""
    let replaced = $"\""
    let escapedQuotes = input.Replace(old, replaced)
    printfn $"From {input} to {escapedQuotes}"
    escapedQuotes.Length - 2


countCharsData "aaa\"aaa\"algo"


"\xa8br\x8bjr\""
".br.jr."