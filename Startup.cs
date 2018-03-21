using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TheWall.Factories;

namespace TheWall
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }

        public Startup(IHostingEnvironment env)
        {
            var Builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

            Configuration = Builder.Build();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MySqlOptions>(Configuration.GetSection("DBInfo"));
            services.AddScoped<UserFactory>();
            services.AddScoped<MessageFactory>();
            services.AddScoped<CommentFactory>();

            services.AddSession();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder App, ILoggerFactory LoggerFactory)
        {
            LoggerFactory.AddConsole();
            App.UseDeveloperExceptionPage();
            App.UseStaticFiles();
            App.UseSession();
            App.UseMvc();
        }
    }
}