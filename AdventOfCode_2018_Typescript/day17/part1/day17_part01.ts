export class Day17Part01 {
    readGroundComposition() {

    }

    displayGroundComposition() {

    }
    execute() {
        const fs = require('fs');
        const path = require('path');

        let filepath = path.join(__dirname, "../test_input.txt");
        // let filepath = path.join(__dirname, "../day17_input.txt");
        let lines = fs.readFileSync(filepath, "utf-8").split("\r\n");





        //
        this.readGroundComposition();

        //
        //console.log(`finished!`);
        return 0;
    }
}

