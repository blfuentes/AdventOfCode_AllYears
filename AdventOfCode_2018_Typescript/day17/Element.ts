import { Coord } from "./Coord";

export enum ElementType {
    FOUNTAIN,
    RUNNING_WATER,
    WATER,
    CLAY,
    FREE
}

export class Element {
    position: Coord;
    elementType: ElementType;
    constructor (location: Coord, type: ElementType) {
        this.position = location;
        this.elementType = type;
    }

    display() {
        switch (this.elementType) {
            case ElementType.FOUNTAIN:
                return "+";
            case ElementType.RUNNING_WATER:
                return "|";
            case ElementType.WATER:
                return "~";
            case ElementType.CLAY:
                return "#";
            case ElementType.FREE:
                return ".";
        }
    }
}