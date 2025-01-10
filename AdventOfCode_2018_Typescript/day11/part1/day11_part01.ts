export class Day11Part01 {
    execute() {
        // let puzzleInput: number = 42;
        let puzzleInput: number = 2866;

        let fuelGrid: Array<Array<number>>;
        let fuelGridSquareValues: Array<Array<number>>;

        fuelGrid = Array(300).fill(null).map(item => (new Array(300).fill(0)));
        fuelGridSquareValues = Array(300).fill(null).map(item => (new Array(300).fill(0)));

        function getFuel(input: number, coord: Array<number>) {
            let fuelResult = 0;
            let rackID = coord[0] + 10;

            fuelResult = rackID;
            fuelResult *= coord[1];
            fuelResult += input;
            fuelResult *= rackID;

            let hundreds = Math.floor(fuelResult / 100 % 10);
            fuelResult = hundreds - 5;

            return fuelResult;
        }

        for (let column = 0; column < 300; column++) {
            for (let row = 0; row < 300; row++) {
                fuelGrid[column][row] = getFuel(puzzleInput, [column, row])
                fuelGridSquareValues[column][row] += fuelGrid[column][row];
                if (column - 1 >= 0) {
                    fuelGridSquareValues[column - 1][row] += fuelGrid[column][row];
                    if (row - 1 >= 0) {
                        fuelGridSquareValues[column - 1][row - 1] += fuelGrid[column][row];
                    }
                    if (row + 1 < 300) {
                        fuelGridSquareValues[column - 1][row + 1] += fuelGrid[column][row];
                    }
                }
                if (column + 1 < 300) {
                    fuelGridSquareValues[column + 1][row] += fuelGrid[column][row];
                    if (row - 1 >= 0) {
                        fuelGridSquareValues[column + 1][row - 1] += fuelGrid[column][row];
                    }
                    if (row + 1 < 300) {
                        fuelGridSquareValues[column + 1][row + 1] += fuelGrid[column][row];
                    }
                }
                if (row - 1 >= 0) {
                    fuelGridSquareValues[column][row - 1] += fuelGrid[column][row];
                }
                if (row + 1 < 300) {
                    fuelGridSquareValues[column][row + 1] += fuelGrid[column][row];
                }
            }
        }
        let maxValue = 0;
        let coordMax = [0, 0];
        for (let column = 0; column < 300; column++) {
            for (let row = 0; row < 300; row++) {
                if (fuelGridSquareValues[column][row] > maxValue) {
                    maxValue = fuelGridSquareValues[column][row];
                    coordMax = [column, row];
                }
            }
        }

        // console.log(`Cell [3, 5], grid serial number 8: power level ${getFuel(8, [3, 5])}.`);
        // console.log(`Cell [122, 79], grid serial number 57: power level ${getFuel(57, [122, 79])}.`);
        // console.log(`Cell [217, 196], grid serial number 39: power level ${getFuel(39, [217, 196])}.`);
        // console.log(`Cell [101, 153], grid serial number 71: power level ${getFuel(71, [101, 153])}.`);

        coordMax[0] -= 1;
        coordMax[1] -= 1;
        //console.log(`Part 1--> Highest coordinate ${coordMax.toString()} with ${maxValue}.`);
        return coordMax.toString();
    }
}
