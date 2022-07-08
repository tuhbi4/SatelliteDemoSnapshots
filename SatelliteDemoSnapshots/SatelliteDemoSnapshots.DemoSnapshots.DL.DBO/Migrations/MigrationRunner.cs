using Dapper;
using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace SatelliteDemoSnapshots.DemoSnapshots.DL.DBO.Migrations
{
    public static class MigrationRunner
    {
        public static void RunMigrations(IConfiguration configuration)
        {
            var serviceProvider = CreateServices(configuration);

            using (var scope = serviceProvider.CreateScope())
            {
                EnsureDatabase(configuration);
                UpdateDatabase(scope.ServiceProvider);
            }
        }

        public static void EnsureDatabase(IConfiguration configuration)
        {
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(configuration.GetConnectionString("DefaultConnection"));
            var databaseName = sqlConnectionStringBuilder.InitialCatalog;

            sqlConnectionStringBuilder.InitialCatalog = "master";

            using (var connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                var sql = "SELECT * FROM master.dbo.sysdatabases WHERE name = @Name";
                connection.Open();
                var result = connection.Query<string>(sql, new { Name = databaseName }).Count();

                if (result == 0)
                {
                    connection.Execute($"CREATE DATABASE {databaseName}");
                }
            }
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }

        private static IServiceProvider CreateServices(IConfiguration configuration)
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSqlServer()
                    .WithGlobalConnectionString(configuration.GetConnectionString("DefaultConnection"))
                    .ScanIn(typeof(MigrationRunner).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }
    }
}