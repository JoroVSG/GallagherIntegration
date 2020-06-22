namespace RodeoParser

open System.Net
open System.Net.Sockets
open System.Text
open System.Threading.Tasks

module Say =
    open FSharp.Data
    let BASE_URL = "http://192.168.43.73:15001";
    
    type GallagherType = XmlProvider<Schema="./AnimalSchemaProposal.xsd">
    
    let discoveryEndpoint () =
        
        let ipep = new IPEndPoint(IPAddress.Any, 15000);

        let newsock = new UdpClient(ipep);

        let sender = new IPEndPoint(IPAddress.Any, 0);

        let data = newsock.Receive(ref sender);
        
        sender.Address.ToString()
        
        
        
       
            
    let getAsync url =
        
        async {
            let result = discoveryEndpoint()
            
            let httpClient = new System.Net.Http.HttpClient()
            let! response = httpClient.GetAsync(result + url) |> Async.AwaitTask
            response.EnsureSuccessStatusCode () |> ignore
            let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
            return content
        }
    let getSession (url) =
      async {
          let! animalsResponse = getAsync (url + "/session");
          let animals = GallagherType.Parse animalsResponse
          
          if (animals.Sessions.IsSome) then
             for session in animals.Sessions.Value.XElement.Descendants() do
             let s = GallagherType.Session(session)
             if s.Name.IsSome then
                printfn "%s" s.Name.Value
          else
             printfn "%s" animals.Header.CreatedBy
      }
      
    let getSessionsTask (url) =
        let task = new Task(fun () -> getSession(url) |> Async.StartImmediate)
        task.RunSynchronously()
        task
    
    let discoveryEndpointTask () =
        let task = discoveryEndpoint()
        //task.RunSynchronously();
        task
      
   