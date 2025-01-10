import { Element } from "./Element";

export class Coord {
    coordX: number;
    coordY: number;
    isFree: boolean;
    element: Element;

    constructor (xpos: number, ypos: number) {
        this.coordX = xpos;
        this.coordY = ypos;
    }
    
    isEqual(target: Coord | undefined) {
        return target != undefined && this.coordX == target.coordX && this.coordY == target.coordY;
    }
 }
