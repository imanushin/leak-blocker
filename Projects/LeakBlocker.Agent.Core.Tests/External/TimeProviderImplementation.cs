using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.SystemTools;

namespace LeakBlocker.Agent.Core.Implementations
{
    class TimeProviderImplementation : ITimeProvider
    {
        public TimeProviderImplementation(Time currentTime)
        {
            CurrentTime = currentTime;
        }

        public Time CurrentTime
        {
            get;
            private set;
        }

        public TimeSpan SystemUptime
        {
            get { return TimeSpan.FromHours(1); }
        }
    }
}
