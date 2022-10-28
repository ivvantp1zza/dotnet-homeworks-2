module Hw6.Calculator

open System
open System.Net
open Hw6.Parser
open Microsoft.AspNetCore.Http

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
    | Message.SuccessfulExecution-> "Executed successfully"
    | Message.WrongArgLength -> "Wrong argument length"
    | Message.WrongArgFormat -> "One or both arguments is incorrect"
    | Message.WrongArgFormatOperation -> "Operation is not supported"
    | Message.DivideByZero -> "Cannot divide by zero"
 
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]   
let statusCode (message: Message) =
    match message with
    | Message.SuccessfulExecution -> HttpStatusCode.OK
    | Message.DivideByZero -> HttpStatusCode.OK
    | _ -> HttpStatusCode.BadRequest
 
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]   
let ToString (obj): string =
        obj.ToString()

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline calculate (request: HttpRequest): 'a =
    let parsed = parseCalcArguments request
    match parsed with
    | Error (m, str) ->
        match m with
        | Message.DivideByZero -> Ok str
        | _ -> Error str
    | Ok args ->
        let (value1, operation, value2) = args
        match operation with
        | CalculatorOperation.Plus -> Ok (ToString (value1 + value2))
        | CalculatorOperation.Minus -> Ok (ToString (value1 - value2))
        | CalculatorOperation.Multiply -> Ok (ToString (value1 * value2))
        | CalculatorOperation.Divide -> Ok (ToString (value1 / value2))
        
        
    