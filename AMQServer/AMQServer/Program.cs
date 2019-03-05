using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMQServer
{
    class Program
    {
        private const string URI = "failover:(tcp://localhost:61616)";
        private const string DESTINATION = "test.queue.request.respond";
        static ActiveMQServer server;
        static void Main(string[] args)
        {

            Console.WriteLine("Creating and starting the server...");
            server = new ActiveMQServer(URI, DESTINATION);
            Console.WriteLine("Server started! Press CTRL+C to exit.");
            Console.WriteLine();


            Console.ReadLine();
        }
    }
}
