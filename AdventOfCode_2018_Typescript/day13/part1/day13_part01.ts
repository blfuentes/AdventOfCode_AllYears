import { CarPosition } from "../CarPosition";

export class Day13Part01 {
    displayRoadMap(roadmap: Array<Array<string>>) {
        let rowIdx = 0;
        while (roadmap[rowIdx] && rowIdx < roadmap[rowIdx].length) {
            let rowDisplay = "";
            for (let colIdx = 0; colIdx < roadmap.length; colIdx++) {
                rowDisplay += roadmap[colIdx][rowIdx];
            }
            console.log(`${rowDisplay}`);
            rowIdx++;
        }
    }

    elementIsCard(element: string) {
        return element == ">" || element == "<" || element == "^" || element == "v";
    }

    execute() {
        let fs = require("fs");
        let path = require('path');

         //let filepath = path.join(__dirname, "../test13_input.txt");
        let filepath = path.join(__dirname, "../day13_input.txt");
        let lines = fs.readFileSync(filepath, "utf-8").split("\r\n");

        // INIT MAPROAD
        let roadMap: Array<Array<string>> = [];
        roadMap = Array(lines[0].length).fill(null).map(item => (new Array(lines.length).fill(" ")));
        let roadMapNoCars: Array<Array<string>> = [];
        roadMapNoCars = Array(lines[0].length).fill(null).map(item => (new Array(lines.length).fill(" ")));

        let carsPositions: Array<CarPosition> = [];

        // READ
        let rowIdx = 0;
        let cCard = 0;
        for (let line of lines) {
            let lineParts = line.split('');
            for (let column = 0; column < lineParts.length; column++) {
                roadMap[column][rowIdx] = lineParts[column];
                roadMapNoCars[column][rowIdx] = lineParts[column].replace("^", "|").replace("v", "|").replace(">", "-").replace("<", "-");
                if (this.elementIsCard(lineParts[column])) {
                    let newCar = new CarPosition(cCard++, lineParts[column], [column, rowIdx]);
                    carsPositions.push(newCar);
                }
            }
            rowIdx++;
        }

        // 
        function sortByPosition(a: CarPosition, b: CarPosition) {
            if (a.coordY == b.coordY) return a.coordX - b.coordX;
            return a.coordY - b.coordY;
        }

        let crashed = false;
        let coordCrashed: Array<number> = []
        //this.displayRoadMap(roadMap);
        do {
            crashed = false;
            carsPositions = carsPositions.sort((a, b) => sortByPosition(a, b));
            for (let car of carsPositions) {
                let originCharacter = roadMapNoCars[car.coordX][car.coordY];
                roadMap[car.coordX][car.coordY] = originCharacter;
                let nextCoord = car.getNextCoordinate();
                car.coordX = nextCoord[0];
                car.coordY = nextCoord[1];
                let nextOriginCharacter = roadMapNoCars[nextCoord[0]][nextCoord[1]];
                car.setOrientation(nextOriginCharacter);
                if (nextOriginCharacter == "+") {
                    car.numIntersections += 1;
                }
                let stateNextCoord = roadMap[nextCoord[0]][nextCoord[1]];
                if (stateNextCoord == "<" || stateNextCoord == ">" || stateNextCoord == "^" || stateNextCoord == "v") {
                    roadMap[car.coordX][car.coordY] = "X";
                    coordCrashed = [car.coordX, car.coordY];
                    crashed = true;
                    break;
                } else {
                    roadMap[car.coordX][car.coordY] = car.state;
                }
            }
        } while (!crashed);

        //console.log(`Part 1 crashed: ${coordCrashed.toString()}!`);
        return coordCrashed.toString();
    }
}

