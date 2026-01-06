using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiCatalago.Filters
{
    public class APILoggingFilter:IActionFilter
    {
        private readonly ILogger<APILoggingFilter> _logger;

        public APILoggingFilter(ILogger<APILoggingFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation($"Log teste inicial on action executing");

            _logger.LogInformation($"__________________________________________________________");

            _logger.LogInformation($"{DateTime.UtcNow.ToString()}");

            _logger.LogInformation($"Model State: {context.ModelState.IsValid}");

            _logger.LogInformation($"Http context path: {context.HttpContext.Request.Path.Value}");

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation($"Log teste inicial on action executed");

            _logger.LogInformation($"__________________________________________________________");

            _logger.LogInformation($"{DateTime.UtcNow.ToString()}");

            _logger.LogInformation($"Model State: {context.ModelState.IsValid}");

            _logger.LogInformation($"Http context status: {context.HttpContext.Response.StatusCode}");


        }

        
    }
}
