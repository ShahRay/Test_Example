using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using Test_Example.Back.Data.DapperHelpers;
using Test_Example.Back.Data.Queries;
using Test_Example.Back.Data.Repositories;
using Test_Example.Back.Domain;
using Test_Example.Back.Messaging.Options;
using Test_Example.Back.Messaging.Receiver;
using Test_Example.Back.Messaging.Sender;

namespace Test_Example.Back.Services
{
    public static class ServicesConfiguration
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(typeof(IRepository), typeof(Repository));
            services.Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMq"));
            services.AddTransient(typeof(IRabbitMqSender), typeof(RabbitMqSender));
            services.AddTransient(typeof(ICommandText), typeof(CommandText));
            services.AddTransient(typeof(IDapperHelper<Request>), typeof(DapperHelper));
            services.AddSingleton<DbConnection, SqlConnection>(provider =>
            {
                return new SqlConnection
                {
                    ConnectionString = configuration.GetConnectionString("myconn")
                };
            });
        }
    }
}
