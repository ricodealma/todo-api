using Todo.Api.Domain.SeedWork.ErrorResult;
using Todo.Api.Domain.SeedWork;
using Todo.Api.Infra.External;
using Todo.Api.Domain.Aggregates.Todo;
using Todo.Api.Domain.Aggregates.Todo.Entities;
using Newtonsoft.Json;
using Todo.Api.Infra.Data.Todo.Entities;
using Todo.Api.Domain.Aggregates.Todo.Entities.Filter;
using Multipay.Receivable.Microservice.Api.Domain.SeedWork;
using Multipay.Receivable.Microservice.Api.Domain.SeedWork.Paging;

namespace Todo.Api.Infra.Repositories
{
    public sealed class TodoRepository(
        ITodoDao todoDao,
        EnvironmentKey environmentKey,
        IDistributedMemoryCacheDao distributedMemoryCacheDao
        ) : ITodoRepository
    {
        private readonly ITodoDao _todoDao = todoDao;
        private readonly EnvironmentKey _environmentKey = environmentKey;
        private readonly IDistributedMemoryCacheDao _distributedMemoryCacheDao = distributedMemoryCacheDao;

        public async Task<Tuple<TodoModel?, ErrorResult>> DeleteTodoByIdAsync(Guid id)
        {
            var (result, error) = await _todoDao.DeleteTodoByIdAsync(id);

            if (result is null)
                return new(null, error);

            return new(result.ToDomain(), new());
        }

        public async Task<Tuple<TodoModel?, ErrorResult>> InsertTodoAsync(TodoModel todo)
        {
            var (result, error) = await _todoDao.InsertAsync(todo.ToDto());

            if (result is null)
                return new(null, error);

            return new(result.ToDomain(), new());
        }


        public async Task<Tuple<Search<TodoModel>?, ErrorResult>> SelectTodoByFilterAsync(Filter filter)
        {
            var serializedFilter = JsonConvert.SerializeObject(filter);
            var key = serializedFilter.ToBase64String();

            var isInCache = _distributedMemoryCacheDao.TryGetGroupValue<TodoModel>(key, out var cachedTodos) && cachedTodos.Count == 0;

            if (isInCache && cachedTodos is not null)
                return new(CreateSearchResult(cachedTodos, filter, key), new());

            var (searchTodos, searchError) = await _todoDao.SelectByFilterAsync(filter);

            if (searchTodos is null)
                return new(null, searchError);

            Search<TodoModel> searchTodo = new()
            {
                Paging = searchTodos.Paging,
                Data = searchTodos.Data.ToDomain()
            };

            for (int i = 0; i < searchTodo.Data.Count; i++)
            {
                var item = searchTodo.Data[i];

                if (item is null)
                    continue;

                _distributedMemoryCacheDao.SetValueToGroup(
                    $"{item.Id}",
                    item,
                    key,
                    TimeSpan.FromMinutes(_environmentKey.RedisInformation.CacheExpirationHours));

                if (i == 0)
                {
                    _distributedMemoryCacheDao.SetKeyExpire(
                        $"{Constant.APP_REDIS_CACHE_GROUP_SEARCH_BASE_NAME}{key}",
                        TimeSpan.FromMinutes(_environmentKey.RedisInformation.CacheExpirationHours));

                    _distributedMemoryCacheDao.SetValue(
                        $"{Constant.APP_REDIS_CACHE_GROUP_SEARCH_TOTAL_BASE_NAME}{key}",
                        searchTodo.Paging.Total.ToString(),
                        TimeSpan.FromMinutes(_environmentKey.RedisInformation.CacheExpirationHours));
                }
            }

            return new(searchTodo, new());
        }

        public async Task<Tuple<TodoModel?, ErrorResult>> UpdateTodoAsync(Guid id, TodoModel request)
        {
            var (updatedTodo, updateError) = await _todoDao.PutTodoAsync(id, request.ToDto());

            if (updatedTodo is null)
                return new(null, updateError);

            _distributedMemoryCacheDao.DeleteValue(id.ToString());

            _distributedMemoryCacheDao.SetValue(
                $"{updatedTodo.Id}",
                JsonConvert.SerializeObject(updatedTodo),
                                TimeSpan.FromHours(_environmentKey.RedisInformation.CacheExpirationHours));

            return new(updatedTodo.ToDomain(), new());
        }

        private Search<TodoModel> CreateSearchResult(List<TodoModel> todos, Filter filter, string key)
        {
            int cachedTotal = GetCachedTotal(key, todos.Count);
            return new Search<TodoModel>
            {
                Data = todos,
                Paging = new Paging
                {
                    PerPage = filter.Paging.PerPage,
                    Pages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(cachedTotal) / filter.Paging.PerPage)),
                    Total = cachedTotal,
                    CurrentPage = filter.Paging.Page
                }
            };
        }

        private int GetCachedTotal(string key, int defaultCount)
        {
            return _distributedMemoryCacheDao.TryGetValue(
                $"{Constant.APP_REDIS_CACHE_GROUP_SEARCH_TOTAL_BASE_NAME}{key}",
                out int totalRecords)
                ? totalRecords
                : defaultCount;
        }

    }
}
