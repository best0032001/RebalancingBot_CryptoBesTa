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
using RebalancingBot.Model.Interface;
using RebalancingBot.Model.Repository;
namespace RebalancingBot
{
    public class TimerBackgroundService : BackgroundService
    {
        private Timer _timer;
        private readonly IHttpClientFactory _clientFactory;
        private IBitkubRepository _bitkubRepository;

        public TimerBackgroundService(IHttpClientFactory clientFactory, IBitkubRepository bitkubRepository)
        {
            _clientFactory = clientFactory;
            _bitkubRepository = bitkubRepository;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
             _bitkubRepository.Worker();
        }
    }
}
