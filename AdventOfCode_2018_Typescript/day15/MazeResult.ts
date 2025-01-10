import { Coord } from "./Coord";

export class MazeResult {
    position: Coord;
    path: Array<Coord>;

    constructor (_position: Coord, _path: Array<Coord>) {
        this.position = _position;
        this.path = _path;
    }
}

export class MazePoint {
    position: Coord;
    parent?: MazePoint | null;

    constructor (_position: Coord, _parent: MazePoint | null) {
        this.position = _position;
        this.parent = _parent;
    }
}