using AuthenticationService.Application;
using AuthenticationService.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuthenticationService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment CurrentEnviroment;

        public Startup(IConfiguration configuration,
            IWebHostEnvironment currentEnviroment)
        {
            Configuration = configuration;
            CurrentEnviroment = currentEnviroment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication();
            services.AddAuthorization();
            services.ConfigureCors();
            services.ConfigureManagers();
            services.ConfigureIdentity();
            services.ConfigureSqlContext(Configuration, CurrentEnviroment);
            services.AddAuthenticationServiceApplication();
            services.ConfigureSwagger();
            services.AddControllers(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.ConfigureExceptionHandler();
            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Authentication Service API");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
