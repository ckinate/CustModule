using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.CustomerPortal.Infrastructure.Integration.Caching
{
    public class CacheResult<T>
    {
        public bool ResultExist { get; set; }
        public T Result { get; set; }
    }
}
