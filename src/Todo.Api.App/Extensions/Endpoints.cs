using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using Todo.Api.Domain.Aggregates.Todo;
using Todo.Api.Domain.Aggregates.Todo.Entities;
using Todo.Api.Domain.Aggregates.Todo.Entities.Filter;
using Todo.Api.Domain.SeedWork.ErrorResult;

namespace Todo.Api.App.Extensions
{
    public static class EndpointsExtensions
    {
        public static void AddEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/health", HealthCheck);
            endpoints.MapPost("/v1/todos", CreateTodo);
            endpoints.MapPut("/v1/todos/{id}", UpdateTodoById);
            endpoints.MapDelete("/v1/todos/{id}", DeleteTodoById);
            endpoints.MapGet("/v1/todos", GetTodosByFilter);
        }

        [SwaggerOperation(
            Summary = "Health Check",
            Description = "Verifica se a aplicação está operando corretamente.",
            OperationId = "HealthCheck",
            Tags = ["Health"]
        )]
        public static IResult HealthCheck() => Results.Ok("Healthy");

        [SwaggerOperation(
            Summary = "Cria um novo Todo",
            Description = "Cria um novo registro de Todo com os dados fornecidos.",
            OperationId = "CreateTodo",
            Tags = ["Todo"]
        )]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> CreateTodo([FromBody] TodoCreateRequest todo, [FromServices] ITodoService todoService)
        {
            var (result, error) = await todoService.InsertTodoAsync(todo);

            if (result is null || error.Error)
                return GenerateErrorResult(error);

            return Results.Created($"/v1/todos/{result.Id}", result);
        }

        [SwaggerOperation(
            Summary = "Atualiza um Todo para o estado de assinatura",
            Description = "Atualiza o estado do Todo identificado pelo ID, marcando-o como enviado para assinatura.",
            OperationId = "UpdateTodo",
            Tags = ["Todo"]
        )]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> UpdateTodoById([FromRoute] Guid id, [FromBody] TodoModel todo, [FromServices] ITodoService todoService)
        {
            var result = await todoService.UpdateTodoByIdAsync(id, todo);
            if (result.Item1 is null || result.Item2.Error)
                return GenerateErrorResult(result.Item2);

            return Results.Created($"/v1/todos/{result.Item1.Id}", result.Item1);
        }

        [SwaggerOperation(
            Summary = "Confirma a assinatura de um Todo",
            Description = "Atualiza o estado do Todo para assinado, identificado pelo ID.",
            OperationId = "ConfirmTodoSignature",
            Tags = ["Todo"]
        )]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> DeleteTodoById([FromRoute] Guid id, [FromServices] ITodoService todoService)
        {
            var result = await todoService.DeleteTodoByIdAsync(id);
            if (result.Item1 is null || result.Item2.Error)
                return GenerateErrorResult(result.Item2);

            return Results.Ok(result.Item1);
        }

        [SwaggerOperation(
            Summary = "Consulta Todos com base em filtros",
            Description = "Retorna uma lista de Todos com base nos filtros fornecidos.",
            OperationId = "GetTodosByFilter",
            Tags = ["Todo"]
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetTodosByFilter(
            [FromServices] ITodoService todoService,
            [FromQuery] Guid? id,
            [FromQuery] bool? isCompleted,
            [FromQuery] string? title,
            [FromQuery] int page = 1,
            [FromQuery] int perPage = 10)
        {
            var filter = new Filter()
            {
                Id = id,
                IsCompleted = isCompleted,
                Title = title,
                Paging = new()
                {
                    Page = page,
                    PerPage = perPage
                }
            };
            var result = await todoService.SelectTodoByFilterAsync(filter);
            if (result.Item2 is not null && result.Item2.Error)
                return GenerateErrorResult(result.Item2);

            return Results.Ok(result.Item1);
        }
        private static IResult GenerateErrorResult(ErrorResult errorResult) => errorResult.StatusCode switch
        {
            ErrorCode.Undefined => Results.Problem(JsonConvert.SerializeObject(errorResult), statusCode: 500),
            ErrorCode.NotFound => Results.NotFound(errorResult),
            ErrorCode.BadRequest => Results.BadRequest(errorResult),
            ErrorCode.Unauthorized => Results.Unauthorized(),
            ErrorCode.Forbidden => Results.Forbid(),
            ErrorCode.InternalServerError => Results.Problem(JsonConvert.SerializeObject(errorResult), statusCode: 500),
            ErrorCode.UnprocessableEntity => Results.UnprocessableEntity(errorResult),
            _ => Results.Problem(JsonConvert.SerializeObject(errorResult), statusCode: 422)
        };
    }
}
