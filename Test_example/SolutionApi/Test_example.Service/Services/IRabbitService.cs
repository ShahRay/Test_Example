using System;
using System.Collections.Generic;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using Test_example.Domain;

namespace Test_example.Service.Services
{
    public interface IRabbitService
    {
        Task<Guid> SendRequest(Request request);
        Task<Request> SendRequestId(Guid id);
        Task<List<Request>> SendParams(Request request);
    }
}
