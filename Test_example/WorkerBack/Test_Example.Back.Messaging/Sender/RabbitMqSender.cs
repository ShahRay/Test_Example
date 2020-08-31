using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;
using Test_Example.Back.Messaging.Options;

namespace Test_Example.Back.Messaging.Sender
{
    public class RabbitMqSender : IRabbitMqSender
    {
        private readonly string _hostname;
        public RabbitMqSender(IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            _hostname = rabbitMqOptions.Value.Hostname;
        }
        public async Task SendToRabbitMqAsync(string queueName, string message)
        {
            var factory = new ConnectionFactory() { HostName = _hostname};

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            }
        }
    }
}
