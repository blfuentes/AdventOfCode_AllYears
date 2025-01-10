import { Coord } from "./Coord";

export enum ElementType {
    FIELD,
    WALL,
    GOBLIN,
    ELF
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
            case ElementType.ELF:
                return "E";
            case ElementType.FIELD:
                return ".";
            case ElementType.GOBLIN:
                return "G";
            case ElementType.WALL:
                return "#";
        }
    }
}

export class Field extends Element {
    constructor (location: Coord) {
        super(location, ElementType.FIELD);
    }
}

export class Wall extends Element {
    constructor (location: Coord) {
        super (location, ElementType.WALL);
    }
}

export class Player extends Element {
    isAlive: boolean;
    HP: number;
    AP: number;

    EnemiesPositions: Array<Coord>;

    NextPosition: Coord;

    constructor (location: Coord, type: ElementType) {
        super(location, type);
        this.isAlive = true;
        this.HP = 200;
        this.AP = 3;
        this.EnemiesPositions = new Array<Coord>();        
    }

    showStats() {
        return `${this.display} (${this.HP})`;
    }
}

export class Elf extends Player {
    constructor (location: Coord) {
        super (location, ElementType.ELF)
    }
}

export class Goblin extends Player {
    constructor (location: Coord) {
        super (location, ElementType.GOBLIN);
    }
}