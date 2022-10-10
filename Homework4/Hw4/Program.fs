open Hw4

[<EntryPoint>]
let main (args: string[]) =
    let parsed = Parser.parseCalcArguments args
    printf $"{Calculator.calculate parsed.arg1 parsed.operation parsed.arg2}"
    0