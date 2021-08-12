using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LittlePet
{
    internal struct LASTINPUTINFO
    {
        public uint cbSize;
        public uint dwTime;
    }



    /// <summary>
    /// Contains various tricks for dealing with things that need a native call for.
    /// </summary>
    public static class InteropWorkarounds
    {
        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public const UInt32 SWP_NOSIZE = 0x0001;
        public const UInt32 SWP_NOMOVE = 0x0002;
        public const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        /// <summary>
        /// This allows for setting the window position. Most importantly, it allows us
        /// to set special flags that aren't normally available. Particularly important,
        /// the "stay on top" flag.
        /// </summary>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);



        /// <summary>
        /// This gets us the timestamp of when the user last interacted with the computer.
        /// Not this program, the computer.
        /// </summary>
        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);



        /// <summary>
        /// A wrapper to get an offset for how long ago the user last interacted with
        /// their computer. It makes use of User32.dll.
        /// </summary>
        /// <returns>The number of seconds since the user last interacted with their PC.</returns>
        public static long GetLastInputTime()
        {
            uint idleTime = 0;
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            lastInputInfo.dwTime = 0;

            uint envTicks = (uint)Environment.TickCount;

            if (GetLastInputInfo(ref lastInputInfo))
            {
                uint lastInputTick = lastInputInfo.dwTime;

                idleTime = envTicks - lastInputTick;
            }

            return ((idleTime > 0) ? (idleTime / 1000) : 0);
        }
    }
}
