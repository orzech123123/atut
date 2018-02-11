using System.IO;
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
            //            RegisterServices(services);

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            IMvcBuilder mvc = services.AddMvc(options =>
            {
//                options.Filters.Add(typeof(ApiExceptionFilter));
            });

            services.AddSingleton<INotificationManager, NotificationManager>();
            services.AddScoped<IDatabaseManager, DatabaseManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IEmailService, EmailLabsMailService>();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

            //            IndentJsonForDevelopment(mvc);

            //            services.Configure<ConfigurationManager>(Configuration.GetSection("AppSettings"));
            //            services.Resolve<IJWtHelper>().AddJwtAuthentication(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IDatabaseManager databaseManager
            )
        {
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

            databaseManager.EnsureDatabaseCreated();
        }

//        private void RegisterServices(IServiceCollection services)
//        {
//            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CallGateConnectionString")));
//            services.AddAutoMapper();
//            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
//            DependencyInjectionRegisterer.RegisterAssemblies(services);
//
//            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
//            services.AddScoped<IUrlHelper>(provider =>
//            {
//                var actionContext = provider.GetService<IActionContextAccessor>().ActionContext;
//                return new UrlHelper(actionContext);
//            });
//            services.AddTransient<ISeeder, UserSeeder>();
//            services.AddTransient<ISeeder, GroupSeeder>();
//        }
    }
}
