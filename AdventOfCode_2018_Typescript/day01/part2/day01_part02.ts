export class Day01Part02 {
    execute() {
        let fs = require("fs");
        let path = require('path');

        let changes = [];
        let result: number = 0;

        let filepath = path.join(__dirname, "../day01_part01_input.txt");
        // var filepath = path.join(__dirname, "../test02.txt");
        // var filepath = path.join(__dirname, "./test03.txt");
        // var filepath = path.join(__dirname, "./test04.txt");

        let text = fs.readFileSync(filepath, "utf-8");
        changes = text.split("\r\n");

        let newValue: number = 0;
        let calculatedFrecuencies: number[] = [];

        let duplicatedFound: boolean = false;
        let numberofloops = 0;
        do {
            for (let value of changes) {
                newValue = result + parseInt(value);
                if (calculatedFrecuencies.indexOf(newValue) > -1) {
                    duplicatedFound = true;
                }
                //console.log(`Current frecuency ${result}, change of ${value}; resulting frecuency ${newValue}`);
                result = newValue;
                calculatedFrecuencies.push(newValue);
                if (duplicatedFound) {
                    break;
                }
            }
            numberofloops++;
        } while (!duplicatedFound);
        //console.log(`Final value: ${result}. Duplicated frecuency: ${newValue}. Loops: ${numberofloops}.`);
        return result;
    }
}