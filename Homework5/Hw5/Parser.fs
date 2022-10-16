module Hw5.Parser

open System
open System.IO
open Hw5.Calculator
open Hw5.MaybeBuilder

let isArgLengthSupported (args:string[]): Result<'a,'b> =
    match args.Length with
    | 3 -> Ok args
    | _ -> Error Message.WrongArgLength
    
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isOperationSupported (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    match operation with
    | "+" -> Ok (arg1, CalculatorOperation.Plus, arg2)
    | "-" -> Ok (arg1, CalculatorOperation.Minus, arg2)
    | "*" -> Ok (arg1, CalculatorOperation.Multiply, arg2)
    | "/" -> Ok (arg1, CalculatorOperation.Divide, arg2)
    | _ -> Error Message.WrongArgFormatOperation

let parseArgs (args: string[]): Result<('a * CalculatorOperation * 'b), Message> =
    let parsedArg1 = Double.TryParse(args[0])
    match parsedArg1 with
    | false, v1 -> Error Message.WrongArgFormat
    | true, v1 ->
        match Double.TryParse(args[2]) with
        | false, v2 -> Error Message.WrongArgFormat
        | true, v2 -> isOperationSupported (v1, args[1], v2)
    
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isDividingByZero (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    match operation with
    | CalculatorOperation.Divide ->
        match arg2 with
        | 0.0 -> Error Message.DivideByZero
        | _ -> Ok (arg1, operation, arg2)
    | _ -> Ok (arg1, operation, arg2)
    
let parseCalcArguments (args: string[]): Result<'a, 'b> =
    let maybe = new MaybeBuilder()
    maybe {
        let! correctLength = args |> isArgLengthSupported
        let! correctArgs = correctLength |> parseArgs
        let! divByZero = correctArgs |> isDividingByZero
        return divByZero
    }
