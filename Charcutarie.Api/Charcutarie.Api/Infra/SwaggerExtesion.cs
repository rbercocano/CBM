using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Linq;

namespace Charcutarie.Api.Infra
{
    public static class SwaggerExtesion
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "v1 API",
                    Description = "Charcutarie Business Manager API",
                });
                options.OperationFilter<RemoveVersionFromParameter>();
                options.DocumentFilter<ReplaceVersionWithExactValueInPath>();
                options.DocInclusionPredicate((version, desc) =>
                {
                    var versions = desc.ActionDescriptor.EndpointMetadata.OfType<ApiVersionAttribute>()
                            .SelectMany(attr => attr.Versions);
                    return versions.Any(v => $"v{v}" == version);
                });
            });
        }
    }
}
