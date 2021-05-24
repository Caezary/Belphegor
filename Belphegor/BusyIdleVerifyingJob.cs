using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Quartz;

namespace Belphegor
{
    [DisallowConcurrentExecution]
    public class BusyIdleVerifyingJob : IJob
    {
        private static readonly TimeSpan IdleTimeout = TimeSpan.FromMinutes(1);
        
        private readonly ApplicationState _applicationState;

        public BusyIdleVerifyingJob(ApplicationState applicationState)
        {
            _applicationState = applicationState;
        }
        
        public Task Execute(IJobExecutionContext context)
        {
            if (!_applicationState.IsIdleVerifyEnabled())
            {
                return Task.CompletedTask;
            }
            
            return Task.Run(() =>
            {
                var idleTime = IdleTimeExtractor.GetIdleTime();
                Console.WriteLine(idleTime);
                if (idleTime >= IdleTimeout)
                {
                    SendKeys.Send("{SHIFT}");
                }
            });
        }
    }

    public class IdleTimeExtractor
    {
        [StructLayout( LayoutKind.Sequential )]
        private struct LASTINPUTINFO
        {
            public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 cbSize;    
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dwTime;
        }
        
        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        public static TimeSpan GetIdleTime()
        {
            var inputInfo = new LASTINPUTINFO();
            inputInfo.cbSize = (uint) LASTINPUTINFO.SizeOf;

            if (!GetLastInputInfo(ref inputInfo))
            {
                return TimeSpan.Zero;
            }

            var delta = Environment.TickCount - (int) inputInfo.dwTime;
            
            return TimeSpan.FromMilliseconds(delta);
        }
    }
}