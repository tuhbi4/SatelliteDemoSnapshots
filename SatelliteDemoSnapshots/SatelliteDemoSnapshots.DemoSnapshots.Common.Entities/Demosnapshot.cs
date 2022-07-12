using Microsoft.SqlServer.Types;
using System;

namespace SatelliteDemoSnapshots.DemoSnapshots.Common.Entities
{
    public class DemoSnapshot
    {
        public int Id { get; set; }

        public Satellites Satellite { get; set; }

        public DateTime ShootingDate { get; set; }

        public decimal Cloudiness { get; set; }

        public int Turn { get; set; }

        public SqlGeography Coordinates { get; set; }
    }
}