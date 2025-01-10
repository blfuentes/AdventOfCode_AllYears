export class Day08Part02 {
    MetadataValue: number = 0;
    Sumatory: number = 0;
    InitIdxRead: number = 0;

    calculateSum(inputArray: number[]) {
        const children = inputArray[this.InitIdxRead++];
        const metadata = inputArray[this.InitIdxRead++];
        let sum = 0;
        if (children == 0) {
            for (let idxMetadata = 0; idxMetadata < metadata; idxMetadata++) {
                const value = inputArray[this.InitIdxRead++];
                this.MetadataValue += value;
                sum += value;
            }
        } else {
            const metadataValues: Array<number> = [];
            for (let counterChild = 0; counterChild < children; counterChild++) {
                metadataValues.push(this.calculateSum(inputArray));
            }
            for (let counterMetadata = 0; counterMetadata < metadata; counterMetadata++) {
                const value = inputArray[this.InitIdxRead++];
                this.MetadataValue += value;
                if (value >= 1 && value <= metadataValues.length) {
                    sum += metadataValues[value - 1];
                }
            }
        }

        return sum;
    }

    execute() {
        let fs = require("fs");
        let path = require('path');

        // let filepath = path.join(__dirname, "../test01_input.txt");
        let filepath = path.join(__dirname, "../day08_input.txt");
        let text: string = fs.readFileSync(filepath, "utf-8");
        let inputArray = text.split(" ").map(e => parseInt(e));

        this.Sumatory = this.calculateSum(inputArray);

        return this.Sumatory;
    }
}
