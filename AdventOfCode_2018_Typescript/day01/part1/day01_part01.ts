export class Day01Part01 {
    execute() {
        let fs = require("fs");
        let path = require('path');

        let changes = [];
        let result: number = 0;

        let filepath = path.join(__dirname, "../day01_part01_input.txt");

        let text = fs.readFileSync(filepath, "utf-8");
        changes = text.split("\r\n");

        let newValue: number = 0;

        for (let value of changes) {
            newValue = result + parseInt(value);
            //console.log(`Current frecuency ${result}, change of ${value}; resulting frecuency ${newValue}`);
            result = newValue;
        }

        return result;
        //console.log(`Final value: ${result}.`);
    }
}