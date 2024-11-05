using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlatformServiceApi.Data;
using PlatformServiceApi.Records;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddDbContext<PlatformContext>(options => options.UseInMemoryDatabase("InMemoryDatabase"));
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

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
});

app.Run();

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}