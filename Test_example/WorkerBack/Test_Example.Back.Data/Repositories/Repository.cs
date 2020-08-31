using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Test_Example.Back.Data.DapperHelpers;
using Test_Example.Back.Data.Queries;
using Test_Example.Back.Domain;

namespace Test_Example.Back.Data.Repositories
{
    public class Repository : BaseRepository, IRepository
    {
        private readonly ICommandText _commandText;
        private readonly IDapperHelper<Request> _dapperHelper;
        private readonly ILogger<Repository> _logger;
        public Repository(ICommandText commandText, DbConnection connection, IDapperHelper<Request> dapperHelper, ILogger<Repository> logger) : base(connection)
        {
            _commandText = commandText;
            _dapperHelper = dapperHelper;
            _logger = logger;
        }
        public async Task<Guid> AddAsync(Request entity)
        {
            try
            {
                return await WithConnection(async conn =>
                {
                    var query = await _dapperHelper.AddAsync(conn, entity, _commandText.Add);
                    return query;
                });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Reposytory is ERROR!");
                throw ex;
            }
        }

        public async Task<Request> GetByIdAsync(Guid id)
        {
            try
            {
                return await WithConnection(async conn =>
                {
                    var query = await _dapperHelper.GetByIdAsync(conn, id, _commandText.GetById);
                    return query;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reposytory is ERROR!");
                throw ex;
            }
        }

        public async Task<IEnumerable<Request>> GetByParamsAsync(Guid client_id, string department_address)
        {
            try
            {
                return await WithConnection(async conn =>
                {
                    var query = await _dapperHelper.GetByParamsAsync(conn, client_id, department_address, _commandText.GetByParams);
                    return query;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reposytory is ERROR!");
                throw ex;
            }
        }
    }
}
