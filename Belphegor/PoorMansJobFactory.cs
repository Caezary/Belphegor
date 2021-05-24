using System;
using Quartz;
using Quartz.Spi;

namespace Belphegor
{
    public class PoorMansJobFactory : IJobFactory
    {
        private readonly ApplicationState _applicationState;

        public PoorMansJobFactory(ApplicationState applicationState)
        {
            _applicationState = applicationState;
        }
        
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            if (bundle.JobDetail.JobType == typeof(BusyIdleVerifyingJob))
            {
                return new BusyIdleVerifyingJob(_applicationState);
            }
            
            throw new ArgumentException(nameof(bundle.JobDetail.JobType));
        }

        public void ReturnJob(IJob job)
        {
        }
    }
}