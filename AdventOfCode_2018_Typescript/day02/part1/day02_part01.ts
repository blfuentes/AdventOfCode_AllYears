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
    constructor(value: string) {
        this.entryValue = value;
        this.description = [];
        this.candidatesTwo = [];
        this.candidatesThree = [];
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

export class Day02Part01 {
    execute() {
        let fs = require("fs");
        let path = require('path');

        let entryElements = [];
        let filepath = path.join(__dirname, "../day02_input.txt");
        // let filepath = path.join(__dirname, "./test_01.txt");
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
        return numberOfTwo * numberOfThree;
    }
}

