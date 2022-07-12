using Dapper;
using SatelliteDemoSnapshots.DemoSnapshots.Common.Entities;
using SatelliteDemoSnapshots.DemoSnapshots.DL.DAO.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SatelliteDemoSnapshots.DemoSnapshots.DL.DAO
{
    public class DemoSnapshotsRepository : IRepository<DemoSnapshot>
    {
        private readonly IDbConnection dbConnection;

        public DemoSnapshotsRepository(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public async Task<IReadOnlyList<DemoSnapshot>> GetAllAsync(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                var sql = "SELECT * FROM DemoSnapshots";
                using (dbConnection)
                {
                    dbConnection.Open();
                    var result = await dbConnection.QueryAsync<DemoSnapshot>(sql);

                    return result.ToList();
                }
            }
            else
            {
                var sql = "SELECT * FROM DemoSnapshots WHERE [Coordinates] LIKE '%' + @Query + '%' ";
                using (dbConnection)
                {
                    dbConnection.Open();
                    var result = await dbConnection.QueryAsync<DemoSnapshot>(sql, new { Query = query });

                    return result.ToList();
                }
            }
        }

        public async Task<int> CreateAsync(DemoSnapshot entity)
        {
            var sql = "Insert into DemoSnapshots (Satellite,ShootingDate,Cloudiness,Coordinates,Turn) " +
                "VALUES (@Satellite,@ShootingDate,@Cloudiness,@Coordinates,@Turn)";
            using (dbConnection)
            {
                dbConnection.Open();
                var result = await dbConnection.ExecuteAsync(sql, entity);

                return result;
            }
        }

        public async Task<DemoSnapshot> ReadAsync(int id)
        {
            var sql = "SELECT * FROM DemoSnapshots WHERE Id = @Id";
            using (dbConnection)
            {
                dbConnection.Open();
                var result = await dbConnection.QuerySingleOrDefaultAsync<DemoSnapshot>(sql, new { Id = id });

                return result;
            }
        }

        public async Task<int> UpdateAsync(DemoSnapshot entity)
        {
            var sql = "UPDATE DemoSnapshots SET Satellite = @Satellite, ShootingDate = @ShootingDate, " +
                "Cloudiness = @Cloudiness, Coordinates = @Coordinates,Turn = @Turn WHERE Id = @Id";
            using (dbConnection)
            {
                dbConnection.Open();
                var result = await dbConnection.ExecuteAsync(sql, entity);

                return result;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM DemoSnapshots WHERE Id = @Id";
            using (dbConnection)
            {
                dbConnection.Open();
                var result = await dbConnection.ExecuteAsync(sql, new { Id = id });

                return result;
            }
        }
    }
}