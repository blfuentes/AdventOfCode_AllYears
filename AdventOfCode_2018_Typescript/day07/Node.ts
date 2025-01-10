export class Node {
    element: string;
    parentNodes: Array<Node>;
    childNodes: Array<Node>;

    duration: number;
    remaining: number;

    constructor(value: string, parent: Node | undefined, baseDuration: number) {
        this.element = value;
        this.parentNodes = [];
        if (parent != undefined) {
            this.parentNodes.push();
        }
        this.childNodes = [];
        this.duration = (this.element.charCodeAt(0) - 64) + baseDuration;
    }
}