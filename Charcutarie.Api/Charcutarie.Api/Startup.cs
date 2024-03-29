using Charcutarie.Api.Infra;
using Charcutarie.Repository.DbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using Charcutarie.Api.Validators;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Charcutarie.Api.Infra.Middlewares;
using Charcutarie.Core.SMTP;
using System;
using Microsoft.Extensions.Caching.Memory;

namespace Charcutarie.Api
{
    public class Startup
    {
        public static readonly LoggerFactory DbCommandDebugLoggerFactory = new LoggerFactory(new[] { new DebugLoggerProvider() });

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddDbContext<CharcuterieDbContext>(options =>
            {
                options.UseLoggerFactory(DbCommandDebugLoggerFactory);
                options.EnableSensitiveDataLogging(true);
                options.UseSqlServer(Configuration.GetConnectionString("CharcutarieDB"));
            }, ServiceLifetime.Transient);
            services.AddMvc().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            }).AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ClientRegistrationValidator>());
            services.AddCors(options =>
            {
                options.AddPolicy("CBM", p =>
                {
                    p.AllowAnyOrigin();
                    p.AllowAnyMethod();
                    p.AllowAnyHeader();
                });
            });
            services.ConfigureJWT(Configuration);
            services.AddHttpContextAccessor();
            services.AddDIServices();
            services.AddAutoMapper();
            services.AddApiVersioning();
            services.AddControllers();
            services.AddSwagger();

            services.AddOptions<SMTPSettings>().Bind(Configuration.GetSection("Smtp"));

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCBMErrorLogging();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"../swagger/v1/swagger.json", $"v1");
            });
            app.UseCors("CBM");
            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
