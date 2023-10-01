module Hw4.Parser
open Hw4.Calculator

type CalcOptions = {
    arg1: float
    arg2: float
    operation: CalculatorOperation
}

let isArgLengthSupported (args : string[]) = args.Length = 3
    
let parseOperation (arg : string) =
    match arg with
    | "+" -> CalculatorOperation.Plus
    | "-" -> CalculatorOperation.Minus
    | "*" -> CalculatorOperation.Multiply
    | "/" -> CalculatorOperation.Divide
    | _ -> raise (System.InvalidOperationException("Incorrect operation"))
    
let parseArgument (arg: string) =
    match System.Double.TryParse(arg) with
    | (true, num) -> num
    | _ -> raise (System.ArgumentException("It not number!"))
    
let parseCalcArguments (args: string[]) =
    if (not (isArgLengthSupported args)) then
        raise (System.ArgumentException("Incorrect number of arguments"))
    let args = {
        arg1 = parseArgument args.[0]
        arg2 = parseArgument args.[2]
        operation = parseOperation args.[1] 
    }
    args
