using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.CustomerPortal.Infrastructure.Integration.Caching
{
    public enum CachePeriod
    {
        Ticks = 1,
        Milliseconds = 2,
        Seconds = 3,
        Minutes = 4,
        Hours = 5,
        Days = 6,
        Months = 7,
        Years = 8
    }
}
