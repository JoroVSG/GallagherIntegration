namespace RodeoParser

open System.Net
open System.Net.Sockets
open System.Text
open System.Threading.Tasks
open System
open System.Net.Http

module Say =
    open FSharp.Data
    
    type GallagherType = XmlProvider<Schema="./AnimalSchemaProposal.xsd">
    
    let discoveryEndpoint () =
        
        let ipep = new IPEndPoint(IPAddress.Any, 15000);

        let newsock = new UdpClient(ipep);

        let mutable sender = new IPEndPoint(IPAddress.Any, 0);

        let data = newsock.Receive(&sender);
        
        sender.Address.ToString();
       
            
    let getAsync url =
        async {
            let baseUrl = discoveryEndpoint();
            let fullUrl = sprintf "http://%s:%i%s" baseUrl 15001 url
            let httpClient = new HttpClient();
            try
                use! resp = httpClient.GetAsync(fullUrl, HttpCompletionOption.ResponseHeadersRead) |> Async.AwaitTask                       
                resp.EnsureSuccessStatusCode |> ignore

                let! str = resp.Content.ReadAsStringAsync() |> Async.AwaitTask
                return str
            with
                | :? HttpRequestException as ex -> return ex.Message
        }
    
    let getSession () =
      async {
          let animalsResponse = getAsync "/sessions" |> Async.RunSynchronously;
          let animals = GallagherType.Parse animalsResponse
          
          if (animals.Sessions.IsSome) then
             for session in animals.Sessions.Value.XElement.Descendants() do
             let s = GallagherType.Session(session)
             if s.Name.IsSome then
                printfn "%s" s.Name.Value
          else
             printfn "%s" animals.Header.CreatedBy
      }
      
    let getSessionsTask () =
        let task = new Task(fun () -> getSession() |> Async.StartImmediate)
        task.RunSynchronously()
        task
      
   