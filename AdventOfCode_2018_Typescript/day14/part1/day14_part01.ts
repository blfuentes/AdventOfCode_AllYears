import { parse } from "url";

export class Day14Part01 {
    getNextRecipePosition(seekPosition: number, table: Array<number>, position: number) {
        return (position + seekPosition) % table.length;
    }

    printCookTable(positions: Array<number>, table: Array<number>) {
        let tableState = "";
        let element = "";
        for (let idx = 0; idx < table.length; idx++) {
            if (idx == positions[0]) {
                element = `(${table[idx]})`;
            } else if (idx == positions[1]) {
                element = `[${table[idx]}]`;
            } else {
                element = ` ${table[idx]} `;
            }
            tableState += element;
        }
        console.log(tableState);
    }

    execute() {
        const fs = require('fs');
        let input = parseInt("074501");
        let cookTable: Array<number> = [];

        let currentRecipes: Array<number> = [];
        currentRecipes.push(3);
        currentRecipes.push(7);
        cookTable.push(currentRecipes[0]);
        cookTable.push(currentRecipes[1]);
        let currentRecipePosition: Array<number> = [];
        currentRecipePosition.push(0);
        currentRecipePosition.push(1);


        let counter = 0;
        let newPosition = 0;
        let numberOfNewRecipes = 2;
        let magicFormula: Array<number> = [];
        let magicFormulaStatus = "";
        let secondFormula: Array<number> = [];
        let initIndex = 0;
        //this.printCookTable(currentRecipePosition, cookTable);
        do {
            // initialize the cooktable
            let nextRecipeNumber = currentRecipes[0] + currentRecipes[1];
            let nextRecipeStringfy = nextRecipeNumber.toString().split('');
            for (let recipePart of nextRecipeStringfy) {
                let tmpRecipe = parseInt(recipePart);
                numberOfNewRecipes++;
                if (numberOfNewRecipes > input && counter < 10) {
                    magicFormula.push(tmpRecipe);
                    counter++;
                }
                cookTable.push(tmpRecipe);
            }
            newPosition = this.getNextRecipePosition(currentRecipes[0] + 1, cookTable, currentRecipePosition[0]);
            currentRecipes[0] = cookTable[newPosition]
            currentRecipePosition[0] = newPosition;
            newPosition = this.getNextRecipePosition(currentRecipes[1] + 1, cookTable, currentRecipePosition[1]);
            currentRecipes[1] = cookTable[newPosition]
            currentRecipePosition[1] = newPosition;
            //this.printCookTable(currentRecipePosition, cookTable);
        } while (counter < 10);

        magicFormulaStatus = magicFormula.toString().replace(new RegExp(",", "g"), "");
        return magicFormulaStatus;
    }
}
