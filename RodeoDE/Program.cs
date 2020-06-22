using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace RodeoDE
{
    class Program
    {
        static void Main(string[] args)
        {
            var ipep = new IPEndPoint(IPAddress.Any, 15000);

            var newsock = new UdpClient(ipep);

            var sender = new IPEndPoint(IPAddress.Any, 0);

            var data = newsock.Receive(ref sender);
        
            
            RodeoParser.Say.getSessionsTask(sender.Address.ToString()).Wait();
            // var address = RodeoParser.Say.discoveryEndpointTask().Result;
            // Console.WriteLine(address.ToString());
            
        }
    }
}