import { CheckCase } from "./CheckCase";

export enum OperatorType {
    NONE = -1,
    ADDR = 0,
    ADDI = 1,
    MULR = 2,
    MULI = 3,
    BANR = 4,
    BANI = 5,
    BORR = 6,
    BORI = 7,
    SETR = 8,
    SETI = 9,
    GTIR = 10,
    GTRI = 11,
    GTRR = 12,
    EQIR = 13,
    EQRI = 14,
    EQRR = 15
}

export class OperatorContainer {
    operator: OperatorType;
    oppCodes: Array<number>;
    
    constructor () {
        this.oppCodes = [];
    }
}

export class Operator {
    addr(cCase: CheckCase) {
        let output = cCase.before[cCase.input[1]] + cCase.before[cCase.input[2]];
        return output == cCase.after[cCase.input[3]];
    }
    executeADDR(operation: Array<number>, result: Array<number>) {
        result[operation[3]] = result[operation[1]] + result[operation[2]];
    }
    addi(cCase: CheckCase) {
        let output = cCase.before[cCase.input[1]] + cCase.input[2];
        return output == cCase.after[cCase.input[3]];
    }
    executeADDI(operation: Array<number>, result: Array<number>) {
        result[operation[3]] = result[operation[1]] + operation[2];
    }

    mulr(cCase: CheckCase) {
        let output = cCase.before[cCase.input[1]] * cCase.before[cCase.input[2]]
        return output == cCase.after[cCase.input[3]];
    }
    executeMULR(operation: Array<number>, result: Array<number>) {
        result[operation[3]] = result[operation[1]] * result[operation[2]];
    }
    muli(cCase: CheckCase) {
        let output = cCase.before[cCase.input[1]] * cCase.input[2];
        return output == cCase.after[cCase.input[3]];
    }
    executeMULI(operation: Array<number>, result: Array<number>) {
        result[operation[3]] = result[operation[1]] * operation[2];
    }

    banr(cCase: CheckCase) {
        let output =   cCase.before[cCase.input[1]] & cCase.before[cCase.input[2]];
        return output == cCase.after[cCase.input[3]];
    }
    executeBANR(operation: Array<number>, result: Array<number>) {
        result[operation[3]] = result[operation[1]] & result[operation[2]];
    }
    bani(cCase: CheckCase) {
        let output = cCase.before[cCase.input[1]] & cCase.input[2];
        return output == cCase.after[cCase.input[3]];
    }
    executeBANI(operation: Array<number>, result: Array<number>) {
        result[operation[3]] = result[operation[1]] & operation[2];
    }

    borr(cCase: CheckCase) {
        let output =   cCase.before[cCase.input[1]] | cCase.before[cCase.input[2]];
        return output == cCase.after[cCase.input[3]];
    }
    executeBORR(operation: Array<number>, result: Array<number>) {
        result[operation[3]] = result[operation[1]] | result[operation[2]];
    }
    bori(cCase: CheckCase) {
        let output = cCase.before[cCase.input[1]] | cCase.input[2];
        return output == cCase.after[cCase.input[3]];
    }
    executeBORI(operation: Array<number>, result: Array<number>) {
        result[operation[3]] = result[operation[1]] | operation[2];
    }

    setr(cCase: CheckCase) {
        let output = cCase.before[cCase.input[1]];
        return output == cCase.after[cCase.input[3]];
    }
    executeSETR(operation: Array<number>, result: Array<number>) {
        result[operation[3]] = result[operation[1]];
    }
    seti(cCase: CheckCase) {
        let output = cCase.input[1];
        return output == cCase.after[cCase.input[3]];
    }
    executeSETI(operation: Array<number>, result: Array<number>) {
        result[operation[3]] = operation[1];
    }

    gtir(cCase: CheckCase) {
        let output = 0;
        if (cCase.input[1] > cCase.before[cCase.input[2]]) {
            output = 1;
        }
        return output == cCase.after[cCase.input[3]];
    }
    executeGTIR(operation: Array<number>, result: Array<number>) {
        let output = 0;
        if (operation[1] > result[operation[2]]) {
            output = 1;
        }
        result[operation[3]] = output;
    }
    gtri(cCase: CheckCase) {
        let output = 0;
        if (cCase.before[cCase.input[1]] > cCase.input[2]) {
            output = 1;
        }
        return output == cCase.after[cCase.input[3]];
    }
    executeGTRI(operation: Array<number>, result: Array<number>) {
        let output = 0;
        if (result[operation[1]] > operation[2]) {
            output = 1;
        }
        result[operation[3]] = output;
    }
    gtrr(cCase: CheckCase) {
        let output = 0;
        if (cCase.before[cCase.input[1]] > cCase.before[cCase.input[2]]) {
            output = 1;
        }
        return output == cCase.after[cCase.input[3]];
    }
    executeGTRR(operation: Array<number>, result: Array<number>) {
        let output = 0;
        if (result[operation[1]] > result[operation[2]]) {
            output = 1;
        }
        result[operation[3]] = output;
    }

    eqir(cCase: CheckCase) {
        let output = 0;
        if (cCase.input[1] == cCase.before[cCase.input[2]]) {
            output = 1;
        }
        return output == cCase.after[cCase.input[3]];
    }
    executeEQIR(operation: Array<number>, result: Array<number>) {
        let output = 0;
        if (operation[1] == result[operation[2]]) {
            output = 1;
        }
        result[operation[3]] = output;
    }
    eqri(cCase: CheckCase) {
        let output = 0;
        if (cCase.before[cCase.input[1]] == cCase.input[2]) {
            output = 1;
        }
        return output == cCase.after[cCase.input[3]];
    }
    executeEQRI(operation: Array<number>, result: Array<number>) {
        let output = 0;
        if (result[operation[1]] == operation[2]) {
            output = 1;
        }
        result[operation[3]] = output;
    }
    eqrr(cCase: CheckCase) {
        let output = 0;
        if (cCase.before[cCase.input[1]] == cCase.before[cCase.input[2]]) {
            output = 1;
        }
        return output == cCase.after[cCase.input[3]];
    }
    executeEQRR(operation: Array<number>, result: Array<number>) {
        let output = 0;
        if (result[operation[1]] == result[operation[2]]) {
            output = 1;
        }
        result[operation[3]] = output;
    }
}