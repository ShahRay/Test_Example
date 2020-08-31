using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test_Example.Back.Domain;

namespace Test_Example.Back.Data.Repositories
{
    public interface IRepository
    {
        Task<Request> GetByIdAsync(Guid id);
        Task<IEnumerable<Request>> GetByParamsAsync(Guid client_id, string department_address);
        Task<Guid> AddAsync(Request entity);

    }
}
