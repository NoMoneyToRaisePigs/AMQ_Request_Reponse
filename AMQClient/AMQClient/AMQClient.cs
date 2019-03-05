using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System.Threading;

namespace AMQClient
{
    class AMQClient
    {
    }

    public class ActiveMQClient
    {

        private ITemporaryQueue respondingChannel;

        private ISession session;
        private IConnection connection;
        private IMessageProducer messagePublisher;

        private string respondingText = null;

        public ActiveMQClient(string uri, string destinationQueue)
        {
            try
            {
                var connectionFactory = new ConnectionFactory(uri);
                connection = connectionFactory.CreateConnection();
                connection.Start();

                this.session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
                var destination = session.GetDestination(destinationQueue);
                this.messagePublisher = session.CreateProducer(destination);
                this.messagePublisher.DeliveryMode = MsgDeliveryMode.NonPersistent;


                this.respondingChannel = session.CreateTemporaryQueue();
                var responseConsumer = session.CreateConsumer(respondingChannel);
                responseConsumer.Listener += new MessageListener(responseConsumer_Listener);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public string SendMessage(IMessage message)
        {
            var correlationID = Guid.NewGuid().ToString();
            message.NMSCorrelationID = correlationID;
            message.NMSReplyTo = this.respondingChannel;


            this.messagePublisher.Send(message);

            //how stupid is this ??
            while (respondingText == null)
            {
                int xxx = 0;
            }

            string result = respondingText;
            respondingText = null;

            return result;
        }

        public ITextMessage CreateTextMessage(string text)
        {
            return this.session.CreateTextMessage(text);
        }

        private void responseConsumer_Listener(IMessage message)
        {
            respondingText = ((ITextMessage)message).Text;
        }
    }
}
