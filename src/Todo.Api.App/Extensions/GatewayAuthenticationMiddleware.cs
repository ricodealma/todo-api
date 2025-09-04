using Todo.Api.Domain.SeedWork;

namespace Todo.Api.App.Extensions
{
    public sealed class GatewayAuthenticationMiddleware(RequestDelegate requestDelegate, EnvironmentKey environmentKey)
    {
        private readonly RequestDelegate _requestDelegate = requestDelegate;
        private readonly EnvironmentKey _environmentKey = environmentKey;
        private readonly List<string> _routesWithoutAuthentication = ["/health"];

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (!_routesWithoutAuthentication.Contains(httpContext.Request.Path))
            {
                if (httpContext.Request.Headers[Constant.APP_REQUEST_HEADER_KEY] == _environmentKey.AppInformation.HeaderKey)
                {
                    await _requestDelegate(httpContext);
                    return;
                }

                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

                return;
            }
            await _requestDelegate(httpContext);

        }
    }
}
