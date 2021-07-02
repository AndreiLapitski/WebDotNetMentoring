using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NorthwindApp.Helpers;
using Serilog;

namespace NorthwindApp.Filters
{
    public class LogFilter : IActionFilter
    {
        private readonly IConfiguration _configuration;

        public LogFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (_configuration.GetValue<bool>(Constants.LoggingActionsEnabledKey))
            {
                GetControllerAndActionNames(
                    (ControllerBase)context.Controller,
                    out string controllerName,
                    out string actionName);

                Log.Information($"Start controller name: {controllerName}, action name: {actionName}");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (_configuration.GetValue<bool>(Constants.LoggingActionsEnabledKey))
            {
                GetControllerAndActionNames(
                    (ControllerBase)context.Controller,
                    out string controllerName,
                    out string actionName);

                Log.Information($"End controller name: {controllerName}, action name: {actionName}");
            }
        }

        private void GetControllerAndActionNames(
            ControllerBase baseController,
            out string controllerName,
            out string actionName)
        {
            controllerName = baseController.ControllerContext.ActionDescriptor.ControllerName;
            actionName = baseController.ControllerContext.ActionDescriptor.ActionName;
        }
    }
}
