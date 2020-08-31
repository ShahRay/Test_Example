using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;

namespace Test_example.Messaging.Receiver
{
    public static class RabbitMqReceiver
    {
        public static async Task RequestListener(EventingBasicConsumer consumer, BlockingCollection<string> respQueue)
        {
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                respQueue.Add(response);
            };
        }
    }
}
