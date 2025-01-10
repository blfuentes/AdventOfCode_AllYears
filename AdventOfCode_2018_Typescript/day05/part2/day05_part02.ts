import { PolymerResult } from "./PolymerResult";

export class Day05Part02 {
    execute() {
        let fs = require("fs");
        let path = require('path');


        // let filepath = path.join(__dirname, "../test01_input.txt");
        let filepath = path.join(__dirname, "../day05_input.txt");
        let text: string = fs.readFileSync(filepath, "utf-8");

        let polymerCollection: Array<PolymerResult> = [];

        let shortestPolymer: number = 0;
        var startTime = new Date();
        for (var counter = 0; counter < 26; counter++) {
            var tmpPolymer = new PolymerResult(text, String.fromCharCode(counter + 97));
            tmpPolymer.reactPolymer();
            if (counter == 0) {
                shortestPolymer = tmpPolymer.value;
            } else if (tmpPolymer.value < shortestPolymer)
                shortestPolymer = tmpPolymer.value;
            // polymerCollection.push(tmpPolymer);
        }
        var endTime = new Date();
        var timeElapsed = endTime.valueOf() - startTime.valueOf();


        // console.log(`Remaining units for ${text} :: ${text.length}`);
        //console.log(`Time elapsed: ${timeElapsed}`)
        //console.log(`Shortest polymer: ${shortestPolymer}`)
        return shortestPolymer;
    }
}

