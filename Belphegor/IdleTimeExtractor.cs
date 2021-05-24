using System;
using System.Runtime.InteropServices;

namespace Belphegor
{
    public static class IdleTimeExtractor
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