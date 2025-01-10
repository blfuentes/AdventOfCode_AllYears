module day18_part02

open AdventOfCode_2020.Modules
open FParsec

// parsers
let parseDecimal : Parser<decimal, unit> = puint64 |>> decimal
let strWs s = pstring s >>. spaces
let parseTerm expression = (parseDecimal .>> spaces) <|> between (strWs "(") (strWs ")") expression

let runParser expr str =
  match run expr str with
    | Success (result, _, _) -> result
    | Failure (errorMsg, _, _) -> failwithf "Error from parser: %s" errorMsg

let runPart addOperator input =
  let opp = OperatorPrecedenceParser<decimal, unit, unit>()
  let expression = opp.ExpressionParser
  
  let multOperator = InfixOperator ("*", spaces, 1, Associativity.Left, (*))
  
  opp.TermParser <- parseTerm expression
  opp.AddOperator(addOperator)
  opp.AddOperator(multOperator)
  
  input |> Array.sumBy (runParser expression) 

let execute =
    let path = "day18/day18_input.txt"
    let operations = LocalHelper.GetLinesFromFile path
    runPart (InfixOperator ("+", spaces, 2, Associativity.Left, (+))) operations