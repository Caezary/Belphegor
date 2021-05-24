using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Quartz;
using Serilog;

namespace Belphegor
{
    [DisallowConcurrentExecution]
    public class BusyIdleVerifyingJob : IJob
    {
        private static readonly TimeSpan IdleTimeout = TimeSpan.FromMinutes(1);
        
        private readonly IToggleIdle _idleState;
        private readonly ILogger _logger;

        public BusyIdleVerifyingJob(IToggleIdle idleState)
        {
            _logger = Log.Logger.ForContext<BusyIdleVerifyingJob>();
            _idleState = idleState;
        }
        
        public Task Execute(IJobExecutionContext context)
        {
            if (!_idleState.IsIdleVerifyEnabled())
            {
                return Task.CompletedTask;
            }
            
            return Task.Run(() =>
            {
                var idleTime = IdleTimeExtractor.GetIdleTime();
                _logger.Information("Calculated idle time: {idleTime}", idleTime);
                if (idleTime >= IdleTimeout)
                {
                    SendKeys.SendWait("+");
                }
            });
        }
    }
}