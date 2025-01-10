module day02_part02


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

let buildGame (line: string) =
    let gameParts = line.Split(':')
    let cubeSets = 
        gameParts.[1].Split(';') |> Array.map(fun cubeSet -> 
            cubeSet.Split(',') |> Array.map(fun cube -> 
                let cubeParts = cube.Split(' ')
                { numberOfCubes = int cubeParts.[1]; color = cubeParts.[2] }
            ) |> List.ofArray
        ) |> List.ofArray
    { idGame = int (gameParts.[0].Split(' ').[1]); cubes = cubeSets }

let findMininumNumOfCubes (gameSet: GameSet) =
    let allCubes = gameSet.cubes |> List.concat
    let minCubesByColor = 
        allCubes |> List.groupBy _.color
            |> List.map (fun (co, cu) -> { numberOfCubes = (cu |> List.maxBy _.numberOfCubes).numberOfCubes ; color = co })
    minCubesByColor |> List.map _.numberOfCubes |> List.reduce (*)

let processGame (lines: string array) =
    let minimunByGame = lines |> Array.map buildGame |> Array.map findMininumNumOfCubes
    minimunByGame |> Array.sum

let execute =
    GetLinesFromFile path |> processGame