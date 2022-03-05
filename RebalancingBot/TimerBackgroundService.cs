using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
namespace RebalancingBot
{
    public class TimerBackgroundService : BackgroundService
    {
        private Timer _timer;
        private readonly IHttpClientFactory _clientFactory;
        public TimerBackgroundService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            Console.WriteLine("test");
        }
    }
}
