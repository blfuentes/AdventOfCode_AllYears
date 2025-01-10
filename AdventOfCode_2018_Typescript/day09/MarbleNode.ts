export class MarbleNode {
    value: number;
    nextMarble: MarbleNode;
    prevMarble: MarbleNode;

    constructor (input: number) {
        this.value = input;
    }
}