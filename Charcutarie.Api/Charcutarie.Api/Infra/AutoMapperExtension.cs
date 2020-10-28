using Charcutarie.Models.MappingProfile;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Charcutarie.Api.Infra
{
    public static class AutoMapperExtension
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            var mapperConfiguration = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfileConfiguration());
            });
            var mapper = mapperConfiguration.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}
