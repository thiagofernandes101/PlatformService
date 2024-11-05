using Microsoft.EntityFrameworkCore;
using PlatformServiceApi.Models;
using PlatformServiceApi.Models.Pattern;

namespace PlatformServiceApi.Data;

public interface IPlatformRepository
{
    public Task CreatePlatform(Platform platform);
    public Task<IList<Platform>> GetAllPlatformsAsync();
    public Task<Result<Platform>> GetPlatformByIdAsync(int id);
    public Task<bool> SaveChangesAsync();
}

public class PlatformRepository : IPlatformRepository
{
    private readonly PlatformContext _context;

    public PlatformRepository(PlatformContext context)
    {
        _context = context;
    }
    
    public async Task CreatePlatform(Platform platform)
    {
        ArgumentNullException.ThrowIfNull(platform);
        await _context.Platforms.AddAsync(platform);
    }

    public async Task<IList<Platform>> GetAllPlatformsAsync()
    {
        var result = _context.Platforms.ToList();
        return await Task.FromResult(result);
    }

    public async Task<Result<Platform>> GetPlatformByIdAsync(int id)
    {
        var result = await _context.Platforms.FirstOrDefaultAsync(p => p.Id == id);
        return result != null
            ? Result<Platform>.Success(result)
            : Result<Platform>.Failure("Platform not found");
    }

    public async Task<bool> SaveChangesAsync()
    {
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }
}