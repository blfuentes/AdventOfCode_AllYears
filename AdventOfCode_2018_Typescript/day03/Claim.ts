export class Claim {
    description: string;
    id: number;
    initLeft: number;
    initTop: number;
    width: number;
    height: number;

    usedArea: Array<Array<number>>;

    constructor (value: string) {
        // #123 @ 3,2: 5x4
        this.description = value;
        var tmpValues = value.split(" ");
        this.id = parseInt(tmpValues[0].replace("#", ""));
        this.initLeft = parseInt(tmpValues[2].split(",")[0]);
        this.initTop = parseInt(tmpValues[2].split(",")[1].replace(":", ""));
        this.width = parseInt(tmpValues[3].split("x")[0]);
        this.height = parseInt(tmpValues[3].split("x")[1]);
    }
}
