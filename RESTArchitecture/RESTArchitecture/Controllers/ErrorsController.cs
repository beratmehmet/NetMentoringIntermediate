using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTArchitecture.Models;

namespace RESTArchitecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ErrorsController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public ErrorsController(IWebHostEnvironment environment)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        [Route("/errors")]
        protected IActionResult Error()
        {
            var error = HttpContext
                .Features
                .Get<IExceptionHandlerFeature>()
                ?.Error;

            return error switch
            {
                ResourceNotFoundException resourceNotFoundException => HandleResourceNotFoundException(resourceNotFoundException),
                _ => HandleUnknownException(error),
            };
        }

        private IActionResult HandleResourceNotFoundException(ResourceNotFoundException resourceNotFoundException)
        {
            var problemDetails = new ProblemDetails()
            {
                Detail = resourceNotFoundException.Message,
                Instance = string.Empty,
                Status = StatusCodes.Status404NotFound,
                Title = "A resource was not found",
                Type = $"https://httpstatuses.com/{StatusCodes.Status404NotFound}",
            };
            problemDetails.Extensions.Add("traceId", HttpContext.TraceIdentifier);
            HttpContext.Response.StatusCode = 404;
            return new ObjectResult(problemDetails);
        }

        private IActionResult HandleUnknownException(Exception? exception)
        {
            if (exception == null)
                return NoContent();

            var problemDetails = new ProblemDetails()
            {
                Instance = string.Empty,
                Status = StatusCodes.Status500InternalServerError,
                Type = $"https://httpstatuses.com/{StatusCodes.Status500InternalServerError}"
            };

            if (!_environment.IsDevelopment())
            {
                problemDetails.Detail = exception.StackTrace;
                problemDetails.Title = exception.Message;
            }
            else
            {
                problemDetails.Detail = string.Empty;
                problemDetails.Title = "An unexpected server fault occurred";
            }

            problemDetails.Extensions.Add("traceId", HttpContext.TraceIdentifier);

            HttpContext.Response.StatusCode = 500;
            return new ObjectResult(problemDetails);
        }
    }
}
