using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.SystemTools.Win32;
using LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes;

namespace LeakBlocker.Libraries.SystemTools.Implementations
{
    internal sealed class TimeProvider : ITimeProvider
    {
        Time ITimeProvider.CurrentTime
        {
            get
            {
                using (var frequency = new UnmanagedLong())
                using (var counter = new UnmanagedLong())
                {
                    if (!NativeMethods.QueryPerformanceFrequency(+frequency))
                        NativeErrors.ThrowLastErrorException("QueryPerformanceFrequency");

                    if (!NativeMethods.QueryPerformanceCounter(+counter))
                        NativeErrors.ThrowLastErrorException("QueryPerformanceCounter");

                    ulong valueWithoutSeconds = (counter.UValue / frequency) * frequency;

                    double secondParts = (counter.UValue - valueWithoutSeconds) / (double)frequency.UValue;

                    var precisionTicks = (long)(secondParts * 1000.0 * 1000.0 * 10.0);
                    long currentTicks = Time.Now.Ticks;

                    return new Time((currentTicks - currentTicks % 1000 * 10) + (precisionTicks % 1000 * 10));
                }
            }
        }

        TimeSpan ITimeProvider.SystemUptime
        {
            get
            {
                return TimeSpan.FromMilliseconds(NativeMethods.GetTickCount());
            }
        }
    }
}
