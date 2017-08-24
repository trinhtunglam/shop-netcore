using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.VisualBasic;

namespace SERVICES.Caching
{
    public class RedisCacheManager : ICacheManager
    {
        private readonly IDatabase _db;
        private readonly IRedisConnectionWrapper _redisConnectionWrapper;
        private readonly IMemoryCacheManager _memoryCacheManager;
        private static readonly int MemoryCacheTime = Convert.ToInt32(TimeSpan.FromHours(1).TotalMinutes);

        #region Ctor

        public RedisCacheManager(IRedisConnectionWrapper redisConnectionWrapper,
            IMemoryCacheManager memoryCacheManager)
        {
            _redisConnectionWrapper = redisConnectionWrapper;
            _memoryCacheManager = memoryCacheManager;
            this._db = _redisConnectionWrapper.Database();
        }

        #endregion

        #region Utilities

        //protected virtual byte[] Serialize(object item)
        //{
        //    var jsonString = item.JsonSerialize();
        //    return Encoding.UTF8.GetBytes(jsonString);
        //}

        //protected virtual T Deserialize<T>(byte[] serializedObject)
        //{
        //    if (serializedObject == null)
        //        return default(T);

        //    //var jsonString = JToken.Parse(Encoding.UTF8.GetString(serializedObject)).ToString();
        //    var jsonString = Encoding.UTF8.GetString(serializedObject);

        //    return jsonString.JsonDeserialize<T>();
        //}

        #endregion

        #region Methods
        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        public virtual T Get<T>(string key)
        {
            var rValue = _db.StringGet(GenerateKey(key));

            if (!rValue.HasValue)
                return default(T);
            var result =Newtonsoft.Json.JsonConvert.DeserializeObject<T>(rValue);

            return result;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var rValue = await _db.StringGetAsync(GenerateKey(key));
            if (!rValue.HasValue)
                return default(T);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(rValue);

            return result;
        }

        public IEnumerable<T> GetByMultiKey<T>(IEnumerable<string> keys)
        {
            if (keys == null || !keys.Any())
                return default(IEnumerable<T>);

            var rediskeys = keys.Select(x => (RedisKey)GenerateKey(x)).ToArray();

            var rValues = _db.StringGet(rediskeys);
            if (rValues.All(x => !x.HasValue))
                return default(IEnumerable<T>);

            return rValues.Where(x => x.HasValue).Select(vaule => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(vaule)).ToList();
        }

        public async Task<IEnumerable<T>> GetByMultiKeyAsync<T>(IEnumerable<string> keys)
        {
            if (keys == null || !keys.Any())
                return default(IEnumerable<T>);

            var rediskeys = keys.Select(x => (RedisKey)GenerateKey(x)).ToArray();

            var rValues = await _db.StringGetAsync(rediskeys);
            if (rValues.All(x => !x.HasValue))
                return default(IEnumerable<T>);

            return rValues.Where(x => x.HasValue).Select(vaule => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(vaule)).ToList();
        }

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache time</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            var cacheKey = GenerateKey(key);

            var entryBytes = Newtonsoft.Json.JsonConvert.SerializeObject(data);

            if (cacheTime == 0)
            {
                _db.StringSet(cacheKey, entryBytes);
            }
            else
            {
                var expiresIn = TimeSpan.FromMinutes(cacheTime);
                _db.StringSet(cacheKey, entryBytes, expiresIn);
            }
        }


        /// <summary>
        /// Adds the specified key and HashEntry[] to the cache.
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="data"></param>
        public void HashSet(string redisKey, HashEntry[] data)
        {
            if (data == null)
                return;

            var key = GenerateKey(redisKey);

            _db.HashSet(key, data);
        }

        public async Task HashSetAsync(string redisKey, HashEntry[] data)
        {
            if (data == null)
                return;

            var key = GenerateKey(redisKey);

            await _db.HashSetAsync(key, data);
        }

        /// <summary>
        /// add or update a HashEntry to the cache with specified key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisHashKey"></param>
        /// <param name="redisHashValue"></param>
        public void HashSetToDictionary<T>(string redisKey, string redisHashKey, T redisHashValue)
        {
            if (redisHashValue == null)
                return;

            var key = GenerateKey(redisKey);

            var entryBytes = Newtonsoft.Json.JsonConvert.SerializeObject(redisHashValue);
            _db.HashSet(key, redisHashKey, entryBytes);
        }

        public async Task HashSetToDictionaryAsync<T>(string redisKey, string redisHashKey, T redisHashValue)
        {
            if (redisHashValue == null)
                return;

            var key = GenerateKey(redisKey);

            var entryBytes = Newtonsoft.Json.JsonConvert.SerializeObject(redisHashValue);
            await _db.HashSetAsync(key, redisHashKey, entryBytes);
        }

        /// <summary>
        /// Gets the value from HashEntry[] with the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="redisHashKey"></param>
        /// <returns></returns>
        public T GetItemFromHash<T>(string redisKey, string redisHashKey)
        {
            var rValue = _db.HashGet(GenerateKey(redisKey), redisHashKey);

            if (!rValue.HasValue)
                return default(T);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(rValue);
        }

        public async Task<T> GetItemFromHashAsync<T>(string redisKey, string redisHashKey)
        {
            var compareKey = GenerateKey(redisKey + redisHashKey);
            if (_memoryCacheManager.IsSet(compareKey))
                return _memoryCacheManager.Get<T>(compareKey);

            var rValue = await _db.HashGetAsync(GenerateKey(redisKey), redisHashKey);

            if (!rValue.HasValue)
            {
                _memoryCacheManager.Set(compareKey, 0, MemoryCacheTime);
                return default(T);
            }

            var resultValue = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(rValue);

            _memoryCacheManager.Set(compareKey, resultValue, MemoryCacheTime);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(rValue);
        }

        public async Task<IDictionary<string, string>> GetItemHashFromKeyAsync(string redisKey)
        {
            var entries = await _db.HashGetAllAsync(GenerateKey(redisKey));

            if (!entries.Any())
                return new Dictionary<string, string>();

            return entries.ToDictionary(x => x.Name.ToString(), x => x.Value.ToString());
        }

        public IEnumerable<T> GetMultipleItemFromHash<T>(string redisKey, IEnumerable<string> redisHashKeys)
        {
            var redisValues = redisHashKeys.Select(item => (RedisValue)item).ToArray();
            var rValues = _db.HashGet(GenerateKey(redisKey), redisValues);

            if (rValues.All(x => !x.HasValue))
                return default(IEnumerable<T>);

            return rValues.Where(x => x.HasValue).Select(value => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value)).ToList();
        }

        public async Task<IEnumerable<T>> GetMultipleItemFromHashAsync<T>(string redisKey, IEnumerable<string> redisHashKeys)
        {
            var redisValues = redisHashKeys.Select(item => (RedisValue)item).ToArray();
            var rValues = await _db.HashGetAsync(GenerateKey(redisKey), redisValues);

            return rValues.Select(value =>
            {
                if (value.HasValue)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
                }

                return default(T);
            }).ToList();
        }

        public async Task<bool> IsHashSetAsync(string redisKey, string redisHashKey)
        {
            var compareKey = GenerateKey(redisKey + redisHashKey);
            if (_memoryCacheManager.IsSet(compareKey))
                return _memoryCacheManager.Get<bool>(compareKey);

            bool isSet = await _db.HashExistsAsync(GenerateKey(redisKey), redisHashKey);
            _memoryCacheManager.Set(compareKey, isSet, MemoryCacheTime);

            return isSet;
        }

        public async Task<bool> IsHashSetWithoutMemoryAsync(string redisKey, string redisHashKey)
        {
            return await _db.HashExistsAsync(GenerateKey(redisKey), redisHashKey);
        }

        public async Task RemoveHashItemAsync(string redisKey, string redisHashKey)
        {
            await _db.HashDeleteAsync(GenerateKey(redisKey), redisHashKey);
        }

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisHashKey"></param>
        /// <returns></returns>
        public bool IsHashSet(string redisKey, string redisHashKey)
        {
            return _db.HashExists(GenerateKey(redisKey), redisHashKey);
        }

        /// <summary>
        ///  Removes the specified field from hash stored at key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisHashKey"></param>
        public void RemoveHashItem(string redisKey, string redisHashKey)
        {
            _db.HashDelete(GenerateKey(redisKey), redisHashKey);
        }

        public async Task SetAsync(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            var cacheKey = GenerateKey(key);

            var entryBytes = Newtonsoft.Json.JsonConvert.SerializeObject(data);

            if (cacheTime == 0)
            {
                await _db.StringSetAsync(cacheKey, entryBytes);
            }
            else
            {
                var expiresIn = TimeSpan.FromMinutes(cacheTime);
                await _db.StringSetAsync(cacheKey, entryBytes, expiresIn);
            }
        }

        /// <summary>
        /// Adds the list keys and list object to the cache.
        /// </summary>
        /// <param name="values">Values</param>
        public virtual void SetAll(IDictionary<string, string> values)
        {
            if (values.Count == 0)
                return;

            var keyValuePair = values.Select(x => new KeyValuePair<RedisKey, RedisValue>(GenerateKey(x.Key), Newtonsoft.Json.JsonConvert.SerializeObject(x.Value)));

            _db.StringSet(keyValuePair.ToArray(), When.NotExists);
        }

        public async Task SetAllAsync(IDictionary<string, string> values)
        {
            if (values.Count == 0)
                return;

            var keyValuePair = values.Select(x => new KeyValuePair<RedisKey, RedisValue>(GenerateKey(x.Key), Newtonsoft.Json.JsonConvert.SerializeObject(x.Value)));

            await _db.StringSetAsync(keyValuePair.ToArray(), When.NotExists);
        }

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Result</returns>
        public virtual bool IsSet(string key)
        {
            //little performance workaround here:
            //we use "PerRequestCacheManager" to cache a loaded object in memory for the current HTTP request.
            //this way we won't connect to Redis server 500 times per HTTP request (e.g. each time to load a locale or setting)
            if (_memoryCacheManager.IsSet(GenerateKey(key)))
                return true;

            return _db.KeyExists(GenerateKey(key));
        }

        public async Task<bool> IsSetAsync(string key)
        {
            //little performance workaround here:
            //we use "PerRequestCacheManager" to cache a loaded object in memory for the current HTTP request.
            //this way we won't connect to Redis server 500 times per HTTP request (e.g. each time to load a locale or setting)
            if (_memoryCacheManager.IsSet(GenerateKey(key)))
                return true;

            return await _db.KeyExistsAsync(GenerateKey(key));
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">/key</param>
        public virtual void Remove(string key)
        {
            _db.KeyDelete(GenerateKey(key));
            _memoryCacheManager.Remove(GenerateKey(key));
        }

        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(GenerateKey(key));
            _memoryCacheManager.Remove(GenerateKey(key));
        }

        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="pattern">pattern</param>
        public virtual void RemoveByPattern(string pattern)
        {
            var patternKey = GenerateKey(pattern);
            foreach (var ep in _redisConnectionWrapper.GetEndpoints())
            {
                var server = _redisConnectionWrapper.Server(ep);
                var keys = server.Keys(pattern: "*" + patternKey + "*");
                foreach (var key in keys)
                    _db.KeyDelete(key);
            }
        }

        public async Task RemoveByPatternAsync(string pattern)
        {
            var patternKey = GenerateKey(pattern);
            foreach (var ep in _redisConnectionWrapper.GetEndpoints())
            {
                var server = _redisConnectionWrapper.Server(ep);
                var keys = server.Keys(pattern: "*" + patternKey + "*");
                foreach (var key in keys)
                    await _db.KeyDeleteAsync(key);
            }
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        public virtual void Clear()
        {
            foreach (var ep in _redisConnectionWrapper.GetEndpoints())
            {
                var server = _redisConnectionWrapper.Server(ep);
                //we can use the code belwo (commented)
                //but it requires administration permission - ",allowAdmin=true"
                //server.FlushDatabase();

                //that's why we simply interate through all elements now
                var keys = server.Keys();
                foreach (var key in keys)
                    _db.KeyDelete(key);
            }
        }

        public async Task ClearAsync()
        {
            foreach (var ep in _redisConnectionWrapper.GetEndpoints())
            {
                var server = _redisConnectionWrapper.Server(ep);
                //we can use the code belwo (commented)
                //but it requires administration permission - ",allowAdmin=true"
                //server.FlushDatabase();

                //that's why we simply interate through all elements now
                var keys = server.Keys();
                foreach (var key in keys)
                    await _db.KeyDeleteAsync(key);
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose()
        {
            _redisConnectionWrapper?.Dispose();
        }

        #endregion

        #region Utility

        private string GenerateKey(string key)
        {
            return $"{Constants.AppCacheKey}:Cache:{key}";
        }



        #endregion
    }
}
