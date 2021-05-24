using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            var applicationState = new ApplicationState();
            var context = new BelphegorApplicationContext(applicationState);

            var jobFactory = new PoorMansJobFactory(applicationState);
            var schedulerStartup = new SchedulerStartup(jobFactory);

            schedulerStartup.Initialize().Wait();
            Application.Run(context);
        }
    }
}