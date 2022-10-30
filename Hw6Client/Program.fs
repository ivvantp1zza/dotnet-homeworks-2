// For more information see https://aka.ms/fsharp-console-apps

open System
open System.Diagnostics.CodeAnalysis
open System.Net.Http
open System.Threading.Tasks
[<ExcludeFromCodeCoverage>]
let client = new HttpClient()

[<ExcludeFromCodeCoverage>]
let getUrl value1 operation value2 =
    $"http://localhost:5000/calculate?value1={value1}&operation={operation}&value2={value2}"

[<ExcludeFromCodeCoverage>]
let matchOperation op =
    match op with
    | "+" -> "Plus"
    | "-" -> "Minus"
    | "*" -> "Multiply"
    | "/" -> "Divide"
    | _ -> op
    
[<ExcludeFromCodeCoverage>]
let requestCalculating (url:string) =
    task {
        let! work = Task.Delay(2000)
        let! response = client.GetAsync(url)
        let ct = response.Content
        return! ct.ReadAsStringAsync()
    }
  
[<ExcludeFromCodeCoverage>]  
[<EntryPoint>]
let main (args:string[]) =
    while true do
        Console.WriteLine("Enter args")
        let input = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries)
        match input.Length with
        | 3 ->
            let operation = matchOperation input[1]
            let url = getUrl input[0] operation input[2] 
            let result = requestCalculating url
            Console.WriteLine(result.Result)
        | _ -> Console.WriteLine("Input len should be 3")
    0
        
        