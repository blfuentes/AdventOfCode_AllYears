import { Element } from "./Element";

export class Coord {
    coordX: number;
    coordY: number;
    isFree: boolean;
    element: Element;
    distance: number;

    constructor (xpos: number, ypos: number) {
        this.coordX = xpos;
        this.coordY = ypos;
        this.distance = 0;
    }
    isEqual(target: Coord | undefined) {
        return target != undefined && this.coordX == target.coordX && this.coordY == target.coordY;
    }
}