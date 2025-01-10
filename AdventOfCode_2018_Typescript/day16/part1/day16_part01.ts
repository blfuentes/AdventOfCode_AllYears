import { CheckCase } from "../CheckCase";
import { Operator } from "../Operator";

export class Day16Part01 {
    calculateCase(checkCasesCollection, checkOperator) {
        let numberOfCases = 0;
        for (let mycase of checkCasesCollection) {
            let result = 0;
            if (checkOperator.addr(mycase)) {
                result++
            }
            if (checkOperator.addi(mycase)) {
                result++;
            }

            if (checkOperator.mulr(mycase)) {
                result++
            }
            if (checkOperator.muli(mycase)) {
                result++;
            }

            if (checkOperator.banr(mycase)) {
                result++
            }
            if (checkOperator.bani(mycase)) {
                result++;
            }

            if (checkOperator.borr(mycase)) {
                result++
            }
            if (checkOperator.bori(mycase)) {
                result++;
            }

            if (checkOperator.setr(mycase)) {
                result++
            }
            if (checkOperator.seti(mycase)) {
                result++;
            }

            if (checkOperator.gtir(mycase)) {
                result++;
            }
            if (checkOperator.gtri(mycase)) {
                result++
            }
            if (checkOperator.gtrr(mycase)) {
                result++;
            }

            if (checkOperator.eqir(mycase)) {
                result++;
            }
            if (checkOperator.eqri(mycase)) {
                result++
            }
            if (checkOperator.eqrr(mycase)) {
                result++;
            }

            if (result >= 3) {
                numberOfCases++;
            }
        }

        return numberOfCases;
    }

    execute() {
        const fs = require('fs');
        const path = require('path');


        // let filepath = path.join(__dirname, "../test_input.txt");
        let filepath = path.join(__dirname, "../day16_input.txt");
        let lines = fs.readFileSync(filepath, "utf-8").split("\r\n");

        let checkCasesCollection: Array<CheckCase> = [];
        let checkOperator: Operator = new Operator();        

        for (var ldx = 0; ldx < lines.length; ldx += 4) {
            let tmpline = lines[ldx];
            if (tmpline != "") {
                let before = [];
                let input = [];
                let after = [];
                let regExp = new RegExp("-?\\d+", "g");
                for (var counter = 0; counter < 3; counter++) {
                    tmpline = lines[ldx + counter];
                    // var foundValues = tmpline.match(regExp);
                    let numberParts = tmpline.match(regExp);
                    if ((ldx + counter) % 4 == 0) {
                        before.push(parseInt(numberParts[0]));
                        before.push(parseInt(numberParts[1]));
                        before.push(parseInt(numberParts[2]));
                        before.push(parseInt(numberParts[3]));
                    } else if ((ldx + counter) % 4 == 1) {
                        input.push(parseInt(numberParts[0]));
                        input.push(parseInt(numberParts[1]));
                        input.push(parseInt(numberParts[2]));
                        input.push(parseInt(numberParts[3]));
                    } else if ((ldx + counter) % 4 == 2) {
                        after.push(parseInt(numberParts[0]));
                        after.push(parseInt(numberParts[1]));
                        after.push(parseInt(numberParts[2]));
                        after.push(parseInt(numberParts[3]));
                    }
                }
                checkCasesCollection.push(new CheckCase(before, input, after));
            } else {
                break;
            }
        }

        let resultPart1 = this.calculateCase(checkCasesCollection, checkOperator);

        //console.log(`Part 1: ${resultPart1}`);
        return resultPart1;
    }
}

