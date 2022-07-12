using AutoMapper;
using Microsoft.SqlServer.Types;
using SatelliteDemoSnapshots.DemoSnapshots.BL.API.Models;
using SatelliteDemoSnapshots.DemoSnapshots.Common.Entities;
using System.Data.SqlTypes;

namespace SatelliteDemoSnapshots.DemoSnapshots.BL.API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DemoSnapshot, DemoSnapshotModel>();
            CreateMap<DemoSnapshotModel, DemoSnapshot>()
                .ForMember(dest => dest.Coordinates, opt => opt.MapFrom(src => SqlGeography.Parse(new SqlString(src.Coordinates))));
        }
    }
}