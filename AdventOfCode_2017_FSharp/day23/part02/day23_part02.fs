module day23_part02

open System.Collections.Generic

let hackProgram =
    // Analyze the program and replicate it:
    let registers = new Dictionary<string, int64>()
    [|'a'..'h'|]
    |> Array.iter (fun c -> registers.Add(c.ToString(), 0))
    registers["a"] <- 1L
    registers["b"] <- 57L // set b 57
    registers["c"] <- registers["b"] // set c b

    if registers["a"] <> 0L then // jnz a 2
        registers["b"] <- registers["b"] * 100L + 100000L // mul b 100 | sub b -100000
        registers["c"] <- registers["b"] + 17000L // set c b | sub c -17000
    let mutable firstrun = true
    while firstrun || registers["g"] <> 0L do
        firstrun <- false
        //Set "f" 1L
        //Set "d" 2L
        //Set "e" 2L
        //Set "d" 2L
        registers["f"] <- 1L // set f 1
        registers["d"] <- 2L // set d 2
        registers["e"] <- 2L // set e 2
        registers["d"] <- 2L // set d 2 init of the loop (jnz 1 -23) always jump back
        let mutable docontinue = true
        while registers["d"] * registers["d"] <= registers["b"] && docontinue do
            if (registers["b"] % registers["d"] = 0) then
                registers["f"] <- 0L // set f 0
                docontinue <- false
            registers["d"] <- registers["d"] + 1L // sub d -1
        if registers["f"] = 0 then  // jnz f 2
            registers["h"] <- registers["h"] + 1L // sub h -1
        registers["g"] <- registers["b"] - registers["c"] // set g b | sub g c
        registers["b"] <- registers["b"] + 17L // sub b -17

    registers["h"]

let execute() =
    hackProgram