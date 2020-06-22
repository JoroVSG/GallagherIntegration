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
            RodeoParser.Say.getSessionsTask().Wait();
        }
    }
}