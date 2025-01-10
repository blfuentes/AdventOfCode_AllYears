import { StarPoint } from "../StarPoint";

export class Day10Part01 {
    execute() {
        let fs = require("fs");
        let path = require('path');


        // let filepath = path.join(__dirname, "../test01_input.txt");
        let filepath = path.join(__dirname, "../day10_input.txt");
        let lines = fs.readFileSync(filepath, "utf-8").split("\r\n");

        let starCollection: Array<StarPoint> = [];

        for (let line of lines) {
            let regExp = new RegExp("-?\\d+", "g");
            var foundValues = line.match(regExp);
            if (foundValues != null) {
                starCollection.push(new StarPoint(new Array(parseInt(foundValues[0]), parseInt(foundValues[1])),
                    new Array(parseInt(foundValues[2]), parseInt(foundValues[3]))));

            }
        }

        let counter = 0;
        let go = true;
        do {
            starCollection.map(starpoint => starpoint.calculatePosition(counter));

            let minX = Math.min.apply(null, starCollection.map(starpoint => starpoint.position[0]));
            let minY = Math.min.apply(null, starCollection.map(starpoint => starpoint.position[1]));

            // ALTERNATIVE: PRINT MOVE THE POINTS TO THE LEFT-TOP CORNER
            // move to the left
            // if (minX < 0) {
            //     starCollection.map(starpoint => {
            //         starpoint.position[0] = starpoint.position[0] + Math.abs(minX);
            //     });
            // } else {
            //     starCollection.map(starpoint => {
            //         starpoint.position[0] = starpoint.position[0] - minX;
            //     });
            // }

            // set to the top
            // if (minY < 0) {
            //     starCollection.map(starpoint => {
            //         starpoint.position[1] = starpoint.position[1] + Math.abs(minY);
            //     });
            // } else {
            //     starCollection.map(starpoint => {
            //         starpoint.position[1] = starpoint.position[1] - minY;
            //     });
            // }

            let maxX = Math.max.apply(null, starCollection.map(starpoint => starpoint.position[0]));
            let maxY = Math.max.apply(null, starCollection.map(starpoint => starpoint.position[1]));

            // ALTERNATIVE: PRINT MOVE THE POINTS TO THE LEFT-TOP CORNER
            // minX = Math.min.apply(null, starCollection.map(starpoint => starpoint.position[0]));
            // minY = Math.min.apply(null, starCollection.map(starpoint => starpoint.position[1]));

            if (minY >= 0 && maxY >= 0 && maxY - minY == 9) {
                go = false;
                //console.log(`Second ${counter} printed. `);
            } else {
                counter++;
                //console.log(`Second ${counter} skipped.`);
                continue;
            }

            let ouputMessage: Array<Array<string>> = [];
            ouputMessage = Array(maxX + 1).fill(null).map(item => (new Array(maxY + 1).fill(".")));

            for (let star of starCollection) {
                let tmpX = star.position[0];
                let tmpY = star.position[1];
                ouputMessage[tmpX][tmpY] = "#";
            }

            var newFileName = `second ${counter + 1}.txt`;
            var file = fs.createWriteStream(newFileName);
            file.on('error', function (err: string) {
                /* error handling */
                console.log(`error: ${err}`);
            });
            // 
            for (var idx = 0; idx < maxY + 1; idx++) {
                var newline: string = "";
                for (var jdx = 0; jdx < maxX + 1; jdx++) {
                    // newline += ouputMessage[idx][jdx];
                    newline += ouputMessage[jdx][idx];
                }
                file.write(newline + "\n");
                console.log(newline);
            }
            // ouputMessage.forEach(function(v) { console.log(file.write(v.toString() + "\n")); });
            file.end();
            // console.log(`Min X: ${minX}, min Y: ${minY}.`);

            // console.log(`Second ${counter + 1} finished.`);
            counter++;
        } while (go);
        //console.log(`Finished in second: ${counter}`);
    }
}

