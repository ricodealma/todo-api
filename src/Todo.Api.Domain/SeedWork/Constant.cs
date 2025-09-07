namespace Todo.Api.Domain.SeedWork;
public sealed class Constant
{
    public const string APP_ENV = "APP_ENV";
    public const string APP_ENV_DEV = "DEV";
    public const string APP_ENV_QA = "QA";
    public const string APP_ENV_PROD = "PROD";
    public const string APP_REQUEST_HEADER_KEY = "X-Api-Header";
    public const string HEADER_TOKEN = "X_API_HEADER";

    public const string APP_FILTER_SORT_CRITERIA_ASC = "asc";
    public const string APP_FILTER_SORT_CRITERIA_DESC = "desc";
    public const string APP_FILTER_SORT_DATE = "date";

    public const string APP_REDIS_CACHE_ENTITY_BASE_NAME = "todo-api:";
    public const string APP_REDIS_CACHE_GROUP_SEARCH_BASE_NAME = "todo-api:grouped-search:";

    public const string APP_REDIS_CACHE_GROUP_SEARCH_MATCH = "MATCH";
    public const string APP_REDIS_CACHE_GROUP_SEARCH_SCAN = "SCAN";
    public const string APP_REDIS_CACHE_GROUP_SEARCH_COUNT = "COUNT";

    public const string APP_REDIS_CACHE_GROUP_SEARCH_TOTAL_BASE_NAME = "grouped-search:total:";

    public const string SQL_SERVER = "SQL_SERVER";
    public const string SQL_USER = "SQL_USER";
    public const string SQL_PASSWORD = "SQL_PASSWORD";
    public const string SQL_DATABASE = "SQL_DATABASE";


    public const string REDIS_SERVER = "REDIS_SERVER";
    public const string REDIS_USER = "REDIS_USER";
    public const string REDIS_PASSWORD = "REDIS_PASSWORD";

    public const string REDIS_CACHE_ENTITY_EXPIRATION_HOURS = "REDIS_CACHE_ENTITY_EXPIRATION_HOURS";
}
