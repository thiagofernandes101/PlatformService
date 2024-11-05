using Microsoft.EntityFrameworkCore;
using PlatformServiceApi.Models;

namespace PlatformServiceApi.Data;

public class PlatformContext : DbContext
{
    public PlatformContext(DbContextOptions<PlatformContext> options) : base(options)
    {
    }
    
    public DbSet<Platform> Platforms { get; set; }
}