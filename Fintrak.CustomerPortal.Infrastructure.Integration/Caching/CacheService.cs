using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.CustomerPortal.Infrastructure.Integration.Caching
{
    public class CacheService : ICacheService
    {
        private readonly ILogger<CacheService> _logger;
        private readonly IDistributedCache _distributedCache;

        public CacheService(ILoggerFactory loggerFactory, IDistributedCache distributedCache)
        {
            _logger = loggerFactory.CreateLogger<CacheService>();
            _distributedCache = distributedCache;
        }

        //public CacheService(ILoggerFactory loggerFactory)
        //{
        //    _logger = loggerFactory.CreateLogger<CacheService>();
        //}

        public async Task<CacheResult<T>> GetAsync<T>(string cacheKey)
        {
            CacheResult<T> result = new CacheResult<T>();

            try
            {
                var cacheData = await _distributedCache.GetAsync(cacheKey);
                if (cacheData != null)
                {
                    var serializedData = Encoding.UTF8.GetString(cacheData);
                    result.Result = JsonConvert.DeserializeObject<T>(serializedData);
                    result.ResultExist = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"CacheService GetAsync. Key: {cacheKey}");
            }

            return result;
        }

        public async Task SetAsync<T>(string cacheKey, T data, CacheOption cacheOption)
        {
            var serializedData = JsonConvert.SerializeObject(data, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            var redisData = Encoding.UTF8.GetBytes(serializedData);

            DistributedCacheEntryOptions options = GetOptions(cacheOption);

            try
            {
                await _distributedCache.SetAsync(cacheKey, redisData, options);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"CacheService SetAsync. Key: {cacheKey}");
            }
        }

        public async Task RemoveAsync(string cacheKey)
        {
            try
            {
                await _distributedCache.RemoveAsync(cacheKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"CacheService RemoveAsync. Key: {cacheKey}");
            }
            
        }

        private DistributedCacheEntryOptions GetOptions(CacheOption cacheOption)
        {
            DistributedCacheEntryOptions options = null;

            if (cacheOption != null)
            {
                options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(GetExpirationDateTime(cacheOption.AbsoluteExpiration, cacheOption.AbsoluteExpirationPeriod)).SetSlidingExpiration(GetExpirationTimeSpan(cacheOption.AbsoluteExpiration, cacheOption.AbsoluteExpirationPeriod));
            }

            return options;
        }

        private DateTime GetExpirationDateTime(int? expiration, CachePeriod cachePeriod)
        {
            if(cachePeriod == CachePeriod.Ticks)
            {
                return DateTime.Now.AddTicks(expiration.Value);
            }
            else if (cachePeriod == CachePeriod.Milliseconds)
            {
                return DateTime.Now.AddMilliseconds(expiration.Value);
            }
            else if (cachePeriod == CachePeriod.Seconds)
            {
                return DateTime.Now.AddSeconds(expiration.Value);
            }
            else if (cachePeriod == CachePeriod.Minutes)
            {
                return DateTime.Now.AddMinutes(expiration.Value);
            }
            else if (cachePeriod == CachePeriod.Hours)
            {
                return DateTime.Now.AddHours(expiration.Value);
            }
            else if (cachePeriod == CachePeriod.Days)
            {
                return DateTime.Now.AddDays(expiration.Value);
            }
            else if (cachePeriod == CachePeriod.Months)
            {
                return DateTime.Now.AddMonths(expiration.Value);
            }
            else if (cachePeriod == CachePeriod.Years)
            {
                return DateTime.Now.AddYears(expiration.Value);
            }
            else
            {
                return DateTime.Now.AddMinutes(expiration.Value);
            }
        }

        private TimeSpan GetExpirationTimeSpan(int? expiration, CachePeriod cachePeriod)
        {
            if (cachePeriod == CachePeriod.Ticks)
            {
                return TimeSpan.FromTicks(expiration.Value);
            }
            else if (cachePeriod == CachePeriod.Milliseconds)
            {
                return TimeSpan.FromMilliseconds(expiration.Value);
            }
            else if (cachePeriod == CachePeriod.Seconds)
            {
                return TimeSpan.FromSeconds(expiration.Value);
            }
            else if (cachePeriod == CachePeriod.Minutes)
            {
                return TimeSpan.FromMinutes(expiration.Value);
            }
            else if (cachePeriod == CachePeriod.Hours)
            {
                return TimeSpan.FromHours(expiration.Value);
            }
            else if (cachePeriod == CachePeriod.Days)
            {
                return TimeSpan.FromDays(expiration.Value);
            }
            else
            {
                return TimeSpan.FromMinutes(expiration.Value);
            }
        }
    }

    public interface ICacheService
    {
        Task<CacheResult<T>> GetAsync<T>(string cacheKey);

        Task SetAsync<T>(string cacheKey, T data, CacheOption cacheOption);

        Task RemoveAsync(string cacheKey);

    }
}
