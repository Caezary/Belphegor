using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Belphegor
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            
            var configuration = GetConfiguration();
            var idlingSettings = GetIdlingSettings(configuration);
            
            ConfigureLogger();

            IToggleIdle idleStateToggler = new IdleStateToggler();

            if(idlingSettings.Style == "SendKeys")
            {
                var jobFactory = new PoorMansJobFactory(idleStateToggler);
                var schedulerStartup = new SchedulerStartup(jobFactory);
                schedulerStartup.Initialize().Wait();
            }
            else
            {
                idleStateToggler = new ExecutionStateSwitchingIdleStateTogglingDecorator(idleStateToggler);
            }
            
            var context = new BelphegorApplicationContext(idleStateToggler);
            Application.Run(context);
        }

        private static IdlingSettings GetIdlingSettings(IConfigurationRoot configuration)
        {
            return configuration
                .GetSection(nameof(IdlingSettings))
                .Get<IdlingSettings>();
        }

        private static IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        private static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt")
                .CreateLogger();
        }
    }
}