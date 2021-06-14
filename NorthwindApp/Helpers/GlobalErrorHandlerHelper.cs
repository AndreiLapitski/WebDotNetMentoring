using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace NorthwindApp.Helpers
{
    public static class GlobalErrorHandlerHelper
    {
        public static void UseCustomErrorHandler(this IApplicationBuilder app)
        {
            app.Use(WriteResponse);
        }

        private static async Task WriteResponse(HttpContext httpContext, Func<Task> next)
        {
            IExceptionHandlerFeature exceptionDetails = httpContext.Features.Get<IExceptionHandlerFeature>();
            Exception exception = exceptionDetails?.Error;
            string logKey = httpContext.TraceIdentifier;
            string homeUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";

            Log.Error(exception, $"Log key: {logKey}");

            string handledExceptionTemplate = Path.Combine(Directory.GetCurrentDirectory(), Constants.ErrorTemplatePath);
            string template = await File.ReadAllTextAsync(handledExceptionTemplate);
            StringBuilder stringBuilder = new StringBuilder(template);
            stringBuilder.Replace("{{LogKey}}", logKey);
            stringBuilder.Replace("{{HomeUrl}}", homeUrl);

            httpContext.Response.ContentType = Constants.ContentTypeTextHtml;
            await httpContext.Response.WriteAsync(stringBuilder.ToString());
        }
    }
}
