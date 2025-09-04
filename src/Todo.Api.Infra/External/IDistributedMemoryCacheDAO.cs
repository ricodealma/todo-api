namespace Todo.Api.Infra.External
{
    public interface IDistributedMemoryCacheDao
    {
        public void SetValue(string key, string value, TimeSpan expiration);
        public bool TryGetValue<T>(string key, out T? value);
        void DeleteValue(string key);
    }
}
