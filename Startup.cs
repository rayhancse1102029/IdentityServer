using CARAPI.Data;
using CARAPI.Services.Car;
using IdentityModel.Client;
using IdentityServer4.Middleware;
using IdentityServer4.Services.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CARAPI
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

            services.AddScoped<ICarService, CarService>();
            services.AddControllers();

            #region Database Settings
            services.AddDbContext<CarApiDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DbConnection")));
            #endregion

            #region Authorization
            services.AddHttpContextAccessor();
            services.AddSingleton<ITokenService, TokenService>();
            //new on but working only for token type Bearer
            services.AddAuthentication("Bearer")
              .AddJwtBearer("Bearer", options =>
              {
                  options.Authority = Configuration["IdentityServer4:Url"];
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateAudience = false
                  };
              });

            //services.AddAuthentication()
            // .AddLocalApi("Bearer", option =>
            // {
            //     option.ExpectedScope = "identityServer4Scope";
            // });


            services.AddAuthorization(options =>
            {
                options.AddPolicy("Bearer", policy => 
                {
                    policy.AddAuthenticationSchemes("Bearer");
                    policy.RequireAuthenticatedUser();
                });
            });

            //.AddOpenIdConnect("oidc", options =>
            //{
            //    options.SignInScheme = "Cookies";

            //    options.Authority = "https://localhost:5000";
            //    options.RequireHttpsMetadata = false;

            //    options.ClientId = "rayhancse15@gmail.com";
            //    options.ClientSecret = "pass123#";
            //    options.ResponseType = "code id_token";

            //    options.SaveTokens = true;
            //    options.GetClaimsFromUserInfoEndpoint = true;

            //    options.Scope.Add("identityServer4Scope");
            //    //options.Scope.Add("offline_access");
            //    //options.ClaimActions.MapJsonKey("website", "website");
            //});

            services.AddHttpClient();
            #endregion

            #region Swagger
          
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Protected API", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(Configuration["IdentityServer4:Url"] + "/connect/authorize"),
                            Scopes = new Dictionary<string, string> { { "identityServer4Scope", "Customer Api for CarApi" } }
                        }
                    }
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>{ "identityServer4Scope" }
                    }
                });
            });
            // ================= OLD ===============
            //services.AddSwaggerGen(options =>
            //{
            //    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Protected API", Version = "v1" });

            //    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            //    {
            //        Type = SecuritySchemeType.OAuth2,
            //        Flows = new OpenApiOAuthFlows
            //        {
            //            AuthorizationCode = new OpenApiOAuthFlow
            //            {                            
            //                AuthorizationUrl = new Uri(Configuration["IdentityServer4:Url"] + "/connect/authorize"),
            //                TokenUrl = new Uri(Configuration["IdentityServer4:Url"] + "/connect/token"),
            //                Scopes = new Dictionary<string, string>
            //                {
            //                    {"identityServer4Scope", "Customer Api for CarApi"}
            //                }
            //            }
            //        }
            //    });

            //    options.OperationFilter<AuthorizeCheckOperationFilter>();
            //});
            #endregion

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.OAuthClientId("swagapi@gmail.com");
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApp Space Api V1");
                });
                // ============== OLD ============
                //app.UseSwaggerUI(c =>
                //{
                //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CARAPI v1");
                //    c.OAuthClientId("swagapi@gmail.com");
                //    c.OAuthClientSecret("pass123#");
                //    c.OAuthAppName("Swagger API");

                //});
            }

            //app.UseMiddleware<IdentityRequestMiddleware>();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public class AuthorizeCheckOperationFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var hasAuthorize = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                                   context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

                if (hasAuthorize)
                {
                    operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                    operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                    operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecurityScheme {Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "oauth2"}}]
                            = new[] { "identityServer4Scope" }
                    }
                };
                }
            }
        }

    }
}
