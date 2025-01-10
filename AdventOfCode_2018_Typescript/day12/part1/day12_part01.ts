import { PotState } from "../PotState";

export class Day12Part01 {
    execute() {
        let fs = require("fs");
        let path = require('path');


        // let filepath = path.join(__dirname, "../test12_input.txt");
        let filepath = path.join(__dirname, "../day12_input.txt");
        let lines = fs.readFileSync(filepath, "utf-8").split("\r\n");

        let initialStateInput: string = lines[0].split(' ')[2];
        let potModifiers: Array<Array<string>> = [];
        let potStateCollection: Array<PotState> = [];

        let cPotState = 0;
        for (let pot of initialStateInput) {
            let newPotState = new PotState(pot, cPotState);
            newPotState.id = cPotState;
            cPotState++;
            potStateCollection.push(newPotState);
        }

        for (let ldx = 2; ldx < lines.length; ldx++) {
            potModifiers.push([lines[ldx].split(' ')[0], lines[ldx].split(' ')[2]]);
        }

        // let numberOfPlants = potStateCollection.filter(_p => _p.state == "#").length;
        for (let iteration = 1; iteration <= 20; iteration++) {
            // add four to the left
            let newPot_1 = new PotState(".", potStateCollection[0].id - 1);
            let newPot_2 = new PotState(".", potStateCollection[0].id - 2);
            let newPot_3 = new PotState(".", potStateCollection[0].id - 3);
            let newPot_4 = new PotState(".", potStateCollection[0].id - 4);
            potStateCollection.unshift(newPot_1);
            potStateCollection.unshift(newPot_2);
            potStateCollection.unshift(newPot_3);
            potStateCollection.unshift(newPot_4);

            // add four to the right
            let newPot_11 = new PotState(".", potStateCollection[potStateCollection.length - 1].id + 1);
            let newPot_22 = new PotState(".", potStateCollection[potStateCollection.length - 1].id + 2);
            let newPot_33 = new PotState(".", potStateCollection[potStateCollection.length - 1].id + 3);
            let newPot_44 = new PotState(".", potStateCollection[potStateCollection.length - 1].id + 4);
            potStateCollection.push(newPot_11);
            potStateCollection.push(newPot_22);
            potStateCollection.push(newPot_33);
            potStateCollection.push(newPot_44);

            let minIndex = potStateCollection.findIndex(_p => _p.state == "#");
            let maxIndex = 0;
            for (var idx = minIndex; idx < potStateCollection.length; idx++) {
                if (potStateCollection[idx].state == "#") {
                    maxIndex = idx;
                }
            }

            // mutate
            let newState: Array<string> = [];
            potStateCollection.forEach(_p => newState.push(_p.state));
            for (let idx = minIndex - 2; idx <= maxIndex + 3 && idx < potStateCollection.length; idx++) {
                let tocheck = potStateCollection[idx - 2].state + potStateCollection[idx - 1].state +
                    potStateCollection[idx].state +
                    potStateCollection[idx + 1].state + potStateCollection[idx + 2].state
                let found = potModifiers.find(_p => _p[0] == tocheck);
                if (found != undefined) {
                    newState[idx] = found[1];
                } else {
                    newState[idx] = ".";
                }
            }
            for (let idx = minIndex - 2; idx <= maxIndex + 3; idx++) {
                potStateCollection[idx].state = newState[idx];
            }
        }

        let numberOfPlants = 0;
        let filtered = potStateCollection.filter(_p => _p.state == "#");
        for (var idx = 0; idx < filtered.length; idx++) {
            numberOfPlants += filtered[idx].id;
        }
        //console.log(`Number of plants: ${numberOfPlants}`);
        return numberOfPlants;
    }
}

