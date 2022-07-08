using System;

namespace SatelliteDemoSnapshots.DemoSnapshots.Common.Entities
{
    public class DemoSnapshot
    {
        public int Id { get; set; }

        public string Satellite { get; set; }

        public DateTime ShootingDate { get; set; }

        public decimal Cloudiness { get; set; }

        public int Turn { get; set; }

        public string Coordinates { get; set; }
    }
}