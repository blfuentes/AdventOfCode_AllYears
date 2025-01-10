export class Coordinate {
    coordId: number;
    innerCoord: Array<number>;
    isBoundary: boolean;

    manhattanDistances: Array<Array<number>>;
    valueDistances: number;

    constructor (id: number, input: Array<number>) {
        this.coordId = id;
        this.innerCoord = input;
        this.manhattanDistances = [];
        this.valueDistances = 0;
    }

    SetBoundary(minx: number, miny: number, maxx:number, maxy:number) {
        this.isBoundary = (this.innerCoord[1] == miny ||
                            this.innerCoord[1] == maxy ||
                            this.innerCoord[0] == minx ||
                            this.innerCoord[0] == maxx)
    }

    toString() {
        return `[${this.innerCoord[0]},${this.innerCoord[1]}]`;
    }

    calculateManhattanDistance(origin: Coordinate, end: Coordinate) {

    }
}