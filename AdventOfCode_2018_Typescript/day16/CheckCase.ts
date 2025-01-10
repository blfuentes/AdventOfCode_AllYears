export class CheckCase {
    before: Array<number>;
    input: Array<number>;
    after: Array<number>;

    constructor (_before: Array<number>, _input: Array<number>, _after: Array<number>) {
        // before
        this.before = [];
        for (let value of _before) {
            this.before.push(value);
        }

        // input
        this.input = [];
        for (let value of _input) {
            this.input.push(value);
        }

        // after
        this.after = [];
        for (let value of _after) {
            this.after.push(value);
        }
    } 
}