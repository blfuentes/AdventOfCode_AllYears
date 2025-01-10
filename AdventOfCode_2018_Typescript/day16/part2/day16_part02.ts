import { CheckCase } from "../CheckCase";
import { OperatorType, Operator, OperatorContainer } from "../Operator";

export class Day16Part02 {

    calculateCase(checkCasesCollection, checkOperator, operatorDictionary) {
        let numberOfCases = 0;
        let uniqueOperator = OperatorType.NONE;
        for (let mycase of checkCasesCollection) {
            let opCode = mycase.input[0];
            let result = 0;
            if (checkOperator.addr(mycase)) {
                result++
                if (operatorDictionary[opCode].find(_o => _o == OperatorType.ADDR) == undefined) {
                    operatorDictionary[opCode].push(OperatorType.ADDR);
                }
            }
            if (checkOperator.addi(mycase)) {
                result++;
                if (operatorDictionary[opCode].find(_o => _o == OperatorType.ADDI) == undefined) {
                    operatorDictionary[opCode].push(OperatorType.ADDI);
                }
            }

            if (checkOperator.mulr(mycase)) {
                result++;
                if (operatorDictionary[opCode].find(_o => _o == OperatorType.MULR) == undefined) {
                    operatorDictionary[opCode].push(OperatorType.MULR);
                }
            }
            if (checkOperator.muli(mycase)) {
                result++;
                if (operatorDictionary[opCode].find(_o => _o == OperatorType.MULI) == undefined) {
                    operatorDictionary[opCode].push(OperatorType.MULI);
                }
            }

            if (checkOperator.banr(mycase)) {
                result++;
                if (operatorDictionary[opCode].find(_o => _o == OperatorType.BANR) == undefined) {
                    operatorDictionary[opCode].push(OperatorType.BANR);
                }
            }
            if (checkOperator.bani(mycase)) {
                result++;
                if (operatorDictionary[opCode].find(_o => _o == OperatorType.BANI) == undefined) {
                    operatorDictionary[opCode].push(OperatorType.BANI);
                }
            }

            if (checkOperator.borr(mycase)) {
                result++;
                if (operatorDictionary[opCode].find(_o => _o == OperatorType.BORR) == undefined) {
                    operatorDictionary[opCode].push(OperatorType.BORR);
                }
            }
            if (checkOperator.bori(mycase)) {
                result++;
                if (operatorDictionary[opCode].find(_o => _o == OperatorType.BORI) == undefined) {
                    operatorDictionary[opCode].push(OperatorType.BORI);
                }
            }

            if (checkOperator.setr(mycase)) {
                result++;
                if (operatorDictionary[opCode].find(_o => _o == OperatorType.SETR) == undefined) {
                    operatorDictionary[opCode].push(OperatorType.SETR);
                }
            }
            if (checkOperator.seti(mycase)) {
                result++;
                if (operatorDictionary[opCode].find(_o => _o == OperatorType.SETI) == undefined) {
                    operatorDictionary[opCode].push(OperatorType.SETI);
                }
            }

            if (checkOperator.gtir(mycase)) {
                result++;
                if (operatorDictionary[opCode].find(_o => _o == OperatorType.GTIR) == undefined) {
                    operatorDictionary[opCode].push(OperatorType.GTIR);
                }
            }
            if (checkOperator.gtri(mycase)) {
                result++;
                if (operatorDictionary[opCode].find(_o => _o == OperatorType.GTRI) == undefined) {
                    operatorDictionary[opCode].push(OperatorType.GTRI);
                }
            }
            if (checkOperator.gtrr(mycase)) {
                result++;
                if (operatorDictionary[opCode].find(_o => _o == OperatorType.GTRR) == undefined) {
                    operatorDictionary[opCode].push(OperatorType.GTRR);
                }
            }

            if (checkOperator.eqir(mycase)) {
                result++;
                if (operatorDictionary[opCode].find(_o => _o == OperatorType.EQIR) == undefined) {
                    operatorDictionary[opCode].push(OperatorType.EQIR);
                }
            }
            if (checkOperator.eqri(mycase)) {
                result++;
                if (operatorDictionary[opCode].find(_o => _o == OperatorType.EQRI) == undefined) {
                    operatorDictionary[opCode].push(OperatorType.EQRI);
                }
            }
            if (checkOperator.eqrr(mycase)) {
                result++;
                if (operatorDictionary[opCode].find(_o => _o == OperatorType.EQRR) == undefined) {
                    operatorDictionary[opCode].push(OperatorType.EQRR);
                }
            }

            if (result >= 3) {
                numberOfCases++;
            }
        }

        return numberOfCases;
    }

    operatorsToResolve(operatorDictionary) {
        return operatorDictionary.filter(_o => _o.length > 0).length > 0;
    }

    resolveOpCodes(operatorDictionary, operatorsSolution) {
        let tmpOperator = operatorDictionary.find(_o => _o.length == 1);
        let initialOperator = -1;
        let index = -1;
        if (tmpOperator != undefined) {
            index = operatorDictionary.indexOf(tmpOperator);
            initialOperator = tmpOperator[0];
            tmpOperator.pop();
            operatorsSolution[index] = initialOperator;
        }
        while (this.operatorsToResolve(operatorDictionary)) {
            // find next 
            for (let operator of operatorDictionary) {
                index = operator.indexOf(initialOperator);
                if (index != -1) {
                    operator.splice(index, 1);
                }
            }
            tmpOperator = operatorDictionary.find(_o => _o.length == 1);
            if (tmpOperator != undefined) {
                index = operatorDictionary.indexOf(tmpOperator);
                initialOperator = tmpOperator[0];
                tmpOperator.pop();
                operatorsSolution[index] = initialOperator;
            }
        }
    }

    execute() {
        const fs = require('fs');
        const path = require('path');


        // let filepath = path.join(__dirname, "../test_input.txt");
        let filepath = path.join(__dirname, "../day16_input.txt");
        let lines = fs.readFileSync(filepath, "utf-8").split("\r\n");

        let checkCasesCollection: Array<CheckCase> = [];
        let operationsCollection: Array<Array<number>> = [];

        let operatorDictionary: Array<Array<OperatorType>> = []

        for (let idx = 0; idx < 16; idx++) {
            operatorDictionary.push([]);
        }

        let checkOperator: Operator = new Operator();

        let operatorsSolution: Array<OperatorType> = [];
        let startExecution = false;
        let increment = 4;
        for (var ldx = 0; ldx < lines.length; ldx += increment) {
            let tmpline = lines[ldx];
            if (!startExecution) {
                if (tmpline != "") {
                    let before = [];
                    let input = [];
                    let after = [];
                    let regExp = new RegExp("-?\\d+", "g");
                    for (var counter = 0; counter < 3; counter++) {
                        tmpline = lines[ldx + counter];
                        // var foundValues = tmpline.match(regExp);
                        let numberParts = tmpline.match(regExp);
                        if ((ldx + counter) % 4 == 0) {
                            before.push(parseInt(numberParts[0]));
                            before.push(parseInt(numberParts[1]));
                            before.push(parseInt(numberParts[2]));
                            before.push(parseInt(numberParts[3]));
                        } else if ((ldx + counter) % 4 == 1) {
                            input.push(parseInt(numberParts[0]));
                            input.push(parseInt(numberParts[1]));
                            input.push(parseInt(numberParts[2]));
                            input.push(parseInt(numberParts[3]));
                        } else if ((ldx + counter) % 4 == 2) {
                            after.push(parseInt(numberParts[0]));
                            after.push(parseInt(numberParts[1]));
                            after.push(parseInt(numberParts[2]));
                            after.push(parseInt(numberParts[3]));
                        }
                    }
                    checkCasesCollection.push(new CheckCase(before, input, after));
                } else {
                    startExecution = true;
                    increment = 1;
                }
            } else {
                if (tmpline != "") {
                    let operation = []
                    let regExp = new RegExp("-?\\d+", "g");
                    let numberParts = tmpline.match(regExp);
                    operation.push(parseInt(numberParts[0]));
                    operation.push(parseInt(numberParts[1]));
                    operation.push(parseInt(numberParts[2]));
                    operation.push(parseInt(numberParts[3]));

                    operationsCollection.push(operation);
                }
            }
        }

        let resultPart1 = this.calculateCase(checkCasesCollection, checkOperator, operatorDictionary);
        this.resolveOpCodes(operatorDictionary, operatorsSolution);

        let before: Array<number> = [];
        let after: Array<number> = [];
        let result: Array<number> = [0, 0, 0, 0];
        for (let idx = 0; idx < operationsCollection.length; idx++) {
            let operation = operationsCollection[idx];
            let opCode = operation[0];
            let tmpOperationType = operatorsSolution[opCode];

            switch (tmpOperationType) {
                case OperatorType.ADDR:
                    checkOperator.executeADDR(operation, result);
                    break;
                case OperatorType.ADDI:
                    checkOperator.executeADDI(operation, result);
                    break;
                case OperatorType.MULR:
                    checkOperator.executeMULR(operation, result);
                    break;
                case OperatorType.MULI:
                    checkOperator.executeMULI(operation, result);
                    break;
                case OperatorType.BANR:
                    checkOperator.executeBANR(operation, result);
                    break;
                case OperatorType.BANI:
                    checkOperator.executeBANI(operation, result);
                    break;
                case OperatorType.BORR:
                    checkOperator.executeBORR(operation, result);
                    break;
                case OperatorType.BORI:
                    checkOperator.executeBORI(operation, result);
                    break;
                case OperatorType.SETR:
                    checkOperator.executeSETR(operation, result);
                    break;
                case OperatorType.SETI:
                    checkOperator.executeSETI(operation, result);
                    break;
                case OperatorType.GTIR:
                    checkOperator.executeGTIR(operation, result);
                    break;
                case OperatorType.GTRI:
                    checkOperator.executeGTRI(operation, result);
                    break;
                case OperatorType.GTRR:
                    checkOperator.executeGTRR(operation, result);
                    break;
                case OperatorType.EQIR:
                    checkOperator.executeEQIR(operation, result);
                    break;
                case OperatorType.EQRI:
                    checkOperator.executeEQRI(operation, result);
                    break;
                case OperatorType.EQRR:
                    checkOperator.executeEQRR(operation, result);
                    break;
            }
        }

        //console.log(`Part 1: ${resultPart1}`);
        //console.log(`Part 2: ${result[0]}`);
        return result[0];
    }
}