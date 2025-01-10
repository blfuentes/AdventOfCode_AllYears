module day21_part01

open AdventOfCode_2021.Modules

type Player = {
    Id: int
    Position: int
    Score: int
}

let parseContent(lines: string array) =
    lines
    |> Array.map(fun line ->
        let(i, p) = ((int)(line.Split(" ")[1]), (int)(line.Split(" ")[4]))
        { Id = i; Position = p - 1; Score = 0 }
    )

let turn (player: Player) (rollsdone: int) (lastdiceIdx: int) (dice: int array) =
    let (idx1, idx2, idx3) = ((lastdiceIdx+1)%100, (lastdiceIdx+2)%100, (lastdiceIdx+3)%100)
    let (roll1, roll2, roll3) = (dice[idx1], dice[idx2], dice[idx3])
    let newscore = (roll1 + roll2 + roll3) 
    let newpos = (newscore + player.Position) % 10 
    //printfn "Player %d rolls %d+%d+%d and moves to space %d for a total of score %d" player.Id roll1 roll2 roll3 (newpos+1) (player.Score+(newpos + 1))
    ((idx3, rollsdone + 3), { player with Position = newpos; Score = player.Score + (newpos + 1) })

let play(players: Player array) (towin: int) =
    let dice = [|1..100|]
    let rec playround (diceIdx: int) (rollsdone: int) =
        match (players[0], players[1]) with
        | (p1, p2) when p1.Score >= towin ->
            p2.Score * rollsdone
        | (p1, p2) when p2.Score >= towin ->
            p1.Score * rollsdone
        | _ ->
            // player 1
            let ((newIdx, newrollsdone), newplayer1) = turn players[0] rollsdone diceIdx dice
            if newplayer1.Score >= towin then
                players[1].Score * newrollsdone
            else
                // player 2
                let ((newIdx, newrollsdone), newplayer2) = turn players[1] newrollsdone newIdx dice
                if newplayer2.Score >= towin then
                    newplayer1.Score * newrollsdone
                else
                    players[0] <- newplayer1
                    players[1] <- newplayer2
                    playround newIdx newrollsdone

    playround  -1 0

let execute =
    let path = "day21/day21_input.txt"
    //let path = "day21/test_input_21.txt"
    let content = LocalHelper.GetLinesFromFile path
    let players = parseContent content

    play players 1000