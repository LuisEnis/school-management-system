using SchoolManagement.API.Exceptions;
using SchoolManagement.API.Responses;
using System.Net;
using System.Text.Json;

namespace SchoolManagement.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;


        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    ex.Message);

                await HandleExceptionAsync(
                    context,
                    ex);
            }
        }


        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            var statusCode = exception switch
            {
                NotFoundException =>
                    HttpStatusCode.NotFound,

                BadRequestException =>
                    HttpStatusCode.BadRequest,

                ConflictException =>
                    HttpStatusCode.Conflict,

                _ =>
                    HttpStatusCode.InternalServerError
            };


            var response = new ErrorResponse
            {
                StatusCode = (int)statusCode,
                Message = exception.Message
            };


            context.Response.ContentType =
                "application/json";

            context.Response.StatusCode =
                (int)statusCode;


            var json =
                JsonSerializer.Serialize(response);


            await context.Response.WriteAsync(json);
        }
    }
}
