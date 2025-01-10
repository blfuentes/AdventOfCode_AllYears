export class Day22Part01 {
    execute() {
        const fs = require('fs');
        const path = require('path');

        // let filepath = path.join(__dirname, "../test_input.txt");
        let filepath = path.join(__dirname, "../day22_input.txt");
        let lines = fs.readFileSync(filepath, "utf-8").split("\r\n");

        return 0;
    }
}