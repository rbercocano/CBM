using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Charcutarie.Core.Extensions
{
    public static class HttpResponseExtensions
    {
        public static async Task WriteError(this HttpResponse response, int statusCode, List<string> errors)
        {
            response.StatusCode = statusCode;
            response.ContentType = "application/json";
            await response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(errors));
        }
    }
}
