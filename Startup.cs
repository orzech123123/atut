using System.IO;
using Atut.Identity;
using Atut.Models;
using Atut.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<PolishIdentityErrorDescriber>();

            services.AddScoped<IUserClaimsPrincipalFactory<User>, IdentityAppClaimsPrincipalFactory>();
            
            services.AddMvc();

            services.AddSingleton<INotificationManager, NotificationManager>();
            services.AddScoped<IDatabaseManager, DatabaseManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IEmailService, EmailLabsMailService>();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
        }
        
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IDatabaseManager databaseManager
            )
        {
//            var cultureInfo = new CultureInfo("pl-PL");
//            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
//            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            loggerFactory.AddFile("Logs/logs-{Date}.txt", LogLevel.Error);

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
    }
}
