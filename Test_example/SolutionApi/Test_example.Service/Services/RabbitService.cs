using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test_example.Domain;
using Test_example.Messaging.Sender;

namespace Test_example.Service.Services
{
    public class RabbitService : IRabbitService
    {
        private readonly IRabbitMqSender _sender;
        public RabbitService(IRabbitMqSender sender)
        {
            _sender = sender;
        }
        public async Task<List<Request>> SendParams(Request request)
        {
            Log.Information("Send client_id: {0} and department_address: {1}. {2}", request.ClientId, request.DepartmentAddress, DateTime.Now);
            var data = JsonConvert.SerializeObject(request);
            try
            {
                string res = await _sender.SendToRabbitMqAsync("back_params_queue", data, "api_list_queue");
                return JsonConvert.DeserializeObject<List<Request>>(res);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Guid> SendRequest(Request request)
        {
            Log.Information("Send client_id: {0}, department_address: {1}, amount: {2}, currency: {3}. {4}", request.ClientId, request.DepartmentAddress, request.Amount, request.Currency, DateTime.Now);
            var data = JsonConvert.SerializeObject(request);
            try
            {
                string res = await _sender.SendToRabbitMqAsync("back_request_queue", data, "api_id_queue");
                var req = Guid.Parse(res);
                return req;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Request> SendRequestId(Guid id)
        {
            Log.Information("Send request_id: {0}. {1}", id.ToString(), DateTime.Now);
            try
            {
                string res = await _sender.SendToRabbitMqAsync("back_id_queue", id.ToString(), "api_request_queue");
                return JsonConvert.DeserializeObject<Request>(res);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
