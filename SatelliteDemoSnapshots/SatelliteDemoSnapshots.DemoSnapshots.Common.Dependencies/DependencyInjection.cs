using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SatelliteDemoSnapshots.DemoSnapshots.Common.Entities;
using SatelliteDemoSnapshots.DemoSnapshots.DL.DAO;
using SatelliteDemoSnapshots.DemoSnapshots.DL.DAO.Interfaces;
using System.Data;

namespace SatelliteDemoSnapshots.DemoSnapshots.Common.Dependencies
{
    public static class DependencyInjection
    {
        public static void InjectDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDbConnection>((serviceProvider) => new SqlConnection(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IRepository<DemoSnapshot>, DemoSnapshotsRepository>();
        }
    }
}