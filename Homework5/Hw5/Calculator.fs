module Hw5.Calculator

open System
open Hw4

type CalculatorOperation =
     | Plus = 0
     | Minus = 1
     | Multiply = 2
     | Divide = 3

[<Literal>] 
let plus = "+"

[<Literal>] 
let minus = "-"

[<Literal>] 
let multiply = "*"

[<Literal>] 
let divide = "/"

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let messageText error =
    match error with
    | Message.SuccessfulExecution -> "Executed successfully"
    | Message.WrongArgLength -> "Wrong argument length"
    | Message.WrongArgFormat -> "One or both arguments is incorrect"
    | Message.WrongArgFormatOperation -> "Operation is not supported"
    | Message.DivideByZero -> "Cannot divide by zero"

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline calculate value1 operation value2: 'a =
    match operation with
    | CalculatorOperation.Plus -> value1 + value2
    | CalculatorOperation.Minus -> value1 - value2
    | CalculatorOperation.Multiply -> value1 * value2
    | CalculatorOperation.Divide -> value1 / value2
