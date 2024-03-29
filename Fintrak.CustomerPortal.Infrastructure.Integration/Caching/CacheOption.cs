using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.CustomerPortal.Infrastructure.Integration.Caching
{
    public class CacheOption
    {
        public int? AbsoluteExpiration { get; set; } = 1;
        public CachePeriod AbsoluteExpirationPeriod { get; set; } = CachePeriod.Minutes;
        public int? SlidingExpiration { get; set; } = 1;
        public CachePeriod SlidingExpirationPeriod { get; set; } = CachePeriod.Minutes;
    }
}
