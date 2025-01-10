import { Node } from "../Node";

function by<T extends keyof U, U>(property: T): (a: U, b: U) => number {
    return (a, b) => {
        return a[property] < b[property] ? -1 : a[property] > b[property] ? 1 : 0;
    };
}

function findNextElement(input: Array<string>) {
    return input.sort().shift();
}

function buildParents(element: Node, parentsChain: Array<Node>) {
    if (element == undefined) {
        return parentsChain;
    }
    element.parentNodes.forEach(p => {
        parentsChain.push(p);
        buildParents(p, parentsChain);
    })
}

function isBlocked(element: Node, possibleBlockers: Array<Node>) {
    let isBlocked = false;
    let parentsChain: Array<Node> = [];
    buildParents(element, parentsChain);
    isBlocked = parentsChain.filter(p => possibleBlockers.filter(pb => pb != null).find(pb => pb.element == p.element) != undefined).length > 0;
    return isBlocked;
}

function findNextElementForQueue(workingPlan: Array<Node>, workingQueue: Array<Node>) {
    let elementToRemove = null;

    for (let wIdx = 0; wIdx < workingPlan.length; wIdx++) {
        if (!isBlocked(workingPlan[wIdx], workingQueue)) {
            elementToRemove = workingPlan[wIdx];
            workingPlan[wIdx] = null;
            break;
        }
    }    

    return elementToRemove;
}
export class Day07Part02 {
    execute() {
        let fs = require("fs");
        let path = require('path');
        //let filepath = path.join(__dirname, "../test01_input.txt");
        // let filepath = path.join(__dirname, "../test02_input.txt");
        // let filepath = path.join(__dirname, "../test03_input.txt");
        let filepath = path.join(__dirname, "../day07_input.txt");
        let text: string = fs.readFileSync(filepath, "utf-8");
        let lines = text.split("\r\n");

        let nodeTree: Node | undefined;
        let nodeList: Array<Node> = [];
        let initNodeList: Array<Node> = [];

        for (let line of lines) {
            let lineParts = line.split(" ");
            let tmpNode = nodeList.find(_n => _n.element == lineParts[7]);
            if (tmpNode == undefined) {
                tmpNode = new Node(lineParts[7], undefined, 0);
            }
            let tmpParent = nodeList.find(_n => _n.element == lineParts[1]);
            if (tmpParent == undefined) {
                tmpParent = new Node(lineParts[1], undefined, 0);
                nodeTree = tmpParent;
                nodeList.push(tmpParent);
                initNodeList.push(tmpParent);
            }
            tmpNode.parentNodes.push(tmpParent);
            tmpParent.childNodes.push(tmpNode);

            tmpNode.parentNodes = tmpNode.parentNodes.sort((first: Node, second: Node) => first.element.charCodeAt(0) - second.element.charCodeAt(0));
            tmpParent.childNodes = tmpParent.childNodes.sort((first: Node, second: Node) => first.element.charCodeAt(0) - second.element.charCodeAt(0));

            if (nodeList.find(_n => _n.element == lineParts[7]) == undefined) {
                nodeList.push(tmpNode);
                initNodeList.push(tmpNode);
            }
        }
        let firstNode = nodeList.find(_n => _n.parentNodes.length == 0);
        let lastNode = nodeList.find(_n => _n.childNodes.length == 0);

        let parentNodes: Array<Node> = []
        let currentSolution: Array<string> = [];
        let availableElements: Array<string> = [];
        parentNodes = nodeList.filter(_n => _n.parentNodes.length == 0);
        if (parentNodes.length > 0) {
            for (let parent of parentNodes) {
                availableElements.push(parent.element);
            }
            while (availableElements.length > 0) {
                let elementToRemove = findNextElement(availableElements);
                if (elementToRemove != undefined) {
                    currentSolution.push(elementToRemove);
                    let tmpNode = nodeList.find(_n => _n.element == elementToRemove);
                    if (tmpNode != undefined) {
                        var nodeIndex = nodeList.indexOf(tmpNode);
                        if (nodeIndex > -1) {
                            nodeList.splice(nodeIndex, 1);
                            for (let child of tmpNode.childNodes) {
                                if (currentSolution.indexOf(child.element) == -1) {
                                    let existsParent = true;
                                    for (let parent of child.parentNodes) {
                                        if (currentSolution.indexOf(parent.element) == -1) {
                                            existsParent = false;
                                            break;
                                        }
                                    }
                                    if (existsParent && availableElements.indexOf(child.element) == -1) {
                                        availableElements.push(child.element);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        let workOrder: string;
        if (firstNode != undefined && lastNode != undefined) {
            workOrder = currentSolution.toString().replace(RegExp(",", "g"), "");
        }

        let numberOfWorkers = 5;
        let durationOffset = 60;
        let workerQueue: Array<Node> = [];
        let workingPlan: Array<Node> = [];

        for (let idx = 0; idx < numberOfWorkers; idx++) { workerQueue.push(null); }

        currentSolution.forEach(o => {
            let order = initNodeList.find(n => n.element == o);
            if (order != undefined) {
                order.duration += durationOffset;
                workingPlan.push(order);
            }
        });

        let runningSeconds = 0;
        while (workingPlan.length > 0 || workerQueue.find(w => w != null)) {
            //console.log(`Second ${runningSeconds}`);
            let availableWorkers = workerQueue.filter(w => !w).length;
            if (availableWorkers > 0) {
                for (let wIdx = 0; wIdx < workerQueue.length; wIdx++) {
                    if (workerQueue[wIdx] == null) {
                        let elementForQueue = findNextElementForQueue(workingPlan, workerQueue);
                        if (elementForQueue != null) {
                            //console.log(`Worker ${wIdx + 1} is free`);
                            //console.log(`Element ${elementForQueue.element} is being processed`);
                            workerQueue[wIdx] = elementForQueue;
                            workingPlan = workingPlan.filter(w => w != null);
                        }
                    }
                }
            }
            runningSeconds++;
            for (let wIdx = 0; wIdx < workerQueue.length; wIdx++) {
                if (workerQueue[wIdx] != null) {
                    workerQueue[wIdx].duration--;
                    if (workerQueue[wIdx].duration == 0) {
                        //console.log(`Element ${workerQueue[wIdx].element} is finished`);
                        workerQueue[wIdx] = null;
                    }
                }
            }

        }
        return runningSeconds;
    }
}

