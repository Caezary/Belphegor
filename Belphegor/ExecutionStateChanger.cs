using System;
using System.Runtime.InteropServices;

namespace Belphegor
{
    public static class ExecutionStateChanger
    {
        [Flags]
        private enum EXECUTION_STATE :uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
        }
        
        [DllImport("kernel32.dll", CharSet = CharSet.Auto,SetLastError = true)]
        private static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        public static void PreventIdle() =>
            SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);

        public static void AllowIdle() =>
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
    }
}