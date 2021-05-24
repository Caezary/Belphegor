using System;
using Quartz;
using Quartz.Spi;

namespace Belphegor
{
    public class PoorMansJobFactory : IJobFactory
    {
        private readonly IToggleIdle _idleToggler;

        public PoorMansJobFactory(IToggleIdle idleToggler)
        {
            _idleToggler = idleToggler;
        }
        
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            if (bundle.JobDetail.JobType == typeof(BusyIdleVerifyingJob))
            {
                return new BusyIdleVerifyingJob(_idleToggler);
            }
            
            throw new ArgumentException(nameof(bundle.JobDetail.JobType));
        }

        public void ReturnJob(IJob job)
        {
        }
    }
}