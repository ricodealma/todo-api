using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using Todo.Api.Domain.SeedWork;
using Todo.Api.Domain.SeedWork.Json;
using IDatabase = StackExchange.Redis.IDatabase;

namespace Todo.Api.Infra.External
{
    public class DistributedMemoryCacheDao(IConnectionMultiplexer connectionMultiplexer, ILogger<DistributedMemoryCacheDao> logger) : IDistributedMemoryCacheDao
    {
        private readonly IDatabase? _redisDatabase = connectionMultiplexer?.GetDatabase();
        private readonly ILogger<DistributedMemoryCacheDao> _logger = logger;

        public void SetValue(string key, string value, TimeSpan expiration)
        {
            try
            {
                if (_redisDatabase is null)
                {
                    _logger.LogError("Redis is unavailable. As a result, set value operation will be skipped.");
                    return;
                }
                _redisDatabase!.StringSet($"{Constant.APP_REDIS_CACHE_ENTITY_BASE_NAME}{key}", value, expiration, When.Always, CommandFlags.FireAndForget);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, JsonConvert.SerializeObject(e));
            }


        }
        public bool TryGetValue<T>(string key, out T? value)
        {
            value = default;
            try
            {
                if (_redisDatabase is null)
                {
                    _logger.LogError("Redis is unavailable. As a result, try get value operation will be skipped.");
                    return false;
                }
                string? returnedValue = _redisDatabase!.StringGet($"{Constant.APP_REDIS_CACHE_ENTITY_BASE_NAME}{key}");

                if (string.IsNullOrEmpty(returnedValue))
                    return false;

                if (typeof(T) == typeof(string))
                {
                    value = (T)(object)returnedValue;
                    return true;
                }

                value = JsonHelper.Deserialize<T>(returnedValue);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, JsonConvert.SerializeObject(e));
                return false;
            }
        }
        public void DeleteValue(string key)
        {
            try
            {
                if (_redisDatabase is null)
                {
                    _logger.LogError("Redis is unavailable. As a result, delete value operation will be skipped.");
                    return;
                }

                string fullKey = $"{Constant.APP_REDIS_CACHE_ENTITY_BASE_NAME}{key}";
                _redisDatabase.KeyDelete(fullKey, CommandFlags.FireAndForget);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, JsonConvert.SerializeObject(e));
            }
        }
        public void SetValueToGroup<T>(string key, T value, string groupName, TimeSpan expiration, CommandFlags flags = CommandFlags.FireAndForget)
        {
            try
            {
                if (_redisDatabase is null)
                {
                    _logger.LogError("Redis is unavailable. As a result, set value to group will be skipped.");
                    return;
                }
                if (value == null)
                {
                    _logger.LogWarning("Tentativa de salvar valor nulo para a chave '{Key}' no grupo '{Group}'", key, groupName);
                    return;
                }

                var serializedValue = JsonHelper.Serialize(value);
                SetValue(key, serializedValue, expiration);

                var groupKey = $"{Constant.APP_REDIS_CACHE_GROUP_SEARCH_BASE_NAME}{groupName}";
                var entityKey = $"{Constant.APP_REDIS_CACHE_ENTITY_BASE_NAME}{key}";

                _redisDatabase?.SetAdd(groupKey, entityKey, flags);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, JsonConvert.SerializeObject(e));
            }
        }
        public bool TryGetGroupValue<T>(string key, out List<T> value)
        {
            value = [];
            try
            {
                if (_redisDatabase is null)
                {
                    _logger.LogError("Redis database is not initialized. Skipping TryGetGroupValue operation.");
                    return false;
                }
                var keys = _redisDatabase?.SetMembers($"{Constant.APP_REDIS_CACHE_GROUP_SEARCH_BASE_NAME}{key}");
                var resultList = new List<T>();

                foreach (var item in keys)
                {
                    var cachedValue = _redisDatabase!.StringGet(item.ToString());

                    if (!cachedValue.IsNullOrEmpty)
                    {
                        var entity = JsonConvert.DeserializeObject<T>(cachedValue);
                        resultList.Add(entity);
                    }
                }

                if (resultList.Count > default(int))
                {
                    value = resultList;
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, JsonConvert.SerializeObject(e));
                return false;
            }
        }
        public void RemoveAllGroups()
        {
            try
            {
                RemoveByPattern($"{Constant.APP_REDIS_CACHE_GROUP_SEARCH_BASE_NAME}*");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error removing all groups from Redis cache.");
            }
        }
        public void RemoveByPattern(string pattern)
        {
            try
            {
                const string luaScript = @"
            local cursor = '0'
            repeat
                local result = redis.call('SCAN', cursor, 'MATCH', ARGV[1], 'COUNT', 1000)
                cursor = result[1]
                local keys = result[2]
                if #keys > 0 then
                    redis.call('DEL', unpack(keys))
                end
            until cursor == '0'
            return true
        ";
                if (_redisDatabase is null)
                {
                    _logger.LogError("Redis is unavailable. As a result, cache clear operation will be skipped.");
                    return;
                }

                _redisDatabase!.ScriptEvaluate(luaScript, values: [pattern], flags: CommandFlags.FireAndForget);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error removing items from Redis cache.");
            }
        }
        public void SetKeyExpire(string key, TimeSpan expiration, CommandFlags flags = CommandFlags.FireAndForget)
        {
            try
            {
                if (_redisDatabase is null)
                {
                    _logger.LogError("Redis is unavailable. As a result, cache set key expiration will be skipped.");
                    return;
                }
                _redisDatabase!.KeyExpire(key, expiration, flags);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, JsonConvert.SerializeObject(e));
            }
        }

    }
}
