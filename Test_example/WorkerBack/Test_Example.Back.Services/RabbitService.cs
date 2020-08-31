using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;
using Test_Example.Back.Data.Repositories;
using Test_Example.Back.Domain;
using Test_Example.Back.Messaging.Options;
using Test_Example.Back.Messaging.Receiver;
using Test_Example.Back.Messaging.Sender;

namespace Test_Example.Back.Services
{
    public class RabbitService : IRabbitService
    {
        private RabbitMqReceiver _receiver;
        private readonly IRepository _repository;
        private readonly IRabbitMqSender _sender;
        private readonly ILogger<RabbitService> _logger;
        public RabbitService(IOptions<RabbitMqConfiguration> rabbitMqOptions, IRepository repository, IRabbitMqSender sender, ILogger<RabbitService> logger)
        {
            _sender = sender;
            _repository = repository;
            _logger = logger;
            try
            {
                _receiver = new RabbitMqReceiver(rabbitMqOptions, logger);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public void Run()
        {
            try
            {
                _logger.LogInformation("RabbitService start work.");
                _receiver.RequestAction += RequestHandler;
                _receiver.IdAction += IdHandler;
                _receiver.ParamsAction += ParamsHandler;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        private async Task RequestHandler(object model, BasicDeliverEventArgs ea)
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            var data = JsonConvert.DeserializeObject<Request>(message);
            _logger.LogInformation("Get request for new add. client_id: {0}, department_address: {1}, amount: {2}, currency: {3}, client_ip: {4}, date_time: {5}",
                data.ClientId, data.DepartmentAddress, data.Amount, data.Currency, data.ClientIp, data.DateTime);
            try
            {
                var id = await _repository.AddAsync(data);
                await _sender.SendToRabbitMqAsync("api_id_queue", id.ToString());
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private async Task IdHandler(object model, BasicDeliverEventArgs ea)
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            var data = Guid.Parse(message);
            _logger.LogInformation("Get request for find by request_id. request_id: {0}", data);
            try
            {
                var request = await _repository.GetByIdAsync(data);
                await _sender.SendToRabbitMqAsync("api_request_queue", JsonConvert.SerializeObject(request));
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private async Task ParamsHandler(object model, BasicDeliverEventArgs ea)
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            var data = JsonConvert.DeserializeObject<Request>(message);
            _logger.LogInformation("Get request for find by params. client_id: {0}, department_address: {1}",
                data.ClientId, data.DepartmentAddress);
            try
            {
                var list = await _repository.GetByParamsAsync(data.ClientId, data.DepartmentAddress);
                await _sender.SendToRabbitMqAsync("api_list_queue", JsonConvert.SerializeObject(list));
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
