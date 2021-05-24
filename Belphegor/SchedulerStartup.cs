using System;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Belphegor
{
    public class SchedulerStartup
    {
        private readonly StdSchedulerFactory _schedulerFactory = new StdSchedulerFactory();
        private readonly IJobFactory _jobFactory;

        public SchedulerStartup(IJobFactory jobFactory)
        {
            _jobFactory = jobFactory;
        }

        public async Task Initialize()
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            scheduler.JobFactory = _jobFactory;
            
            await scheduler.Start();

            var (job, trigger) = CreateBusyIdleVerifyingJob();

            await scheduler.ScheduleJob(job, trigger);
        }

        private static (IJobDetail job, ITrigger trigger) CreateBusyIdleVerifyingJob()
        {
            var groupIdentity = $"{nameof(BusyIdleVerifyingJob)}Group";
            var jobIdentity = $"{nameof(BusyIdleVerifyingJob)}";
            var triggerIdentity = $"{nameof(BusyIdleVerifyingJob)}Trigger";
            
            var job = JobBuilder.Create<BusyIdleVerifyingJob>()
                .WithIdentity(jobIdentity, groupIdentity)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity(triggerIdentity, groupIdentity)
                .StartNow()
                .WithSimpleSchedule(s => s.WithInterval(TimeSpan.FromSeconds(10)).RepeatForever())
                .Build();
            
            return (job, trigger);
        }
    }
}