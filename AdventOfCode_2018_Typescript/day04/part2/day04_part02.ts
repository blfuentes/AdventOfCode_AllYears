import { GuardEvent, GuardInfo } from "../GuardEvent";

export class Day04Part02 {
    execute() {
        let fs = require("fs");
        let path = require('path');

        let entryElements = [];
        let filepath = path.join(__dirname, "../day04_input.txt");
        // let filepath = path.join(__dirname, "../test01_input.txt");
        // let filepath = path.join(__dirname, "../test02_input.txt");
        let text = fs.readFileSync(filepath, "utf-8");
        entryElements = text.split("\r\n");

        let guardEvents: GuardEvent[] = []
        let eventsByGuardId: { [key: number]: Array<GuardEvent> } = {}
        let guardInfo: { [key: number]: GuardInfo } = {}
        let guardIds: Array<number> = []

        function findGuardId(message: string): number {
            let result: number = -1;
            let regexp = new RegExp("#\\d+")

            var matched = message.match(regexp);
            if (matched !== null) {
                result = parseInt(matched[0].replace("#", ""));
            } else {
                result = -1;
            }

            return result;
        }

        function getMostSleepGuardValue(ids: Array<number>, guardInfo: { [key: number]: GuardInfo }): number {
            let guardId = 0;
            let maxTime = 0;
            let minute = 0;
            for (let id of ids) {
                var tmpGuard = guardInfo[id];
                if (tmpGuard.timeSleeping > maxTime) {
                    maxTime = tmpGuard.timeSleeping;
                    guardId = id;
                    minute = tmpGuard.minutesSleeping.indexOf(Math.max.apply(null, tmpGuard.minutesSleeping));
                }
            }

            return guardId * minute;
        }

        function getMostRepeatedSleepGuardValue(ids: Array<number>, guardInfo: { [key: number]: GuardInfo }): number {
            let guardId = 0;
            let maxMinute = 0;
            let minute = 0;
            for (let id of ids) {
                var tmpGuard = guardInfo[id];
                var tmpMaxMinute = Math.max.apply(null, tmpGuard.minutesSleeping);
                if (maxMinute < tmpMaxMinute) {
                    minute = tmpGuard.minutesSleeping.indexOf(tmpMaxMinute);
                    maxMinute = tmpMaxMinute;
                    guardId = id;
                }
            }

            return guardId * minute;
        }

        // create events
        for (let event of entryElements) {
            var tmpEvent = new GuardEvent(event);
            guardEvents.push(tmpEvent);
        }

        // sort events
        guardEvents = guardEvents.sort(function (a, b) {
            return (a.occuredAt.valueOf() - b.occuredAt.valueOf());
        });

        // assign ids and initialize guardinfo array
        let currentGuardId = 0;
        for (let event of guardEvents) {
            var tmpGuardId = findGuardId(event.action);
            if (tmpGuardId > -1) {
                currentGuardId = tmpGuardId;
                if (eventsByGuardId[currentGuardId] == null) {
                    eventsByGuardId[currentGuardId] = new Array<GuardEvent>();
                    guardInfo[currentGuardId] = new GuardInfo(currentGuardId);
                }
                if (guardIds.indexOf(currentGuardId) == -1) {
                    guardIds.push(currentGuardId);
                }
            }
            event.guardId = currentGuardId;
            eventsByGuardId[currentGuardId].push(event);
            // console.log(`Guard ${event.guardId} :: ${event.action} at ${event.occuredAt}.`);
        }

        // check each guard
        let start = 0;
        let end = 0;
        let tmpIsSleeping = false;
        for (let event of guardEvents) {
            currentGuardId = event.guardId;
            if (!event.isAwake) {
                tmpIsSleeping = true;
                start = event.minutes;
            } else {
                if (tmpIsSleeping) {
                    end = event.minutes;

                    while (start < end) {
                        guardInfo[currentGuardId].timeSleeping++;
                        guardInfo[currentGuardId].minutesSleeping[start]++;
                        start++;
                    }
                    tmpIsSleeping = false;
                    start = 0;
                    end = 0;
                }
            }
        }

        let finalResultPart1 = getMostSleepGuardValue(guardIds, guardInfo);
        let finalResultPart2 = getMostRepeatedSleepGuardValue(guardIds, guardInfo);
        //console.log(`Result part1: ${finalResultPart1}. Result part2: ${finalResultPart2}.`);
        return finalResultPart2;
    }
}

