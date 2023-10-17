open System
open System.Net.Http

let inputOperation operation =
    match operation with
    | "+" -> "Plus"
    | "-" -> "Minus"
    | "*" -> "Multiply"
    | "/" -> "Divide"
    | _ -> operation


let sendRequestAsync(client : HttpClient) (url : string) =
    async {
        let! response = Async.AwaitTask (client.GetAsync url)
        let! result = Async.AwaitTask (response.Content.ReadAsStringAsync())
        return result
    }
    
    
[<EntryPoint>]
let main args =
    let input = Console.ReadLine()
    use handler = new HttpClientHandler()
    use client = new HttpClient(handler)
    let args = input.Split(" ")
    match args.Length with
    | 3 ->
        let url = $"https://localhost:54327/calculate?value1={args[0]}&operation={inputOperation args[1]}&value2={args[2]}"
        let result = Async.RunSynchronously(sendRequestAsync client url)
        printf $"{result}"
    | _ -> printfn "Error"
    0