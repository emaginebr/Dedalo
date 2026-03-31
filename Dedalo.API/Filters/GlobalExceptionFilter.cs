using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Dedalo.API.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is UnauthorizedAccessException)
            {
                context.Result = new ForbidResult();
                context.ExceptionHandled = true;
                return;
            }

            if (context.Exception is ValidationException validationException)
            {
                var errors = validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                context.Result = new BadRequestObjectResult(new
                {
                    title = "Validation failed",
                    status = 400,
                    errors
                });
                context.ExceptionHandled = true;
                return;
            }

            _logger.LogError(context.Exception, "Unhandled exception on {Method} {Path}",
                context.HttpContext.Request.Method,
                context.HttpContext.Request.Path);

            context.Result = new ObjectResult(context.Exception.Message)
            {
                StatusCode = 500
            };
            context.ExceptionHandled = true;
        }
    }
}
