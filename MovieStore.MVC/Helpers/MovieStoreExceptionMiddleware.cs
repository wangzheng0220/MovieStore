using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MovieStore.MVC.Helpers
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    // Always make sure you have Exceptionmiddlwares register at the very beginning
    public class MovieStoreExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        // In ASP.NET Core logging is builtin to the framework
        private readonly ILogger<MovieStoreExceptionMiddleware> _logger;

        public MovieStoreExceptionMiddleware(RequestDelegate next, ILogger<MovieStoreExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                _logger.LogInformation("MovieStoreExceptionMiddleware is called");
                await _next(httpContext); //call next middleware
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Happened: {ex}"); //catch the exception
                await HandleException(httpContext, ex); //call in seperate method
            }

            // return _next(httpContext);
        }

        public async Task HandleException(HttpContext context, Exception exception)
        {
            // Step 1: You need to log the exception details such as
            // Most Popular Logging frameworks in .NET are
            // Serilog (Use this one), NLog and Log4net (nuget)
            // 1. Exception message
            // 2. Exception StackTrace
            // 3. When the exception happned. Datetime
            // 4. The User Info
            // 5. Where in our code the exception happened
            _logger.LogInformation("-----------START OF Logging--------------");
            _logger.LogError($"Exception Message: {exception.Message}");
            _logger.LogError($"Exception Stack Trace: {exception.StackTrace}"); //give all the info where exception at
            _logger.LogInformation($"Exception for User: {context.User.Identity.Name}");
            _logger.LogInformation($"Exception happened on {DateTime.UtcNow}");
            _logger.LogInformation("-----------END OF Logging--------------");

            // 500
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            // Step 2: Send Notification(Email preferred) to the Dev team ex: dev@antra.com
            // MailKit(free) --send emails, SendGrid(paid, but free for some time)

            // Step 3: Show a friendly error page to the User
            context.Response.Redirect("/Home/Error");
            await Task.CompletedTask;

        }

    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MovieStoreExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseMovieStoreExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MovieStoreExceptionMiddleware>();
        }
    }
}
