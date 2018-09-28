using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace idsrv.api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        ILogger<ExceptionContext> _logger;
        
        public ExceptionFilter(ILogger<ExceptionContext> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            HandleException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            var response = JsonConvert.SerializeObject(new { Error = context.Exception.Message });

            context.ExceptionHandled = true;
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.WriteAsync(response, Encoding.UTF8);

            LogException(context);
        }
        private void LogException(ExceptionContext context)
        {
            if (_logger != null)
                _logger.LogError(context.Exception, $"Error while executing {context.HttpContext.Request.Path}");
        }
    }
}
