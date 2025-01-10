module day02_part01

open System.Text.RegularExpressions

open AdventOfCode_2023.Modules.LocalHelper

let path = "day02/day02_input.txt"

type GameCube = {
    numberOfCubes: int;
    color: string;
}

type GameSet = {
    idGame: int;
    cubes: GameCube list list
}

let gameRules = [
    { numberOfCubes = 12; color = "red" };
    { numberOfCubes = 13; color = "green" };
    { numberOfCubes = 14; color = "blue" }
]

let buildGame (line: string) =
    let gameParts = line.Split(':')
    let pattern = @"(( \d+ (blue|green|red),\s*)+)?( \d+ (blue|green|red)\s*)"
    let cubeSets = 
        (Regex.Matches(gameParts.[1], pattern) |> Array.ofSeq) 
        |> Array.map(fun cubeSet -> 
            cubeSet.Value .Split(',') |> Array.map(fun cube -> 
                let cubeParts = cube.Split(' ')
                { numberOfCubes = int cubeParts.[1]; color = cubeParts.[2] }
            ) |> List.ofArray
        ) |> List.ofArray
    { idGame = int (gameParts.[0].Split(' ').[1]); cubes = cubeSets }

let rec isValidGame (gameSet: GameSet) (rule: GameCube list) =
    match rule with
    | [] -> true
    | ruleCube:: ruleTail ->
        let breaksRule = 
            gameSet.cubes |> List.exists(fun cubeSet -> 
                cubeSet |> List.exists(fun cube -> 
                    cube.color = ruleCube.color && cube.numberOfCubes > ruleCube.numberOfCubes
                )
            )
        match breaksRule with
        | true -> false
        | false -> isValidGame gameSet ruleTail
        

let processGame (lines: string array) =
    let validGames = lines |> Array.map buildGame |> Array.filter(fun g -> isValidGame g gameRules)
    validGames |> Array.sumBy _.idGame

let execute =
    GetLinesFromFile path |> processGame