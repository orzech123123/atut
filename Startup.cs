﻿using System;
using System.Globalization;
using System.IO;
using Atut.Filters;
using Atut.Identity;
using Atut.Models;
using Atut.Services;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace Atut
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }

        private IHostingEnvironment Env { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(Environment.GetEnvironmentVariable("cs")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<PolishIdentityErrorDescriber>();

            services.AddScoped<IUserClaimsPrincipalFactory<User>, IdentityAppClaimsPrincipalFactory>();
            
            services.AddMvc(options =>
            {
                options.Filters.Add<RouteHelperUpdateFilter>();
            });

            services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 443;
            });

            services.AddAutoMapper();

            services.AddSingleton<INotificationManager, NotificationManager>();
            services.AddScoped<IDatabaseManager, DatabaseManager>();
            services.AddScoped<RequestModelService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IEmailService, EmailService>();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddTransient<VehicleService>();
            services.AddTransient<JourneyService>();
            services.AddTransient<RoleService>();
            services.AddTransient<VatNumberService>();
            services.AddTransient<CountriesHelper>();
            services.AddScoped<RouteHelper>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ReportService>();
            services.AddTransient<EnglishReportLabelDictionary>();
            services.AddTransient<GermanReportLabelDictionary>();
            services.AddTransient<Authorizer>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(x => {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });
        }
        
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IDatabaseManager databaseManager
            )
        {
            SetupCulture();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            loggerFactory.AddFile("Logs/logs-{Date}.txt", LogLevel.Error);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "AdminLTE-2.4.3")),
                RequestPath = "/AdminLTE-2.4.3",
                EnableDirectoryBrowsing = true
            });

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            databaseManager.Migrate();
            databaseManager.EnsureDatabaseCreated();
        }

        private static void SetupCulture()
        {
            var cultureInfo = new CultureInfo("pl-PL");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
    }
}
