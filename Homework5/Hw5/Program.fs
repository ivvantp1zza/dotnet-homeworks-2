open System
open Hw4
open Hw5

[<EntryPoint>]
let main (args: string[]) =
    let parsed = Parser.parseCalcArguments args
    match parsed with
    | Ok parsed ->
        let (val1, operation, val2) = parsed
        printf $"{Calculator.calculate val1 operation val2}"
    | Error m -> printf $"{Calculator.messageText m}"
    0
