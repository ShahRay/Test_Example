using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Test_Example.Back.Domain;

namespace Test_Example.Back.Data.DapperHelpers
{
    public class DapperHelper : IDapperHelper<Request>
    {
        public async Task<Guid> AddAsync(IDbConnection connection, Request entity, string commandText)
        {
            var query = await connection.QueryFirstOrDefaultAsync<Guid>(commandText,
            new { client_id = entity.ClientId, department_address = entity.DepartmentAddress, amount = entity.Amount, currency = entity.Currency, client_ip = entity.ClientIp, request_status = entity.RequestStatus, date_time = entity.DateTime },
            commandType: CommandType.StoredProcedure);
            return query;
        }
        public async Task<Request> GetByIdAsync(IDbConnection connection, Guid id, string commandText)
        {
            var query = await connection.QueryFirstOrDefaultAsync<Request>(commandText, new { id }, commandType: CommandType.StoredProcedure);
            return query;
        }
        public async Task<IEnumerable<Request>> GetByParamsAsync(IDbConnection connection, Guid client_id, string department_address, string commandText)
        {
            var query = await connection.QueryAsync<Request>(commandText, new { client_id, department_address }, commandType: CommandType.StoredProcedure);
            return query;
        }
    }
}
