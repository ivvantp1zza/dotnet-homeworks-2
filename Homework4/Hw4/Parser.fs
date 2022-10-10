module Hw4.Parser

open System
open Hw4.Calculator


type CalcOptions = {
    arg1: float
    arg2: float
    operation: CalculatorOperation
}

let isArgLengthSupported (args : string[]) =
    args.Length = 3

let parseOperation (arg : string) =
    match arg with
    | "+" -> Calculator.CalculatorOperation.Plus
    | "-" -> Calculator.CalculatorOperation.Minus
    | "*" -> Calculator.CalculatorOperation.Multiply
    | "/" -> Calculator.CalculatorOperation.Divide
    | _ -> Calculator.CalculatorOperation.Undefined
      
let parseCalcArguments(args : string[]) =
    match isArgLengthSupported args with
    | false -> ArgumentException() |> raise
    | true ->
        match parseOperation args[1] with
        | CalculatorOperation.Undefined -> ArgumentException() |> raise
        | _ ->
            match Double.TryParse args[0], Double.TryParse args[2] with
            | (true, res1), (true, res2) -> {arg1 = Double.Parse args[0]
                                             arg2 = Double.Parse args[2]
                                             operation = parseOperation args[1]}
            | _ -> ArgumentException() |> raise
            
            
            
            
            
            
            
    
    
