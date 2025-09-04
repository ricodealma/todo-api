using StackExchange.Redis;

namespace Todo.Api.Infra.External
{
    public interface IDistributedMemoryCacheDao
    {
        public void SetValue(string key, string value, TimeSpan expiration);
        public bool TryGetValue<T>(string key, out T? value);
        public void SetKeyExpire(string key, TimeSpan expiration, CommandFlags flags = CommandFlags.FireAndForget);
        bool TryGetGroupValue<T>(string key, out List<T> value);
        public void SetValueToGroup<T>(string key, T value, string groupName, TimeSpan expiration, CommandFlags flags = CommandFlags.FireAndForget);
        public void RemoveAllGroups();
        void DeleteValue(string key);
    }
}
