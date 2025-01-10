class Counter {
    element: string;
    numberOfAppearances: number;

    constructor(value: string) {
        this.element = value;
        this.numberOfAppearances = 1;
    }
}
class Entry {
    entryValue: string;
    description: Array<Counter>;
    candidatesTwo: Array<Counter>;
    candidatesThree: Array<Counter>;
    possibleEquals: Array<Entry>;

    constructor(value: string) {
        this.entryValue = value;
        this.description = [];
        this.candidatesTwo = [];
        this.candidatesThree = [];
        this.possibleEquals = [];
    }

    analyze() {
        for (var character of this.entryValue) {
            let index = this.description.findIndex((e) => {
                return e.element.indexOf(character) > -1;
            });
            if (index > -1) {
                ++this.description[index].numberOfAppearances;
            } else {
                let tmpNewCounter = new Counter(character);
                this.description.push(tmpNewCounter);
            }
        }
    }

    findCandidates() {
        for (let candidate of this.description) {
            if (candidate.numberOfAppearances === 2) {
                this.candidatesTwo.push(candidate);
            } else if (candidate.numberOfAppearances === 3) {
                this.candidatesThree.push(candidate);
            }
        }
    }
}

export class Day02Part02 {
    execute() {
        var fs = require("fs");
        var path = require('path');

        let entryElements = [];
        let filepath = path.join(__dirname, "../day02_input.txt");
        // let filepath = path.join(__dirname, "./test_02.txt");
        let text = fs.readFileSync(filepath, "utf-8");
        entryElements = text.split("\r\n");

        let entries: Array<Entry> = [];
        let numberOfTwo: number = 0;
        let numberOfThree: number = 0;

        for (let value of entryElements) {
            let tmpEntry = new Entry(value);
            tmpEntry.analyze();
            entries.push(tmpEntry);
        }

        for (let entry of entries) {
            entry.findCandidates();

            if (entry.candidatesTwo.length > 0) {
                ++numberOfTwo;
            }
            if (entry.candidatesThree.length > 0) {
                ++numberOfThree;
            }
        }

        //console.log(`Checksum: ${numberOfTwo * numberOfThree}.`);

        for (var idx = 0; idx < entries.length; idx++) {
            var tmpEntry = entries[idx];
            for (var jdx = idx + 1; jdx < entries.length; jdx++) {
                var entryToCompare = entries[jdx];
                var cdifferences = 0;
                var issimilar = true;
                let result: string = "";
                for (var kdx = 0; kdx < tmpEntry.entryValue.length; kdx++) {
                    if (tmpEntry.entryValue[kdx] !== entryToCompare.entryValue[kdx]) {
                        cdifferences++;
                    } else {
                        result = result + tmpEntry.entryValue[kdx];
                    }
                    if (cdifferences > 1) {
                        issimilar = false;
                        break;
                    }
                }
                if (issimilar) {
                    //console.log(`Similar found ${result.toString()}.\n`)
                    return result;
                }
            }
        }
    }
}