using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS.ActiveMQ;
using Apache.NMS;

namespace AMQClient
{
    class Program
    {
        private const string URI = "failover:(tcp://localhost:61616)";
        private const string DESTINATION = "test.queue.request.respond";
        static ActiveMQClient client;

        static void Main(string[] args)
        {
            Console.WriteLine("Creating and starting the client...");
            client = new ActiveMQClient(URI, DESTINATION);
            Console.WriteLine("Client started! Press CTRL+C to exit.");
            Console.WriteLine();

            while (true)
            {
                string input = Console.ReadLine();
                string response = client.SendMessage(client.CreateTextMessage(input));
                Console.WriteLine("receive response text: " + response);
            }
        }
    }
}
