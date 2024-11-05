using AutoMapper;
using PlatformServiceApi.Models;
using PlatformServiceApi.Records;

namespace PlatformServiceApi.Profiles;

public class PlatformProfile : Profile
{
    public PlatformProfile()
    {
        CreateMap<PlatformCreate, Platform>();
        CreateMap<Platform, PlatformRead>();
    }
}