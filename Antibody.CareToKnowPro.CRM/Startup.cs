using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Reflection;
using System.Threading.Tasks;
using Antibody.CareToKnowPro.CRM.Configuration;
using Antibody.CareToKnowPro.CRM.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using FluentValidation;
using MediatR;

using Antibody.CareToKnowPro.CRM.Filters;
using Antibody.CareToKnowPro.CRM.IService;
using Antibody.CareToKnowPro.CRM.Models;
using Antibody.CareToKnowPro.CRM.Pipeline;
using Antibody.CareToKnowPro.CRM.Security;
using Antibody.CareToKnowPro.CRM.Services;
using Microsoft.EntityFrameworkCore;

using Microsoft.OpenApi.Models;
using Antibody.CareToKnowPro.CRM.Controllers.Account;

namespace Antibody.CareToKnowPro.CRM
{
    public class Startup
    {
        protected readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _environment;
        
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        private void ConfigureServicesForMvc(IServiceCollection services)
        {
            services.AddMvc(options =>
                {
                    options.Filters.Add(typeof(FluentValidationActionFilter));
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = actionContext =>
                    {
                        // re-package model state errors into a problem details object
                        // which will get serialized to JSON in HTTP 400 responses
                        var problemDetails = new ValidationProblemDetails(actionContext.ModelState)
                        {
                            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                            Instance = actionContext.HttpContext.Request.Path,
                            Status = (int)System.Net.HttpStatusCode.BadRequest,
                            Detail = "Refer to errors property for additional details"
                        };

                        return new BadRequestObjectResult(problemDetails)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    };
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.SetGlobalOptions();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddResponseCompression();
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            services.AddResponseCaching();
            services.AddTransient<ICustomerIoService, CustomerIoService>();
            services.AddTransient<IEncryptionService, EncryptionService>();
            services.AddSingleton<IConfiguration>(this._configuration);
            //services.AddSingleton<IAzureStorageService>(new AzureStorageService(this._configuration["AzureStorage:ConnectionString"]));
        }

        private void ConfigureServicesForDatabases(IServiceCollection services)
        {
            var databaseConnectionString = _configuration.GetConnectionString("Database.DbAntibodyCareToKnowProCrm");
            services.AddDbContext<DbAntibodyCareToKnowProContext>(options => options
                .UseSqlServer(databaseConnectionString)
                .EnableSensitiveDataLogging(_environment.IsDevelopment()));

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PatientCareSolution API", Version = "v1" });
            });
        }

        private void ConfigureServicesForFluentValidation(IServiceCollection services)
        {
            // add validators from this project with transient lifetime
            services.AddValidatorsFromAssemblyContaining<Startup>(ServiceLifetime.Transient);
        }

        private void ConfigureServicesForMediatr(IServiceCollection services)
        {
            // load command/query handlers from the assemblies they are found in
            services.AddMediatR(typeof(Startup).Assembly);

            // register pipeline behaviours           
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FluentValidationPipelineBehavior<,>));
        }

        private static readonly Func<RedirectContext<CookieAuthenticationOptions>, Task> OnRedirectToLogin = async ctx =>
        {
            // repackage into HTTP 401 Unauthorized
            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
                Instance = ctx.Request.Path,
                Title = "Unauthorized",
                Status = (int)System.Net.HttpStatusCode.Unauthorized,
                Detail = "Please log in"
            };

            ctx.Response.StatusCode = problemDetails.Status.Value;
            await ctx.Response.WriteJsonAsync(problemDetails, "application/problem+json");
        };

        private static readonly Func<RedirectContext<CookieAuthenticationOptions>, Task> OnRedirectToAccessDenied = async ctx =>
        {
            // repackage into HTTP 403 Forbidden
            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                Instance = ctx.Request.Path,
                Title = "Forbidden",
                Status = (int)System.Net.HttpStatusCode.Forbidden,
                Detail = "Access denied"
            };

            ctx.Response.StatusCode = problemDetails.Status.Value;
            await ctx.Response.WriteJsonAsync(problemDetails, "application/problem+json");
        };

        private void ConfigureServicesForAuthentication(IServiceCollection services)
        {
            services.AddOptions<LoginCredentialsOptions>().Bind(_configuration.GetSection("LoginService"))
                                                          .PostConfigure(opts =>
                                                          {
                                                              var optionsValidator = new LoginCredentialsOptionsValidator();
                                                              optionsValidator.ValidateAndThrow(opts);
                                                          });

            services.AddTransient<ILoginService, ConfigurableCredentialsLoginService>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = AuthenticationSchemes.DefaultAuthenticationScheme;
                options.DefaultSignInScheme = AuthenticationSchemes.DefaultAuthenticationScheme;
                options.DefaultSignOutScheme = AuthenticationSchemes.DefaultAuthenticationScheme;
                options.DefaultAuthenticateScheme = AuthenticationSchemes.DefaultAuthenticationScheme;
                options.DefaultChallengeScheme = AuthenticationSchemes.DefaultAuthenticationScheme;
                options.DefaultForbidScheme = AuthenticationSchemes.DefaultAuthenticationScheme;
            })
            .AddCookie(AuthenticationSchemes.DefaultAuthenticationScheme, options =>
            {
                options.Cookie.Name = AuthenticationSchemes.DefaultAuthenticationScheme;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
                options.Cookie.HttpOnly = false;

                options.Events.OnRedirectToLogin = OnRedirectToLogin;
                options.Events.OnRedirectToAccessDenied = OnRedirectToAccessDenied;
            });
        }

        private void ConfigureServicesForSinglePageApplication(IServiceCollection services)
        {
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureServicesForMvc(services);
            ConfigureServicesForFluentValidation(services);
            ConfigureServicesForAuthentication(services);
            ConfigureServicesForDatabases(services);
            ConfigureServicesForSinglePageApplication(services);

            ConfigureServicesForMediatr(services);
        }

        private readonly RequestDelegate _developmentExceptionHandler = async context =>
        {
            var exception = GetErrorFeatureException(context);
            var errorDetail = exception.Demystify().ToString();

            await WriteExceptionJson(context, errorDetail, exception);
        };

        private readonly RequestDelegate _productionExceptionHandler = async context =>
        {
            // in production we can show a generic error message in an HTTP 500 error (API response)
            var exception = GetErrorFeatureException(context);
            var errorDetail = $"Unhandled error.  Please refer to code {context.TraceIdentifier} when contacting support.";

            await WriteExceptionJson(context, errorDetail, exception);
        };

        private static Exception GetErrorFeatureException(HttpContext context)
        {
            var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
            return errorFeature.Error;
        }

        private static async Task WriteExceptionJson(HttpContext context, string errorDetail, Exception exception)
        {
            var problemDetails = new ProblemDetails
            {
                Instance = context.Request.Path,
                Title = "Internal Server Error",
                Status = (int)System.Net.HttpStatusCode.InternalServerError,
                Detail = errorDetail
            };

            // write the problem details to the response
            context.Response.StatusCode = problemDetails.Status.Value;
            await context.Response.WriteJsonAsync(problemDetails, "application/problem+json");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/api/docs/PatientCareSolution.Api_1.0.0.json", "PatientCareSolution API 1.0.0");
            //    c.RoutePrefix = "api/docs";
            //});

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });



            // define exception handler(dev vs prod)
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(env.IsDevelopment() ? _developmentExceptionHandler : _productionExceptionHandler);
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
