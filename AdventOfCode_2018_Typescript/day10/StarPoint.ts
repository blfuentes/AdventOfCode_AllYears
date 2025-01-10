export class StarPoint {
    position: Array<number>;
    velocity: Array<number>;

    constructor(initPos: Array<number>, initVelocity: Array<number>){
        // this.position = new Array(Math.floor(initPos[0]), Math.floor(initPos[1]));
        this.position = new Array(initPos[0], initPos[1]);
        this.velocity = new Array(initVelocity[0], initVelocity[1]);
    }

    calculatePosition(second: number) {
        // this.position[0] = this.initialPosition[0] + this.velocity[0] * second;
        // this.position[1] = this.initialPosition[1] + this.velocity[1] * second;        
        this.position[0] += this.velocity[0];// * second;
        this.position[1] += this.velocity[1];// * second;
    }
}