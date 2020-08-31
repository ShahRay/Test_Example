using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Test_example.Messaging.Options;
using Test_example.Messaging.Sender;

namespace Test_example.Service.Services
{
    public static class ServicesConfiguration
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMq"));
            services.AddTransient(typeof(IRabbitMqSender), typeof(RabbitMqSender));
        }
    }
}
