using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Test_Example.Back.Data.DapperHelpers
{
    public interface IDapperHelper<T> where T : class
    {
        Task<T> GetByIdAsync(IDbConnection connection, Guid id, string commandText);
        Task<IEnumerable<T>> GetByParamsAsync(IDbConnection connection, Guid client_id, string department_address, string commandText);
        Task<Guid> AddAsync(IDbConnection connection, T entity, string commandText);
    }
}
