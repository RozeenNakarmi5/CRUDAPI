using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CRUDOperationAPI.Contexts;
using Microsoft.EntityFrameworkCore;
using CRUDOperationAPI.Connections;
using CRUDOperationAPI.Implementation;
using CRUDOperationAPI.InterfaceClass;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CRUDOperationAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;

namespace CRUDOperationAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            
            services.Configure<ConnectionConfig>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<TokenAuthentication>(Configuration.GetSection("TokenAuthentication"));

            services.AddDbContext<EmployeeDbContext>(item =>
            item.UseSqlServer(Configuration.GetConnectionString("myconn")));

            services.AddCors();

            services.AddScoped<ILoginServices, UserServiceImplementation>();
            services.AddScoped<IEmployeeService, EmployeeImplementation>();
            services.AddScoped<IClientService, ClientImplementation>();
            services.AddScoped<IProjectService, ProjectImplementation>();

            services.Configure<IdentityOptions>(options =>
            {
                // avoid redirecting REST clients on 401
                options.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                        ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return Task.FromResult(0);
                    }
                };
            });
            services.AddMvc();

            services.AddScoped<IDepartmentService, DepartmentImplementation>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();
            app.UseDeveloperExceptionPage();
            app.UseApplicationInsightsExceptionTelemetry();
            
            app.UseCors(
               option => option.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            );

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("TokenAuthentication:SecretKey").Value));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,

                TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Configuration.GetSection("TokenAuthentication:Issuer").Value,
                    ValidAudience = Configuration.GetSection("TokenAuthentication:Audience").Value,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateLifetime = true
                }

            });
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = false,
                AutomaticChallenge = false
            });


            app.UseMvc();
        }
    }
}
