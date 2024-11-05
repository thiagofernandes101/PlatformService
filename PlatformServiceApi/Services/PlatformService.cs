using AutoMapper;
using PlatformServiceApi.Data;
using PlatformServiceApi.Models;
using PlatformServiceApi.Records;

namespace PlatformServiceApi.Services;

public interface IPlatformService
{
    public Task<IList<PlatformRead>> GetAllPlatformsAsync();
    public Task<PlatformRead> GetPlatformByIdAsync(int id);
}

public class PlatformService : IPlatformService
{
    private readonly IPlatformRepository _platformRepository;
    private readonly IMapper _mapper;

    public PlatformService(IPlatformRepository platformRepository, IMapper mapper)
    {
        _platformRepository = platformRepository;
        _mapper = mapper;
    }
    
    public async Task<IList<PlatformRead>> GetAllPlatformsAsync()
    {
        var result = await _platformRepository.GetAllPlatformsAsync();
        var mappedResult = _mapper.Map<IList<PlatformRead>>(result);
        return mappedResult;
    }

    public async Task<PlatformRead> GetPlatformByIdAsync(int id)
    {
        var result = await _platformRepository.GetPlatformByIdAsync(id);
        return result.IsSuccess 
            ? _mapper.Map<PlatformRead>(result.Value) 
            : new PlatformRead(default, default!, default!, default!);
    }
}