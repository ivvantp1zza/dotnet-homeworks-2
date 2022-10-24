module Hw6.Parser

open System
open Microsoft.AspNetCore.Http
open Microsoft.FSharp.Core

type CalculatorOperation =
     | Plus = 0
     | Minus = 1
     | Multiply = 2
     | Divide = 3
     
let isArgLengthSupported (request : HttpRequest): Result<'a,'b> =
    let query = request.Query
    match request.Query.Count with
    | 3 -> Ok request
    | _ -> Error (Message.WrongArgLength, "Wrong argument length")
    
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isOperationSupported (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), (Message * string)> =
    match operation with
    | "Plus" -> Ok (arg1, CalculatorOperation.Plus, arg2)
    | "Minus" -> Ok (arg1, CalculatorOperation.Minus, arg2)
    | "Multiply" -> Ok (arg1, CalculatorOperation.Multiply, arg2)
    | "Divide" -> Ok (arg1, CalculatorOperation.Divide, arg2)
    | _ -> Error (Message.WrongArgFormatOperation, $"Could not parse value '{operation}'")

let isParametersCorrect (request: HttpRequest): Result<('a * 'b * 'c), (Message * string)> =
    let query = request.Query
    let parsedArg1 = query.TryGetValue("value1")
    match parsedArg1 with
    | false, v1 -> Error (Message.WrongArgFormat, "Wrong argument name")
    | true, v1 ->
        match query.TryGetValue("value2") with
        | false, v2 -> Error (Message.WrongArgFormat, "Wrong argument name")
        | true, v2 ->
            match query.TryGetValue("operation") with
            | false, op -> Error (Message.WrongArgFormat, "Wrong argument name")
            | true, op -> Ok (v1.ToString(), op.ToString(), v2.ToString())
            
let parseArgs (arg1: string, operation: string, arg2: string): Result<('a * CalculatorOperation * 'b), (Message * string)> =
    match Decimal.TryParse(arg1.Replace(".", ",")) with
    | false, v1 -> Error (Message.WrongArgFormat, $"Could not parse value '{arg1}'")
    | true, v1 ->
        match Decimal.TryParse(arg2.Replace(".", ",")) with
        | false, v2 -> Error (Message.WrongArgFormat, $"Could not parse value '{arg2}'")
        | true, v2 -> isOperationSupported (v1, operation, v2)
    
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isDividingByZero (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), (Message * string)> =
    match operation with
    | CalculatorOperation.Divide ->
        match arg2 with
        | 0m -> Error (Message.DivideByZero, "DivideByZero")
        | _ -> Ok (arg1, operation, arg2)
    | _ -> Ok (arg1, operation, arg2)
    
let parseCalcArguments (request: HttpRequest): Result<'a, 'b> =
    let maybe = new MaybeBuilder()
    maybe {
        let! correctLength = request |> isArgLengthSupported
        let! correctArgs = correctLength |> isParametersCorrect
        let! parsed = correctArgs |> parseArgs
        let! divByZero = parsed |> isDividingByZero
        return divByZero
    }