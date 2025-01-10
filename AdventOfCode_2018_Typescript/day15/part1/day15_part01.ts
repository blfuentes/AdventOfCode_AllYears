import fs = require('fs');
import path = require('path');

import { Coord } from "../Coord";
import { ElementType, Player, Elf, Goblin, Field, Wall } from "../Element";
import { MazePoint } from "../MazeResult";

// 
function sortPlayerByPosition(a: Player, b: Player) {
    if (a.position.coordY == b.position.coordY) return a.position.coordX - b.position.coordX;
    return a.position.coordY - b.position.coordY;
}
function sortByPosition(a: Coord, b: Coord) {
    if (a.coordY == b.coordY) return a.coordX - b.coordX;
    return a.coordY - b.coordY;
}

function getElementType(character: string) {
    if (character == "#") {
        return ElementType.WALL;
    } else if (character == "E") {
        return ElementType.ELF;
    } else if (character == "G") {
        return ElementType.GOBLIN;
    } else {
        return ElementType.FIELD;
    }
}

export class Day15Part01 {
    // let filepath = path.join(__dirname, "../test_input.txt");
    // let filepath = path.join(__dirname, "../movement_input.txt");
    // let filepath = path.join(__dirname, "../basic_sample.txt");
    // let filepath = path.join(__dirname, "../combat_sample.txt");
    // let filepath = path.join(__dirname, "../combat_sample05.txt");
    Filepath = path.join(__dirname, "../day15_input.txt");
    Lines = fs.readFileSync(this.Filepath, "utf-8").split("\r\n");
    // let lines = fs.readFileSync(filepath, "utf-8").split("\n");

    PlayerCollection: Array<Player> = [];
    ElfCollection: Array<Elf> = [];
    GoblinCollection: Array<Goblin> = [];
    MapPositions: Array<Coord> = [];
    CaveMap: Array<Array<string>> = [];

    AreEnemiesLeft(player: Player) {
        if (player.elementType == ElementType.ELF) {
            return this.PlayerCollection.find(_p => _p.elementType == ElementType.GOBLIN && _p.isAlive) != undefined;
        } else if (player.elementType == ElementType.GOBLIN) {
            return this.PlayerCollection.find(_p => _p.elementType == ElementType.ELF && _p.isAlive) != undefined;
        } else {
            return false;
        }
    }

    DisplayCaveMap(caveMap: Array<Array<string>>) {
        let rowIdx = 0;
        while (rowIdx < this.Lines.length) {
            let rowDisplay = "";
            for (let colIdx = 0; colIdx < caveMap.length; colIdx++) {
                rowDisplay += caveMap[colIdx][rowIdx];
            }
            console.log(`${rowDisplay}`);
            rowIdx++;
        }
    }

    InitGame() {
        // READ MAP
        let rowIdx = 0;
        for (let line of this.Lines) {
            let lineParts = line.split('');
            for (let column = 0; column < lineParts.length; column++) {
                let tmpmapelement = lineParts[column];
                let tmpelementtype = getElementType(tmpmapelement);
                let newCoord = new Coord(column, rowIdx);

                if (tmpelementtype == ElementType.FIELD) {
                    let newElement = new Field(newCoord);
                    newCoord.element = newElement;
                    newCoord.isFree = true;
                } else if (tmpelementtype == ElementType.WALL) {
                    let newElement = new Wall(newCoord);
                    newCoord.element = newElement;
                    newCoord.isFree = false;
                } else if (tmpelementtype == ElementType.ELF) {
                    let newElement = new Elf(newCoord);
                    newElement.isAlive = true;
                    newCoord.element = newElement;
                    newCoord.isFree = false;

                    //
                    this.PlayerCollection.push(newElement);
                    this.ElfCollection.push(newElement);
                } else if (tmpelementtype == ElementType.GOBLIN) {
                    let newElement = new Goblin(newCoord);
                    newElement.isAlive = true;
                    newCoord.element = newElement;
                    newCoord.isFree = false;

                    //
                    this.PlayerCollection.push(newElement);
                    this.GoblinCollection.push(newElement);
                }
                this.CaveMap[column][rowIdx] = lineParts[column];
                this.MapPositions.push(newCoord);
            }
            rowIdx++;
        }

        // sort units
        this.PlayerCollection = this.PlayerCollection.sort(sortPlayerByPosition);
    }

    TakeNewPlace(player: Player) {
        if (player.NextPosition != undefined) {
            let newPosition = this.MapPositions.find(_p => _p.isEqual(player.NextPosition));
            let oldPosition = this.MapPositions.find(_p => _p.isEqual(player.position));
            if (oldPosition != undefined && newPosition != undefined) {
                oldPosition.isFree = true;
                oldPosition.element = new Field(new Coord(oldPosition.coordX, oldPosition.coordY));
                this.CaveMap[oldPosition.coordX][oldPosition.coordY] = oldPosition.element.display();

                newPosition.isFree = false;
                newPosition.element = player;
                player.position = newPosition;
                this.CaveMap[newPosition.coordX][newPosition.coordY] = newPosition.element.display();
            }
        }
    }

    FindBFSPath(origin: Coord, target: Coord) {
        let resultPath = new Array<MazePoint>();
        let visistedMazePoints = new Array<Coord>();
        let tmpMazePoint = new MazePoint(origin, null);
        resultPath.push(tmpMazePoint);
        while (resultPath.length > 0) {
            let currentPoint = resultPath.shift();
            if (currentPoint != undefined &&
                currentPoint.position.isEqual(target)) {
                return currentPoint;
            }
            if (currentPoint != undefined &&
                visistedMazePoints.find(_v => _v.isEqual(currentPoint.position)) == undefined) {
                let neighbourMazePoint: MazePoint;
                let xCord: Array<number>;
                let yCord: Array<number>;
                xCord = [0, -1, 1, 0];
                yCord = [-1, 0, 0, 1];
                for (let idx = 0; idx < 4; idx++) {
                    neighbourMazePoint = new MazePoint(new Coord(currentPoint.position.coordX + xCord[idx], currentPoint.position.coordY + yCord[idx]), currentPoint);
                    if (this.IsValid(visistedMazePoints, neighbourMazePoint.position)) {
                        if (visistedMazePoints.find(_v => _v.isEqual(currentPoint.position)) == undefined) {
                            visistedMazePoints.push(currentPoint.position);
                        }
                        resultPath.push(neighbourMazePoint);
                    }
                }
            }
        }

        return null;
    }

    IsValid(visited: Array<Coord>, position: Coord) {
        let checkPosition = this.MapPositions.find(_p => _p.isEqual(position));
        if ((position.coordY >= 0) &&
            (position.coordY < this.Lines.length) &&
            (position.coordX >= 0) &&
            (position.coordX < this.Lines[0].length) &&
            (checkPosition != undefined && checkPosition.element.elementType == ElementType.FIELD)) {
            for (var j = 0; j < visited.length; j++) {
                if (visited[j].isEqual(position)) {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    Round(player: Player) {
        let target = this.GetTarget(player);
        if (target != undefined) {
            this.Attack(player, target);
        } else {
            this.Move(player);
            this.TakeNewPlace(player);
            target = this.GetTarget(player);
            if (target != undefined) {
                this.Attack(player, target);
            }
        }
    }

    Attack(attacker: Player, target: Player) {
        if (target.HP > attacker.AP) {
            target.HP -= attacker.AP;
        } else {
            target.HP = 0;
            target.isAlive = false;
        }
        //console.log(`${target.display()} [${target.position.coordX}, ${target.position.coordY}] attacked by ` +
        //    `${attacker.display()} [${attacker.position.coordX}, ${attacker.position.coordY}]. ` +
        //    `Damaged with ${attacker.AP} points. Current health: ${target.HP}.`)
        if (target.HP == 0) {
            let targetPosition = this.MapPositions.find(_p => _p.isEqual(target.position));
            if (targetPosition != undefined) {
                target.elementType = ElementType.FIELD;
                targetPosition.element = new Field(target.position);
                this.CaveMap[targetPosition.coordX][targetPosition.coordY] = target.display();
            }
        }
    }

    GetTarget(player: Player) {
        let target: Player | undefined;
        let rangePositions = [
            [player.position.coordX, player.position.coordY - 1],
            [player.position.coordX - 1, player.position.coordY],
            [player.position.coordX + 1, player.position.coordY],
            [player.position.coordX, player.position.coordY + 1]
        ];
        let enemyType = ElementType.FIELD;
        if (player.elementType == ElementType.GOBLIN) {
            enemyType = ElementType.ELF;
        } else {
            enemyType = ElementType.GOBLIN;
        }
        let tmpCoord: Coord | undefined;
        let lowestHP = -1;
        for (let idx = 0; idx < rangePositions.length; idx++) {
            let pos = rangePositions[idx];
            tmpCoord = this.MapPositions.find(_m => _m.coordX == pos[0] && _m.coordY == pos[1] && _m.element.elementType == enemyType);
            if (tmpCoord != undefined) {
                if (lowestHP < 0) {
                    target = tmpCoord.element as Player;
                    lowestHP = target.HP;
                } else if ((tmpCoord.element as Player).HP < lowestHP) {
                    target = tmpCoord.element as Player;
                    lowestHP = target.HP;
                }
            }
        }
        return target;

    }

    FindAvailableEnemyPositions(player: Player) {
        let enemies: Array<Player> = [];
        let enemyPositions: Array<Coord> = [];
        if (player.elementType == ElementType.ELF) {
            enemies = this.PlayerCollection.filter(_p => _p.elementType == ElementType.GOBLIN && _p.isAlive);
        } else if (player.elementType == ElementType.GOBLIN) {
            enemies = this.PlayerCollection.filter(_p => _p.elementType == ElementType.ELF && _p.isAlive);
        }
        for (let enemy of enemies) {
            // up
            let upCoord = this.MapPositions.find(_m => _m.coordX == enemy.position.coordX && _m.coordY == (enemy.position.coordY - 1));
            if (upCoord != undefined && upCoord.element.elementType == ElementType.FIELD) {
                enemyPositions.push(upCoord);
            }
            // down
            let downCoord = this.MapPositions.find(_m => _m.coordX == enemy.position.coordX && _m.coordY == (enemy.position.coordY + 1));
            if (downCoord != undefined && downCoord.element.elementType == ElementType.FIELD) {
                enemyPositions.push(downCoord);
            }
            // left
            let leftCoord = this.MapPositions.find(_m => _m.coordX == (enemy.position.coordX - 1) && _m.coordY == enemy.position.coordY);
            if (leftCoord != undefined && leftCoord.element.elementType == ElementType.FIELD) {
                enemyPositions.push(leftCoord);
            }
            // right
            let rightCoord = this.MapPositions.find(_m => _m.coordX == (enemy.position.coordX + 1) && _m.coordY == enemy.position.coordY);
            if (rightCoord != undefined && rightCoord.element.elementType == ElementType.FIELD) {
                enemyPositions.push(rightCoord);
            }
        }
        return enemyPositions;
    }

    Move(player: Player) {
        let coordToMove: Coord = new Coord(0, 0);
        let minDistance = 0;
        let doMove = false;
        player.EnemiesPositions = player.EnemiesPositions.sort(sortByPosition);
        for (let idx = 0; idx < player.EnemiesPositions.length; idx++) {
            let positionToCheck = player.EnemiesPositions[idx];
            let foundPath = this.FindBFSPath(player.position, positionToCheck);
            let tmpDistance = 0;
            if (foundPath) {
                tmpDistance = this.GetLengthMazePointResult(foundPath);
                let tmpNextCoord = this.GetNextStepMazePointResult(foundPath);
                if (idx == 0 || !doMove) {
                    doMove = true;
                    minDistance = tmpDistance;
                    if (tmpNextCoord != undefined) {
                        coordToMove = tmpNextCoord;
                    }
                } else if (tmpDistance < minDistance) {
                    minDistance = tmpDistance;
                    if (tmpNextCoord != undefined) {
                        coordToMove = tmpNextCoord;
                    }
                }
            }
        }

        if (doMove && coordToMove != undefined) {
            let newPosition = this.MapPositions.find(_p => _p.isEqual(coordToMove));
            if (newPosition != undefined) {
                player.NextPosition = newPosition;
            }
        }

    }

    GetLengthMazePointResult(result: MazePoint) {
        let distance = 0;
        while (result.parent != null) {
            distance++;
            result = result.parent;
        }
        return distance;
    }

    GetNextStepMazePointResult(result: MazePoint) {
        let invertedResult = Array<Coord>();
        invertedResult.push(result.position);
        while (result.parent != null) {
            invertedResult.push(result.position);
            result = result.parent;
        }
        invertedResult = invertedResult.reverse();
        return invertedResult.shift();
    }

    Execute() {
        // INIT CAVEMAP
        this.CaveMap = Array(this.Lines[0].length).fill(null).map(item => (new Array(this.Lines.length).fill(" ")));

        this.InitGame();
        //this.DisplayCaveMap(this.CaveMap);

        let roundNumber = 0;
        do {
            roundNumber++;
            //console.log(`Start of round ${roundNumber}.`);
            for (let player of this.PlayerCollection) {
                if (player.isAlive && this.AreEnemiesLeft(player)) {
                    player.EnemiesPositions = this.FindAvailableEnemyPositions(player).sort(sortByPosition);
                    this.Round(player);
                }
            }
            //this.DisplayCaveMap(this.CaveMap);
            //console.log(`End of round ${roundNumber}.`);
            this.PlayerCollection = this.PlayerCollection.filter(p => p.isAlive).sort(sortPlayerByPosition);

        } while (this.ElfCollection.find(e => e.isAlive) && this.GoblinCollection.find(g => g.isAlive))

        //
        let health = this.PlayerCollection.map(p => p.HP)
            .reduce((prev, curr) => prev + curr, 0)
        let score = health * (--roundNumber);

        //
        //console.log(`finished! Score: ${score} (health: ${health})`);
        return score;
    }
}
