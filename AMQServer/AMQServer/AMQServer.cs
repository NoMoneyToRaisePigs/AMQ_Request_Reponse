using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS;
using Apache.NMS.Util;
using Apache.NMS.ActiveMQ;


namespace AMQServer
{
    public class AMQServer
    {
    }

    public class ActiveMQServer
    {
        private IConnection connection;
        private ISession session;
        private IMessageProducer respondingMessagePublisher;

        private Dictionary<string, string> students = new Dictionary<string, string>()
        {
            {"1","Jack"},
            {"2","Tom"},
            {"3","James"},
            {"4","GF"},
            {"5","Paul"},
            {"6","Eugene"},
            {"7","Tony"},
            {"8","Viki"}
        };

        public ActiveMQServer(string uri, string destinationQueue)
        {
            try
            {
                ConnectionFactory connectionFactory = new ConnectionFactory(uri);
                connection = connectionFactory.CreateConnection();
                connection.Start();


                session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
                var queue = this.session.GetDestination(destinationQueue);
                var consumer = this.session.CreateConsumer(queue);
                consumer.Listener += new MessageListener(consumer_Listener);

                respondingMessagePublisher = this.session.CreateProducer();
                respondingMessagePublisher.DeliveryMode = MsgDeliveryMode.NonPersistent;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void consumer_Listener(IMessage message)
        {
            string id = ((ITextMessage)message).Text;
            Console.WriteLine("Received StudentID: " + id);


            ITextMessage response = this.session.CreateTextMessage();
            response.NMSCorrelationID = message.NMSCorrelationID;
            response.Text = GetMessage(id);

            respondingMessagePublisher.Send(message.NMSReplyTo, response);
        }

        private string GetMessage(string id)
        {
            string text = "No such a student, try 1 to 8";

            if (students.ContainsKey(id))
            {
                text = "StudentID: " + id + "'s name is " + students[id];
            }

            return text;
        }
    }
}
