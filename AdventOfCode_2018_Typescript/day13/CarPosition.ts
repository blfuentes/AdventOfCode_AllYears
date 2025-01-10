export class CarPosition {
    cardId: number;
    state: string;
    coordX: number;
    coordY: number;
    numIntersections: number;
    isAlive: boolean;

    // Intersections 
    // 0 => Left
    // 1 => Straight
    // 2 => Right

    constructor (id: number, value: string, coordinate: Array<number>) {
        this.cardId = id;
        this.state = value;
        this.coordX = coordinate[0];
        this.coordY = coordinate[1];
        this.numIntersections = 0;
        this.isAlive = true;
    }

    getNextMove() {
        return this.numIntersections % 3;
    }

    setOrientation(roadType: string) {
        let nextMove = this.getNextMove();
        switch (this.state) {
            case ">" :
                if ((nextMove == 0 && roadType == "+") || roadType == "/") {
                    this.state = "^";
                } else if ((nextMove == 2 && roadType == "+") || roadType == "\\") {
                    this.state = "v";
                }
                break;
            case "v" :
                if ((nextMove == 0 && roadType == "+") || roadType == "\\") {
                    this.state = ">";
                } else if ((nextMove == 2 && roadType == "+") || roadType == "/") {
                    this.state = "<";
                }
                break;
            case "<" :
                if ((nextMove == 0 && roadType == "+") || roadType == "/") {
                    this.state = "v";
                } else if ((nextMove == 2 && roadType == "+") || roadType == "\\") {
                    this.state = "^";
                }
                break;
            case "^" :
                if ((nextMove == 0 && roadType == "+") || roadType == "\\") {
                    this.state = "<";
                } else if ((nextMove == 2 && roadType == "+") || roadType == "/") {
                    this.state = ">";
                }   
                break;
        }
    }

    getNextCoordinate () {
        let nextCoord:Array<number> = [];
        switch (this.state) {
            case ">" :
                nextCoord = [this.coordX + 1, this.coordY];
                break;
            case "v" :
                nextCoord = [this.coordX, this.coordY + 1];
                break;
            case "<" :
                nextCoord = [this.coordX - 1, this.coordY];
                break;
            case "^" :
                nextCoord = [this.coordX, this.coordY - 1];
                break;
        }
        return nextCoord;
    }
}