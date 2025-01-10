import { Claim } from "../Claim";

export class Day03Part01 {
    execute() {
        let fs = require("fs");
        let path = require('path');

        let entryElements = [];
        let filepath = path.join(__dirname, "../day03_input.txt");
        // let filepath = path.join(__dirname, "../test01_input.txt");
        let text = fs.readFileSync(filepath, "utf-8");
        entryElements = text.split("\r\n");

        let claimsList: Claim[] = [];

        for (let entry of entryElements) {
            claimsList.push(new Claim(entry));
        }

        let baseSquare: Array<Array<number>>;
        baseSquare = Array(1000).fill(null).map(item => (new Array(1000).fill(0)));
        // baseSquare = new Array(1000);
        // for (var idx = 0; idx < baseSquare.length; idx++) {
        //     baseSquare[idx] = new Array(1000);
        //     for (var jdx = 0; jdx < baseSquare.length; jdx++) {
        //         baseSquare[idx][jdx] = 0;
        //     }
        // }

        let squareInches: number = 0;

        for (let claim of claimsList) {
            for (var cidx = claim.initTop; cidx < (claim.initTop + claim.height); cidx++) {
                for (var cjdx = claim.initLeft; cjdx < (claim.initLeft + claim.width); cjdx++) {
                    baseSquare[cidx][cjdx] = baseSquare[cidx][cjdx] + 1;
                }
            }
        }

        for (var idx = 0; idx < baseSquare.length; idx++) {
            for (var jdx = 0; jdx < baseSquare.length; jdx++) {
                if (baseSquare[idx][jdx] > 1) {
                    squareInches++;
                }
            }
        }

        //console.log(`Overlaped: ${squareInches}`);
        return squareInches;
    }
}
