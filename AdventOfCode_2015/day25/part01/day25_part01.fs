module day25_part01

open System.Text.RegularExpressions
open AdventOfCode_2015.Modules

let parseContent(content: string) =
    let regexpattern = @"\d+"
    let numbers = Regex.Matches(content, regexpattern)
    let (row, column) = ((int)(numbers.Item(0).Value), (int)(numbers.Item(1).Value))
    (row, column)

let next code = 
    (code * 252533L) % 33554393L

let rec step target (row,col) code = 
        if (row,col) = target then code
        else step target (if row = 1 then (col+1, 1) else (row-1, col+1)) (next code)

let execute =
    let input = "./day25/day25_input.txt"
    let content = LocalHelper.GetContentFromFile input
    let (row, column) = parseContent content
    step(row, column) (1,1) 20151125L