export class Day21Part02 {
    execute() {
        const fs = require('fs');
        const path = require('path');

        // let filepath = path.join(__dirname, "../test_input.txt");
        let filepath = path.join(__dirname, "../day21_input.txt");
        let lines = fs.readFileSync(filepath, "utf-8").split("\r\n");

        return 0;
    }
}