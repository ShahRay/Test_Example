using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using Test_example.Messaging.Options;
using Test_example.Messaging.Receiver;

namespace Test_example.Messaging.Sender
{
    public class RabbitMqSender : IRabbitMqSender
    {
        private readonly string _hostname;
        private IConnection _connection;
        public RabbitMqSender(IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            _hostname = rabbitMqOptions.Value.Hostname;
        }
        public async Task<string> SendToRabbitMqAsync(string queueName, string message, string replyQueueName)
        {
            var factory = new ConnectionFactory() { HostName = _hostname };
            try
            {
                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "RabbitMQ server is OFF!");
                throw ex;
            }
            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                var consumer = new EventingBasicConsumer(channel);
                BlockingCollection<string> respQueue = new BlockingCollection<string>();
                try
                {
                    await RabbitMqReceiver.RequestListener(consumer, respQueue);
                    channel.BasicConsume(consumer: consumer, queue: replyQueueName, autoAck: true);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Start receive again in 3 sec.");
                    await Task.Delay(3000);
                    try
                    {
                        await RabbitMqReceiver.RequestListener(consumer, respQueue);
                        channel.BasicConsume(consumer: consumer, queue: replyQueueName, autoAck: true);
                    }
                    catch (Exception exc)
                    {
                        Log.Error(exc, "Test_Example.Back is OFF!");
                        throw exc;
                    }
                }
                return respQueue.Take();
            }
        }
    }
}
