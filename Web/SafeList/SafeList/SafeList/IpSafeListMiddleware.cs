using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SafeList
{
    public class IpSafeListMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IpSafeListMiddleware> _logger;
        private HashSet<string> _allowedMethods = new HashSet<string>();
        private HashSet<string> _allowedIpAddresses = new HashSet<string>();
        public IpSafeListMiddleware(
            RequestDelegate next,
            ILogger<IpSafeListMiddleware> logger,
            IEnumerable<string> allowedIpAddresses,
            IEnumerable<string> allowedMethods)
        {
            _next = next;
            _logger = logger;
            _allowedIpAddresses = allowedIpAddresses.ToHashSet();
            _allowedMethods = allowedMethods.ToHashSet();
        }

        public async Task Invoke(HttpContext context)
        {
            if (!_allowedMethods.Contains(context.Request.Method))
            {
                var remoteIp = context.Connection.RemoteIpAddress.ToString();
                _logger.LogInformation("Request from Remote IP address: {RemoteIp}", remoteIp);
                

                if (!_allowedIpAddresses.Contains(remoteIp))
                {
                    _logger.LogWarning(
                        "Forbidden Request from Remote IP address: {RemoteIp}", remoteIp);
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return;
                }
            }

            await _next.Invoke(context);
        }
    }
}
