using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SatelliteDemoSnapshots.DemoSnapshots.Common.Dependencies;
using SatelliteDemoSnapshots.DemoSnapshots.DL.DBO.Migrations;

namespace SatelliteDemoSnapshots.DemoSnapshots.BL.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.InjectDependencies(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.MigrateDatabase(Configuration);
        }
    }

    internal static class StartupExtensions
    {
        public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder app, IConfiguration configuration)
        {
            MigrationRunner.RunMigrations(configuration);
            return app;
        }
    }
}