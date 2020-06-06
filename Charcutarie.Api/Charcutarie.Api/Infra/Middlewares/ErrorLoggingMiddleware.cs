using Charcutarie.Core.ExceptionHandling;
using Charcutarie.Core.Extensions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charcutarie.Api.Infra.Middlewares
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorLoggingMiddleware> logger;

        public ErrorLoggingMiddleware(RequestDelegate next, ILogger<ErrorLoggingMiddleware> logger)
        {
            _next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == "OPTIONS")
            {
                await _next(context);
                return;
            }
            var request = await FormatRequest(context.Request);
            var originalBodyStream = context.Response.Body;
            var errors = new List<string>();
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                try
                {
                    await _next(context);
                }
                catch (BusinessException ex)
                {
                    context.Response.StatusCode = 402;
                    errors.AddRange(ex.Errors);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.ToString());
                    throw;
                }
                finally
                {
                    var validationErrors = await FormatResponse(context.Response);
                    errors.AddRange(validationErrors);
                    if (context.Response.StatusCode == 400 || context.Response.StatusCode == 402)
                    {
                        context.Response.Clear();
                        context.Response.StatusCode = 400;
                        context.Response.ContentType = "application/json";
                        var bytes = Encoding.Default.GetBytes(JsonConvert.SerializeObject(errors));
                        await originalBodyStream.WriteAsync(bytes, 0, bytes.Length);
                    }
                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
        }


        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();
            var body = request.Body;
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            body.Position = 0;
            request.Body = body;

            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }

        private async Task<List<string>> FormatResponse(HttpResponse response)
        {
            var errors = new List<string>();
            response.Body.Seek(0, SeekOrigin.Begin);
            string text = await new StreamReader(response.Body).ReadToEndAsync();
            if (response.StatusCode == 400)
            {
                errors = GetErrors(text);
            }
            response.Body.Seek(0, SeekOrigin.Begin);
            return errors;
        }
        private List<string> GetErrors(string json)
        {
            var result = new List<string>();
            var obj = JObject.Parse(json);
            var errors = obj.SelectToken("errors");
            var pErrors = errors.Children();
            foreach (var e in pErrors)
            {
                var error = e.Children();
                var parsedErrors = error.SelectMany(x => x.Select(y => y.ToString())).ToList();
                result.AddRange(parsedErrors);
            }
            return result;
        }
    }
}
