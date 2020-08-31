using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Test_Example.Back.Services;

namespace Test_Example.Worker.Back
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IRabbitService _service;
        public Worker(ILogger<Worker> logger, IRabbitService service)
        {
            _logger = logger;
            _service = service;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Test_Example.Worker.Back is start.");
                _service.Run();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Test_Example.Worker.Back is crashed when start.");
                return base.StopAsync(cancellationToken);
            }
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested) {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
