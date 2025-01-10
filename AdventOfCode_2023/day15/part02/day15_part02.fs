module day15_part02

open System.Text.RegularExpressions

open AdventOfCode_2023.Modules

type op = {
    label: char array
    op: char
    focalLength: int option
}

type box = {
    id: int
    content: op list
}

let calculateHash (input: char array)  =
    let calcHash (acc: int) (input: char) =
        (acc + (int input)) * 17 % 256
    Array.fold calcHash 0 input

let getOp (input: string) =
    let parts = Regex.Match(input, @"([a-zA-Z]+)([-=])(\d+)?")
    {
        label = parts.Groups.[1].Value.ToCharArray()
        op = parts.Groups.[2].Value.[0]
        focalLength = if parts.Groups.[3].Value.Length > 0 then Some(int parts.Groups.[3].Value) else None
    }

let rec runOp (remainingOps: op list) (boxes: box array)=
    match remainingOps with
    | currentOp :: tail ->
        let hashOfLabel = calculateHash currentOp.label
        let box = boxes.[hashOfLabel]
        let newContent =
            if currentOp.op = '-' then
                box.content |> List.filter(fun c -> c.label <> currentOp.label)
            else
                if box.content |> List.exists(fun c -> c.label = currentOp.label) then
                    box.content 
                    |> List.map(fun c ->
                        if c.label = currentOp.label then
                            { c with focalLength = currentOp.focalLength }
                        else
                            c
                    )
                else
                    box.content @ [currentOp]

        boxes.[hashOfLabel] <- { id = hashOfLabel; content = newContent }
        runOp tail boxes
    | [] -> boxes

let calculateFocusingPower (box: box) =
    box.content |> List.mapi (fun idx op -> (box.id + 1) * (idx + 1) * op.focalLength.Value) |> List.sum

let execute =
    let path = "day15/day15_input.txt"
    let lines = LocalHelper.GetContentFromFile path
    let allops = lines.Split(',') |> Array.map getOp |> Array.toList
    let boxes = Array.create 256 {id = 0; content = []}
    let result = runOp allops boxes
    let focusingPowers = result |> Array.map calculateFocusingPower
    focusingPowers |> Array.sum
