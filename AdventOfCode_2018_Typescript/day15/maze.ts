let numberOfRows = 3;
let numberOfColumns = 3;

let elementId = 0;

let adjMatrix = new Array<Array<number>>();
adjMatrix = Array(numberOfRows * numberOfColumns).fill(null).map(item => (new Array(numberOfRows * numberOfColumns).fill(0)));
let gridDefinition = new Array<Array<number>>();
gridDefinition = Array(numberOfRows).fill(null).map(item => (new Array(numberOfRows).fill(0)));

let visitedMatrix = new Array<Array<boolean>>();
visitedMatrix = Array(numberOfRows * numberOfColumns).fill(null).map(item => (new Array(numberOfRows * numberOfColumns).fill(false)));

// grid
for (let idx = 0; idx < numberOfRows; idx++) {
  for (let jdx = 0; jdx < numberOfColumns; jdx++) {
    gridDefinition[idx][jdx] = elementId++;
  }
}

// adjacency matrix
for (let idx = 0; idx <numberOfRows; idx++) {
  for (let jdx = 0; jdx < numberOfColumns; jdx++) {
    let aux = idx*numberOfColumns + jdx;
    if (isFree(idx, jdx)) {
      if (jdx > 0) {
        if (isFree(aux - 1, aux)) {
          adjMatrix[aux - 1][aux] = 1;
        }
        if (isFree(aux, aux - 1)) {
          adjMatrix[aux][aux - 1] = 1;
        }
      }
      if (idx > 0) {
        if (isFree(aux - numberOfColumns, aux)) {
          adjMatrix[aux - numberOfColumns][aux] = 1;
        }
        if (isFree(aux, aux - numberOfColumns)) {
          adjMatrix[aux][aux - numberOfColumns] = 1;
        }
      }
    }
  }
}

function isFree(row: number, col: number) {
  return (row >= 0 && col >= 0 && !visitedMatrix[row][col]);
}

//
let rowQueue = new Array<number>();
let colQueue = new Array<number>();
let startRow = 0;
let startCol = 0;
let endRow = 2;
let endCol = 1;
let moveCount = 0;
let nodesLeftInLayer = 1;
let nodesInNextLayer = 0;

let reachedEnd = false;

let rows = [-1, 1, 0, 0];
let columns = [0, 0, 1, -1];

let currentRow = 0;
let currentCol = 0;



class Point {
  row: number;
  col: number;
  parent: Point;

  constructor (r: number, c: number, p: Point) {
    this.row = r;
    this.col = c;
    this.parent = p;
  }
}
let resultPath = new Array<Point>();

function solve() {
  rowQueue.push(startRow);
  colQueue.push(startCol);
  visitedMatrix[startRow][startCol] = true;

  while (rowQueue.length > 0) {
    currentRow = rowQueue.shift();
    currentCol = colQueue.shift();

    if (currentRow == endRow && currentCol == endCol) {
      reachedEnd = true;
      break;
    }
    exploreNeighbours(currentRow, currentCol);
    nodesLeftInLayer--;
    if (nodesLeftInLayer == 0) {
      nodesLeftInLayer = nodesInNextLayer;
      nodesInNextLayer = 0;
      moveCount++;
    }
  }
  if (reachedEnd) {
    return moveCount;
  }
  return null;
}

function exploreNeighbours(row: number, col: number) {
  for (let idx = 0; idx < 4; idx ++) {
    let rr = row + rows[idx];
    let cc = col + columns[idx];

    if (isFree(rr, cc)) {
      rowQueue.push(rr);
      colQueue.push(cc);
      visitedMatrix[rr][cc] = true;
      nodesInNextLayer++;
    }
  }
}

function solveWithPath(startRow: number, startCol: number) {
  resultPath.push(new Point(startRow, startCol, null));
  
}

//
let result = solve();

console.log(`finieshed in ${result} turns!`);