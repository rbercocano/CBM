using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charcutarie.Api.Infra.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static void UseCBMErrorLogging(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorLoggingMiddleware>();
        }
    }
}
