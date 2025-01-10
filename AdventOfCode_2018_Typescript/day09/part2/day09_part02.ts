import { MarbleNode } from "../MarbleNode";

export class Day09Part02 {
    execute() {
        let fs = require("fs");
        let path = require('path');

        // let filepath = path.join(__dirname, "../test01_input.txt");
        // let filepath = path.join(__dirname, "../test02_input.txt");
        // let filepath = path.join(__dirname, "../test03_input.txt");
        let filepath = path.join(__dirname, "../day09_input.txt");
        let text = fs.readFileSync(filepath, "utf-8").split("\r\n");

        let factor: number = 23;

        function marbleGivesPoints(marble: number) {
            return (marble % factor) == 0;
        }

        var startTime = new Date();
        for (var idx = 0; idx < text.length; idx++) {
            //
            let numberOfPlayers = parseInt(text[idx].split(" ")[0]);
            let lastMarbleWorth = parseInt(text[idx].split(" ")[6]) * 100;

            //
            let playerScore: Array<number> = [];
            playerScore = Array(numberOfPlayers).fill(0);

            //
            let currentMarble = new MarbleNode(0);
            currentMarble.nextMarble = currentMarble;
            currentMarble.prevMarble = currentMarble;

            let marbleBoard: Array<MarbleNode> = [];
            marbleBoard.push(currentMarble);
            let currentPlayer = 1;
            let currentMarbleValue = 1;

            //
            while (currentMarbleValue <= lastMarbleWorth) {
                let newMarble = new MarbleNode(currentMarbleValue);
                currentPlayer = currentMarbleValue % numberOfPlayers;

                if (marbleGivesPoints(newMarble.value)) {
                    playerScore[currentPlayer] += newMarble.value;
                    let tmpMarble = currentMarble;
                    for (var counter = 0; counter < 7; counter++) {
                        tmpMarble = tmpMarble.prevMarble;
                    }
                    playerScore[currentPlayer] += tmpMarble.value;
                    tmpMarble.nextMarble.prevMarble = tmpMarble.prevMarble;
                    tmpMarble.prevMarble.nextMarble = tmpMarble.nextMarble;
                    currentMarble = tmpMarble.nextMarble;
                } else {
                    newMarble.prevMarble = currentMarble.nextMarble;
                    newMarble.nextMarble = currentMarble.nextMarble.nextMarble;

                    currentMarble.nextMarble.nextMarble.prevMarble = newMarble;
                    currentMarble.nextMarble.nextMarble = newMarble;

                    currentMarble = newMarble;

                    marbleBoard.push(newMarble);
                }
                currentPlayer++;
                currentMarbleValue++;
            }

            let maxValue = 0;
            for (let player of playerScore) {
                if (player > maxValue) {
                    maxValue = player;
                }
            }
            var endTime = new Date();
            var timeElapsed = endTime.valueOf() - startTime.valueOf();
            //console.log(`Time elapsed: ${timeElapsed}`)
            //console.log(`Winner: ${maxValue}`)

            return maxValue;
        }

    }
}