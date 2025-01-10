export class GuardEvent {
    description: string;
    action: string;
    occuredAt: Date;
    guardId: number;

    hours: number;
    minutes: number;

    isAwake: boolean;

    constructor(value: string) {
        this.description = value;
        let tmpValues = value.split("] ");
        this.occuredAt = new Date(tmpValues[0].replace("[", ""));
        this.hours = this.occuredAt.getHours();
        this.minutes = this.occuredAt.getMinutes();
        this.action = tmpValues[1];
        this.isAwake = this.action.indexOf("asleep") == -1;
    }
}

export class GuardInfo {
    guardId: number;
    timeSleeping: number;
    minutesSleeping: Array<number>;

    constructor(id: number) {
        this.guardId = id;
        this.timeSleeping = 0;
        this.minutesSleeping = Array(60).fill(0);
    }
}