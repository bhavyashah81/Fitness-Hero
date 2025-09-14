using System.Diagnostics;
using System.Text;

namespace FitnessAppAPI.Middleware
{
    /// <summary>
    /// Middleware for logging HTTP requests and responses
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        private readonly RequestLoggingOptions _options;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger, RequestLoggingOptions? options = null)
        {
            _next = next;
            _logger = logger;
            _options = options ?? new RequestLoggingOptions();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestId = Guid.NewGuid().ToString("N")[..8];
            
            // Add request ID to context
            context.Items["RequestId"] = requestId;

            if (_options.LogRequests)
            {
                await LogRequestAsync(context, requestId);
            }

            // Capture original response body stream
            var originalResponseBodyStream = context.Response.Body;

            try
            {
                if (_options.LogResponses)
                {
                    using var responseBody = new MemoryStream();
                    context.Response.Body = responseBody;

                    await _next(context);

                    stopwatch.Stop();
                    await LogResponseAsync(context, requestId, stopwatch.ElapsedMilliseconds);

                    // Copy response back to original stream
                    await responseBody.CopyToAsync(originalResponseBodyStream);
                }
                else
                {
                    await _next(context);
                    stopwatch.Stop();
                    
                    _logger.LogInformation(
                        "Request {RequestId} completed in {ElapsedMs}ms - {Method} {Path} -> {StatusCode}",
                        requestId,
                        stopwatch.ElapsedMilliseconds,
                        context.Request.Method,
                        context.Request.Path,
                        context.Response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex,
                    "Request {RequestId} failed after {ElapsedMs}ms - {Method} {Path}",
                    requestId,
                    stopwatch.ElapsedMilliseconds,
                    context.Request.Method,
                    context.Request.Path);
                
                throw;
            }
            finally
            {
                context.Response.Body = originalResponseBodyStream;
            }
        }

        private async Task LogRequestAsync(HttpContext context, string requestId)
        {
            var request = context.Request;
            var logBuilder = new StringBuilder();

            logBuilder.AppendLine($"Request {requestId} started:");
            logBuilder.AppendLine($"  Method: {request.Method}");
            logBuilder.AppendLine($"  Path: {request.Path}");
            logBuilder.AppendLine($"  QueryString: {request.QueryString}");
            logBuilder.AppendLine($"  ContentType: {request.ContentType}");
            logBuilder.AppendLine($"  ContentLength: {request.ContentLength}");
            logBuilder.AppendLine($"  UserAgent: {request.Headers.UserAgent}");
            logBuilder.AppendLine($"  RemoteIP: {GetClientIpAddress(context)}");

            if (_options.LogHeaders && request.Headers.Count > 0)
            {
                logBuilder.AppendLine("  Headers:");
                foreach (var header in request.Headers.Where(h => !_options.SensitiveHeaders.Contains(h.Key, StringComparer.OrdinalIgnoreCase)))
                {
                    logBuilder.AppendLine($"    {header.Key}: {string.Join(", ", header.Value)}");
                }
            }

            if (_options.LogRequestBody && request.ContentLength > 0 && request.ContentLength < _options.MaxBodyLogSize)
            {
                request.EnableBuffering();
                var buffer = new byte[request.ContentLength.Value];
                await request.Body.ReadAsync(buffer, 0, buffer.Length);
                request.Body.Position = 0;

                var bodyContent = Encoding.UTF8.GetString(buffer);
                if (!string.IsNullOrWhiteSpace(bodyContent))
                {
                    logBuilder.AppendLine($"  Body: {bodyContent}");
                }
            }

            _logger.LogInformation(logBuilder.ToString());
        }

        private async Task LogResponseAsync(HttpContext context, string requestId, long elapsedMs)
        {
            var response = context.Response;
            var logBuilder = new StringBuilder();

            logBuilder.AppendLine($"Request {requestId} completed in {elapsedMs}ms:");
            logBuilder.AppendLine($"  StatusCode: {response.StatusCode}");
            logBuilder.AppendLine($"  ContentType: {response.ContentType}");
            logBuilder.AppendLine($"  ContentLength: {response.ContentLength}");

            if (_options.LogHeaders && response.Headers.Count > 0)
            {
                logBuilder.AppendLine("  Headers:");
                foreach (var header in response.Headers.Where(h => !_options.SensitiveHeaders.Contains(h.Key, StringComparer.OrdinalIgnoreCase)))
                {
                    logBuilder.AppendLine($"    {header.Key}: {string.Join(", ", header.Value)}");
                }
            }

            if (_options.LogResponseBody && response.Body.CanRead && response.Body.Length > 0 && response.Body.Length < _options.MaxBodyLogSize)
            {
                response.Body.Position = 0;
                var buffer = new byte[response.Body.Length];
                await response.Body.ReadAsync(buffer, 0, buffer.Length);
                response.Body.Position = 0;

                var bodyContent = Encoding.UTF8.GetString(buffer);
                if (!string.IsNullOrWhiteSpace(bodyContent))
                {
                    logBuilder.AppendLine($"  Body: {bodyContent}");
                }
            }

            var logLevel = response.StatusCode >= 400 ? LogLevel.Warning : LogLevel.Information;
            _logger.Log(logLevel, logBuilder.ToString());
        }

        private static string GetClientIpAddress(HttpContext context)
        {
            // Check for forwarded IP first (in case of proxy/load balancer)
            var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                return forwardedFor.Split(',')[0].Trim();
            }

            var realIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(realIp))
            {
                return realIp;
            }

            return context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }
    }

    /// <summary>
    /// Configuration options for request logging middleware
    /// </summary>
    public class RequestLoggingOptions
    {
        /// <summary>
        /// Whether to log incoming requests
        /// </summary>
        public bool LogRequests { get; set; } = true;

        /// <summary>
        /// Whether to log outgoing responses
        /// </summary>
        public bool LogResponses { get; set; } = true;

        /// <summary>
        /// Whether to log request/response headers
        /// </summary>
        public bool LogHeaders { get; set; } = false;

        /// <summary>
        /// Whether to log request body
        /// </summary>
        public bool LogRequestBody { get; set; } = false;

        /// <summary>
        /// Whether to log response body
        /// </summary>
        public bool LogResponseBody { get; set; } = false;

        /// <summary>
        /// Maximum size of body to log (in bytes)
        /// </summary>
        public long MaxBodyLogSize { get; set; } = 4096; // 4KB

        /// <summary>
        /// Headers that should not be logged (for security)
        /// </summary>
        public HashSet<string> SensitiveHeaders { get; set; } = new(StringComparer.OrdinalIgnoreCase)
        {
            "Authorization",
            "Cookie",
            "Set-Cookie",
            "X-API-Key",
            "X-Auth-Token",
            "Authentication",
            "Proxy-Authorization",
            "X-Forwarded-Authorization"
        };

        /// <summary>
        /// Paths that should be excluded from logging
        /// </summary>
        public HashSet<string> ExcludedPaths { get; set; } = new(StringComparer.OrdinalIgnoreCase)
        {
            "/health",
            "/healthcheck",
            "/ping",
            "/metrics",
            "/favicon.ico"
        };

        /// <summary>
        /// Whether to exclude the specified paths from logging
        /// </summary>
        public bool UseExcludedPaths { get; set; } = true;
    }

    /// <summary>
    /// Extension methods for registering request logging middleware
    /// </summary>
    public static class RequestLoggingMiddlewareExtensions
    {
        /// <summary>
        /// Adds request logging middleware to the application pipeline
        /// </summary>
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder, RequestLoggingOptions? options = null)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>(options ?? new RequestLoggingOptions());
        }

        /// <summary>
        /// Adds request logging middleware to the application pipeline with configuration
        /// </summary>
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder, Action<RequestLoggingOptions> configureOptions)
        {
            var options = new RequestLoggingOptions();
            configureOptions(options);
            return builder.UseMiddleware<RequestLoggingMiddleware>(options);
        }
    }

    /// <summary>
    /// Performance metrics for request processing
    /// </summary>
    public class RequestMetrics
    {
        public string RequestId { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public long ElapsedMilliseconds { get; set; }
        public DateTime Timestamp { get; set; }
        public long RequestSize { get; set; }
        public long ResponseSize { get; set; }
        public string UserAgent { get; set; } = string.Empty;
        public string ClientIp { get; set; } = string.Empty;

        /// <summary>
        /// Categorizes the request performance
        /// </summary>
        public PerformanceCategory GetPerformanceCategory()
        {
            return ElapsedMilliseconds switch
            {
                < 100 => PerformanceCategory.Fast,
                < 500 => PerformanceCategory.Normal,
                < 1000 => PerformanceCategory.Slow,
                _ => PerformanceCategory.VerySlow
            };
        }

        /// <summary>
        /// Determines if the request was successful
        /// </summary>
        public bool IsSuccessful => StatusCode >= 200 && StatusCode < 400;

        /// <summary>
        /// Determines if the request was a client error
        /// </summary>
        public bool IsClientError => StatusCode >= 400 && StatusCode < 500;

        /// <summary>
        /// Determines if the request was a server error
        /// </summary>
        public bool IsServerError => StatusCode >= 500;
    }

    /// <summary>
    /// Performance category enumeration
    /// </summary>
    public enum PerformanceCategory
    {
        Fast,
        Normal,
        Slow,
        VerySlow
    }
}
