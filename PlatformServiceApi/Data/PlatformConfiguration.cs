using PlatformServiceApi.Models;

namespace PlatformServiceApi.Data;

public static class PlatformConfiguration
{
    public static async Task InitializeDatabase(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var platformContext = serviceScope.ServiceProvider.GetRequiredService<PlatformContext>();
            await SeedDatabase(platformContext);
        }
    }

    private static async Task SeedDatabase(PlatformContext platformContext)
    {
        if (!platformContext.Platforms.Any())
        {
            Console.WriteLine("Seeding database...");

            await platformContext.AddRangeAsync(
                new Platform
                {
                    Id = 1,
                    Name = "DotNet",
                    Publisher = "Microsoft",
                    Cost = "Free"
                }, new Platform
                {
                    Id = 2,
                    Name = "Sql Server",
                    Publisher = "Microsoft",
                    Cost = "Free"
                }, new Platform
                {
                    Id = 3,
                    Name = "Kubernetes",
                    Publisher = "Cloud Native Computing Foundation",
                    Cost = "Free"
                }
            );
            
            await platformContext.SaveChangesAsync();
        }
        else
        {
            Console.WriteLine("Platform already has data.");
        }
    }
}