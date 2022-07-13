using System.ComponentModel.DataAnnotations;

namespace SatelliteDemoSnapshots.DemoSnapshots.BL.API.Models
{
    public class DemoSnapshotModel
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(255)]
        public string Satellite { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public string ShootingDate { get; set; }

        public decimal? Cloudiness { get; set; }

        [Required]
        public int Turn { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Coordinates { get; set; }
    }
}