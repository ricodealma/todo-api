using Todo.Api.Domain.SeedWork.ErrorResult;
using Todo.Api.Domain.SeedWork;
using Todo.Api.Infra.External;
using Todo.Api.Domain.Aggregates.Todo;
using Todo.Api.Domain.Aggregates.Todo.Entities;

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

        public async Task<Tuple<TodoModel?, ErrorResult>> InsertTodoAsync(TodoCreateRequest todo)
        {
            var (result, error) = await _todoDao.InsertAsync(todo.FromDomain());

            if (result is null)
                return new(null, error);

            return new(result.ToDomain(), new());
        }


        public async Task<Tuple<TodoModel?, ErrorResult>> SelectTodoByFilterAsync(Guid id)
        {
            var isInCache = _distributedMemoryCacheDao.TryGetValue<TodoModel>(id.ToString(), out var cachedTodos);

            if (isInCache && cachedTodos is not null)
                return new(cachedTodos, new());

            var (todoDto, error) = await _todoDao.SelectByIdAsync(id);

            if (todoDto is null)
                return new(null, error);


            _distributedMemoryCacheDao.SetValue(
                $"{todoDto.Id}",
                JsonConvert.SerializeObject(todoDto),
                                TimeSpan.FromHours(_environmentKey.RedisInformation.CacheExpirationHours));

            return new(todoDto.ToDomain(), new());
        }

        public async Task<Tuple<TodoModel?, ErrorResult>> UpdateTodoStatusAsync(Guid id, TodoStatusEnum request)
        {
            var (updatedTodo, updateError) = await _todoDao.PatchTodoStatusAsync(id, request);

            if (updatedTodo is null)
                return new(null, updateError);

            _distributedMemoryCacheDao.DeleteValue(id.ToString());

            _distributedMemoryCacheDao.SetValue(
                $"{updatedTodo.Id}",
                JsonConvert.SerializeObject(updatedTodo),
                                TimeSpan.FromHours(_environmentKey.RedisInformation.CacheExpirationHours));

            return new(updatedTodo.ToDomain(), new());
        }
    }
}
