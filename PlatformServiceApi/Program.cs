using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlatformServiceApi.Data;
using PlatformServiceApi.Models;
using PlatformServiceApi.Models.Pattern;
using PlatformServiceApi.Records;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddDbContext<PlatformContext>(options => options.UseInMemoryDatabase("InMemoryDatabase"));
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await PlatformConfiguration.InitializeDatabase(app);
}

var platformApi = app.MapGroup("api/platforms");

platformApi.MapGet("/", async (IPlatformRepository platformRepository, IMapper mapper) =>
{
    var platforms = await platformRepository.GetAllPlatformsAsync();
    return platforms.Any() ? Results.Ok(mapper.Map<IList<PlatformRead>>(platforms)) : Results.NotFound();
});

platformApi.MapGet("/{id}", async (int id, IPlatformRepository platformRepository, IMapper mapper) =>
{
    var platform = await platformRepository.GetPlatformByIdAsync(id);
    return platform.IsSuccess
        ? Results.Ok(mapper.Map<PlatformRead>(platform.Value))
        : Results.NotFound(platform.Error);
}).WithName("GetPlatformById");

platformApi.MapPost("/",
    async (PlatformCreate platform, IPlatformRepository platformRepository, IMapper mapper) =>
    {
        var mappedPlatform = mapper.Map<Platform>(platform);
        await platformRepository.CreatePlatformAsync(mappedPlatform);
        var isSuccessful = await platformRepository.SaveChangesAsync();
        return isSuccessful
            ? Results.CreatedAtRoute("GetPlatformById", new{Id = mappedPlatform.Id}, mapper.Map<PlatformRead>(mappedPlatform))
            : Results.BadRequest("Could not create platform");
    });

app.Run();