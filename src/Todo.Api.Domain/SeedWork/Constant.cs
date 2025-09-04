namespace Todo.Api.Domain.SeedWork;
public sealed class Constant
{
    public const string APP_ENV = "APP_ENV";
    public const string APP_ENV_DEV = "DEV";
    public const string APP_ENV_QA = "QA";
    public const string APP_ENV_PROD = "PROD";
    public const string APP_REQUEST_HEADER_KEY = "X-Api-Header";
    public const string AWS_SECRET_MANAGER_HEADER_TOKEN = "x-api-header";

    public const string APP_FILTER_SORT_CRITERIA_ASC = "asc";
    public const string APP_FILTER_SORT_CRITERIA_DESC = "desc";
    public const string APP_FILTER_SORT_DATE = "date";

    public const string APP_REDIS_CACHE_ENTITY_BASE_NAME = "todo-api:";
    public const string APP_REDIS_CACHE_GROUP_SEARCH_BASE_NAME = "todo-api:grouped-search:";

    public const string APP_REDIS_CACHE_GROUP_SEARCH_MATCH = "MATCH";
    public const string APP_REDIS_CACHE_GROUP_SEARCH_SCAN = "SCAN";
    public const string APP_REDIS_CACHE_GROUP_SEARCH_COUNT = "COUNT";

    public const string APP_REDIS_CACHE_GROUP_SEARCH_TOTAL_BASE_NAME = "grouped-search:total:";

    public const string SQL_SERVER = "sql-server";
    public const string SQL_USER = "sql-user";
    public const string SQL_PASSWORD = "sql-password";
    public const string SQL_DATABASE = "sql-database";


    public const string REDIS_SERVER = "redis-server";
    public const string REDIS_USER = "redis-user";
    public const string REDIS_PASSWORD = "redis-password";

    public const string REDIS_CACHE_ENTITY_EXPIRATION_HOURS = "REDIS_CACHE_ENTITY_EXPIRATION_HOURS";
}
