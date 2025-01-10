export class PotState {
    id: number;
    state: string;
    next: PotState;
    prev: PotState;

    constructor (value: string, id: number) {
        this.id = id;
        this.state = value;
    }
}