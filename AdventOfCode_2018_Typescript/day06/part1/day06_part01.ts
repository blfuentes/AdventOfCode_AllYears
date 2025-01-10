import { Coordinate } from "../Coordinate";

export class Day06Part01 {
    calculateManhattanDistance(origin: Array<number>, end: Array<number>) {
        let distance: number;
        distance = Math.abs(origin[0] - end[0]) + Math.abs(origin[1] - end[1]);

        return distance;
    }

    execute() {
        let fs = require("fs");
        let path = require('path');
        // let filepath = path.join(__dirname, "../test01_input.txt");
        let filepath = path.join(__dirname, "../day06_input.txt");
        // let filepath = path.join(__dirname, "../day06_input2.txt");
        let text: string = fs.readFileSync(filepath, "utf-8");
        let lines = text.split("\r\n");

        let coordinates: Array<Coordinate> = [];

        // let firstCoord = new Array(parseInt(lines[0].split(",")[0]), parseInt(lines[0].split(",")[1]));
        // coordinates.push(firstCoord);
        let minX: number = 0;
        let minY: number = 0;
        let maxX: number = 0;
        let maxY: number = 0;

        for (var idx = 0; idx < lines.length; idx++) {
            let tmpArray = new Array(parseInt(lines[idx].split(",")[0]), parseInt(lines[idx].split(",")[1]));
            if (idx == 0) {
                minX = tmpArray[0];
                maxX = tmpArray[0];
                minY = tmpArray[1];
                maxY = tmpArray[1];
            } else {
                if (minY > tmpArray[1]) {
                    minY = tmpArray[1];
                }
                if (maxY < tmpArray[1]) {
                    maxY = tmpArray[1];
                }
                if (minX > tmpArray[0]) {
                    minX = tmpArray[0];
                }
                if (maxX < tmpArray[0]) {
                    maxX = tmpArray[0];
                }
            }
            let tmpCoordinate = new Coordinate(idx, tmpArray);
            coordinates.push(tmpCoordinate);
        }

        let boundaries: Array<Coordinate> = [];
        for (let coord of coordinates) {
            coord.SetBoundary(minX, minY, maxX, maxY);
            if (coord.isBoundary) {
                boundaries.push(coord);
            }
        }
        let fullMap: Array<Array<number>> = [];
        fullMap = Array(1000).fill(null).map(item => (new Array(1000).fill(-1)))
        let distancesMap: { [key: string]: Array<number> } = {}

        for (var idx = 0; idx < 1000; idx++) {
            for (var jdx = 0; jdx < 1000; jdx++) {
                let minDistance = maxX * maxY;
                let duplicated = false;
                let minCoord: Coordinate = new Coordinate(-1, [-1, -1]);
                for (var cdx = 0; cdx < coordinates.length; cdx++) {
                    let tmpDistance = this.calculateManhattanDistance(coordinates[cdx].innerCoord, [idx, jdx]);
                    if (cdx == 0) {
                        minDistance = tmpDistance;
                        duplicated = false;
                        minCoord = coordinates[cdx];
                        // break;
                    } else if (minDistance == tmpDistance) {
                        duplicated = true;
                    } else if (tmpDistance < minDistance) {
                        minDistance = tmpDistance;
                        duplicated = false;
                        minCoord = coordinates[cdx];
                    }
                }
                if (duplicated) {
                    fullMap[idx][jdx] = -1;
                    if (distancesMap[`${idx},${jdx}`] == undefined) {
                        distancesMap[`${idx},${jdx}`] = new Array(-1, minDistance);
                    } else {
                        distancesMap[`${idx},${jdx}`][0] = -1;
                        distancesMap[`${idx},${jdx}`][1] = minDistance;
                    }

                } else {
                    fullMap[idx][jdx] = minCoord.coordId;
                    if (distancesMap[`${idx},${jdx}`] == undefined) {
                        distancesMap[`${idx},${jdx}`] = new Array(minCoord.coordId, minDistance);
                    } else {
                        distancesMap[`${idx},${jdx}`][0] = minCoord.coordId;
                        distancesMap[`${idx},${jdx}`][1] = minDistance;
                    }
                    if (idx > minX && idx < maxX &&
                        jdx > minY && jdx < maxY && !minCoord.isBoundary) {
                        minCoord.valueDistances++;
                    }
                    else {
                        minCoord.valueDistances = 0;
                    }
                }
            }
        }

        let maxArea: number = 0;
        for (let coord of coordinates.filter(_c => !_c.isBoundary)) {
            if (maxArea < coord.valueDistances) {
                maxArea = coord.valueDistances;
            }
        }

        // for (var row = 0; row < 20; row++) {
        //     for (var col = 0; row < 20; col++) {
        //         console.log(`${fullMap[row][col]}`);
        //     }
        //     console.log(``);
        // }

        //console.log(`Final result part 1: ${maxArea}`);
        return maxArea;
    }
}

