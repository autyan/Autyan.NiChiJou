using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Mvc.Extension;
using Autyan.NiChiJou.Core.Mvc.Models;
using Autyan.NiChiJou.Core.Mvc.Options;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace Autyan.NiChiJou.Core.Mvc.Middleware
{
    public class UnifyExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly Func<object, Task> _clearCacheHeadersDelegate;
        private readonly DiagnosticSource _diagnosticSource;
        private readonly IMemoryCache _cache;

        public UnifyExceptionHandlerMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            DiagnosticSource diagnosticSource,
            IMemoryCache cache)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<UnifyExceptionHandlerMiddleware>();
            _clearCacheHeadersDelegate = ClearCacheHeadersAsync;
            _diagnosticSource = diagnosticSource;
            _cache = cache;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var requestId = Guid.NewGuid().ToString().ToLower(CultureInfo.CurrentCulture);
                var userId = context.User?.Claims.FirstOrDefault(c => c.Type == "MemberCode")?.Value;
                _logger.LogError(0, ex, "An unhandled exception has occurred: " +
                                        $"RequestId => {requestId}, RequestUrl => {context.Request.GetDisplayUrl()}, " +
                                        $"RequestUser => { userId ?? "Anonymous" }, " +
                                        $"RemoteIpAddress => { context.Connection.RemoteIpAddress}\r\n" +
                                        $"Exceptions => {ex}");
                // We can't do anything if the response has already started, just abort.
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the error handler will not be executed.");
                    throw;
                }

                var originalPath = context.Request.Path;
                try
                {
                    context.Response.Clear();
                    context.Response.StatusCode = 500;
                    context.Response.OnStarting(_clearCacheHeadersDelegate, context.Response);

                    if (_diagnosticSource.IsEnabled("Microsoft.AspNetCore.Diagnostics.HandledException"))
                    {
                        _diagnosticSource.Write("Microsoft.AspNetCore.Diagnostics.HandledException", new { httpContext = context, exception = ex });
                    }
                    if (context.Request.IsAjaxRequest())
                    {
                        var jsonResponse = new JsonResponse
                        {
                            RequestId = requestId
                        };
                        jsonResponse.Messages.Add("Request Failed");

#if DEBUG
                        jsonResponse.Exception = context.Features.Get<IExceptionHandlerFeature>().Error;
#endif
                        context.Response.ContentType = "application/json";
                        using (var writer = new StreamWriter(context.Response.Body))
                        {
                            new JsonSerializer().Serialize(writer, jsonResponse);
                            await writer.FlushAsync().ConfigureAwait(false);
                        }

                        return;
                    }

                    // Do nothing if a response body has already been provided.
                    if (context.Response.HasStarted
                        || context.Response.StatusCode < 400
                        || context.Response.StatusCode >= 600
                        || context.Response.ContentLength.HasValue
                        || !string.IsNullOrEmpty(context.Response.ContentType))
                    {
                        return;
                    }

                    // If not a ajax request, redirect to error pages.
                    var options = new UnhandledExceptionOptions
                    {
                        RequestId = requestId
                    };

#if DEBUG
                    options.Exception = ex;
#endif
                    _cache.Set(requestId, options, TimeSpan.FromMinutes(5));
                    var location = string.Format(CultureInfo.InvariantCulture, "/Errors/Error{0}?requestId={1}", context.Response.StatusCode, requestId);
                    context.Response.Redirect(location);
                    return;
                }
                catch (Exception ex2)
                {
                    // Suppress secondary exceptions, re-throw the original.
                    _logger.LogError(0, ex2, "An exception was thrown attempting to execute the error handler.");
                }
                finally
                {
                    context.Request.Path = originalPath;
                }
                throw; // Re-throw the original if we couldn't handle it
            }
        }

        private static Task ClearCacheHeadersAsync(object state)
        {
            var response = (HttpResponse)state;
            response.Headers[HeaderNames.CacheControl] = "no-cache";
            response.Headers[HeaderNames.Pragma] = "no-cache";
            response.Headers[HeaderNames.Expires] = "-1";
            response.Headers.Remove(HeaderNames.ETag);
            return Task.CompletedTask;
        }
    }
}
