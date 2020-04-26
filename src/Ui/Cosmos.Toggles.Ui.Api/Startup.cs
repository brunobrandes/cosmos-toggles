using AutoMapper;
using Cosmos.Toggles.Infra.DependencyInjection;
using Cosmos.Toggles.Infra.Http.Filters;
using Cosmos.Toggles.Infra.Http.Middlewares;
using Cosmos.Toggles.Infra.Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Cosmos.Toggles.Ui.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers(options => options.Filters.Add<NotificationFilter>());

            services
                .AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cosmos Toggles .api", Version = "v1.0.0-preview" });
            });

            services.AddAutoMapper(typeof(FlagMapping));
            services.AddCosmosToggleAppServices();
            services.AddCosmosToggleDTOValidators();
            services.AddCosmosToggleNotificationContext();
            services.AddCosmosToggleDataContext(Configuration.GetConnectionString("CosmosToggleConnection"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cosmos Toggle .api v1.0.0-preview");
            });

            app.UseRouting();

            app.UseAuthorization();
            app.UseMiddleware(typeof(ExceptionHandlingMiddleware));
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
