using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading;
using System.Threading.Tasks;
using Test_Example.Back.Messaging.Options;
using Microsoft.Extensions.Logging;

namespace Test_Example.Back.Messaging.Receiver
{
    public class RabbitMqReceiver : BackgroundService
    {
        private readonly string _hostname;
        private IModel _channel;
        private IConnection _connection;
        private ILogger _logger;
        public Func<object, BasicDeliverEventArgs, Task> RequestAction;
        public Func<object, BasicDeliverEventArgs, Task> IdAction;
        public Func<object, BasicDeliverEventArgs, Task> ParamsAction;
        public RabbitMqReceiver(IOptions<RabbitMqConfiguration> rabbitMqOptions, ILogger logger)
        {
            _hostname = rabbitMqOptions.Value.Hostname;
            _logger = logger;
            try
            {
                InitializeRabbitMqListener();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        private void InitializeRabbitMqListener()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostname
                };
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                
                    RequestListener();
                    IdListener();
                    ParamsListener();
                
                _logger.LogInformation("RabbitMqReceiver start work.");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "RabbitMQ server is OFF!");
                throw ex;
            }
        }
        private void RequestListener()
        {
            _channel.QueueDeclare(queue: "api_id_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare(queue: "back_request_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) => RequestAction.Invoke(model, ea);
            _channel.BasicConsume(queue: "back_request_queue", autoAck: true, consumer: consumer);
        }
        private void IdListener()
        {
            _channel.QueueDeclare(queue: "api_request_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare(queue: "back_id_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) => IdAction.Invoke(model, ea);
            _channel.BasicConsume(queue: "back_id_queue", autoAck: true, consumer: consumer);
        }
        private void ParamsListener()
        {
            _channel.QueueDeclare(queue: "api_list_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare(queue: "back_params_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) => ParamsAction.Invoke(model, ea);
            _channel.BasicConsume(queue: "back_params_queue", autoAck: true, consumer: consumer);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            RequestListener();
            IdListener();
            ParamsListener();
            return Task.CompletedTask;
        }
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
