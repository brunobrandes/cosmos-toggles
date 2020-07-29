using AutoMapper;
using Cosmos.Toggles.Infra.DependencyInjection;
using Cosmos.Toggles.Infra.Http.Filters;
using Cosmos.Toggles.Infra.Http.Middlewares;
using Cosmos.Toggles.Infra.Mapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Text;
using System.Text.Json;

namespace Cosmos.Toggles.Ui.Api
{
    /// <summary>
    /// Startup web api
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200/",
                            "https://cosmostoggles.net/");
                    });
            });

            services
                .AddControllers(options => options.Filters.Add<NotificationFilter>()).AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            services
                .AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cosmos Toggles .api", Version = "v1.0.0-preview" });
            });

            services.AddAutoMapper(typeof(UserMapping));
            services.AddCosmosToggleDomainServices();
            services.AddCosmosToggleAppServices();
            services.AddCosmosToggleDTOValidators();
            services.AddCosmosToggleNotificationContext();
            services.AddCosmosToggleDataContext(Configuration.GetConnectionString("CosmosToggleConnection"));

            services
                .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    // TODO: Using azure key vault
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("eaa1e32b-8a8e-45ac-bfc5-e6a62078c2e5")),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services
              .AddAuthorization(auth =>
              {
                  auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                      .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                      .RequireAuthenticatedUser().Build());
              });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
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

            app.UseCors(x => x
              .SetIsOriginAllowed(origin => true)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseMiddleware(typeof(ExceptionHandlingMiddleware));
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
