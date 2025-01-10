export class Day08Part01 {
    metadataValue: number = 0;
    initIdxRead: number = 0;

    calculateSum(inputArray: number[]) {
        const children = inputArray[this.initIdxRead++];
        const metadata = inputArray[this.initIdxRead++];
        let sum = 0;
        if (children == 0) {
            for (let idxMetadata = 0; idxMetadata < metadata; idxMetadata++) {
                const value = inputArray[this.initIdxRead++];
                this.metadataValue += value;
                sum += value;
            }
        } else {
            for (let counterChild = 0; counterChild < children; counterChild++) {
                this.calculateSum(inputArray);
            }
            for (let counterMetadata = 0; counterMetadata < metadata; counterMetadata++) {
                const value = inputArray[this.initIdxRead++];
                this.metadataValue += value;
            }
        }

        return sum;
    }

    execute() {
        let fs = require("fs");
        let path = require('path');

        //let filepath = path.join(__dirname, "../test01_input.txt");
        let filepath = path.join(__dirname, "../day08_input.txt");
        let text: string = fs.readFileSync(filepath, "utf-8");
        let inputArray = text.split(" ").map(e => parseInt(e));

        this.calculateSum(inputArray);

        return this.metadataValue;
    }
}

