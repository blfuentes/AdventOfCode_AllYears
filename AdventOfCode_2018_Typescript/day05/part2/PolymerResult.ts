export class PolymerResult {
    character: string;
    result: string;
    value: number;

    constructor (input: string, unit: string) {
        this.result = input;
        this.character = unit;
    }

    reactPolymer() {
        var validString = false;
        var regexp = RegExp(this.character, "gi");
        this.result = this.result.replace(regexp, "");
        while (!validString) {
            validString = true;
            for (var idx = 1; idx < this.result.length; idx++) {
                let currentCharacter: string = this.result[idx];
                let currentCharacterCharCode:number = currentCharacter.charCodeAt(0);
                let prevCharacter:string = this.result[idx - 1];
                let prevCharacterCharCode:number = prevCharacter.charCodeAt(0);
                if(Math.abs(currentCharacterCharCode - prevCharacterCharCode) == 32) {
                    var searchParam = prevCharacter + currentCharacter;
                    var regexp = RegExp(searchParam, "g");
                    this.result = this.result.replace(regexp, "");
                    // this.result = this.result.replace(searchParam, "");
                    validString = false;
                }
            }
        }
        this.value = this.result.length;
    }
}