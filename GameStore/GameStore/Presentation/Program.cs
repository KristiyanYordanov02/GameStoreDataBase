using GameStore.Core.Interfaces;
using GameStore.Core.Models;
using GameStore.Infrastructure;
using GameStore.Infrastructure.Repositories;
using GameStore.Presentation.HealthChecks;
using GameStore.Service;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddHealthChecks().AddCheck<HealthCheck>("GameStoreHealthCheck");

var mongoContext = new MongoDBContext("mongodb://localhost:27017", "GameStore");
builder.Services.AddSingleton(mongoContext.GetDatabase());

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
builder.Host.UseSerilog();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var mongoDatabase = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
    var gameRepository = new GameRepository(mongoDatabase);

    var games = new List<Game>
    {
        new Game { Id = Guid.NewGuid(), Name = "The Witcher 3: Wild Hunt", Price = 49.99m, Developer = "CD Projekt Red" },
        new Game { Id = Guid.NewGuid(), Name = "Cyberpunk 2077", Price = 49.99m, Developer = "CD Projekt Red" },
        new Game { Id = Guid.NewGuid(), Name = "Elden Ring", Price = 49.99m, Developer = "FromSoftware" },
        new Game { Id = Guid.NewGuid(), Name = "Red Dead Redemption 2", Price = 59.99m, Developer = "Rockstar Games" },
        new Game { Id = Guid.NewGuid(), Name = "The Legend of Zelda: Breath of the Wild", Price = 59.99m, Developer = "Nintendo" },
        new Game { Id = Guid.NewGuid(), Name = "God of War", Price = 49.99m, Developer = "Santa Monica Studio" },
        new Game { Id = Guid.NewGuid(), Name = "Halo Infinite", Price = 39.99m, Developer = "343 Industries" },
        new Game { Id = Guid.NewGuid(), Name = "Horizon Forbidden West", Price = 59.99m, Developer = "Guerrilla Games" },
        new Game { Id = Guid.NewGuid(), Name = "Dark Souls III", Price = 39.99m, Developer = "FromSoftware" },
        new Game { Id = Guid.NewGuid(), Name = "Starfield", Price = 49.99m, Developer = "Bethesda Game Studios" },
        new Game { Id = Guid.NewGuid(), Name = "Assassin's Creed Valhalla", Price = 59.99m, Developer = "Ubisoft" },
        new Game { Id = Guid.NewGuid(), Name = "Call of Duty: Modern Warfare", Price = 69.99m, Developer = "Infinity Ward" },
        new Game { Id = Guid.NewGuid(), Name = "FIFA 23", Price = 49.99m, Developer = "EA Sports" },
        new Game { Id = Guid.NewGuid(), Name = "Resident Evil Village", Price = 49.99m, Developer = "Capcom" },
        new Game { Id = Guid.NewGuid(), Name = "Spider-Man: Miles Morales", Price = 49.99m, Developer = "Insomniac Games" },
        new Game { Id = Guid.NewGuid(), Name = "Final Fantasy VII Remake", Price = 49.99m, Developer = "Square Enix" },
        new Game { Id = Guid.NewGuid(), Name = "Death Stranding", Price = 39.99m, Developer = "Kojima Productions" },
        new Game { Id = Guid.NewGuid(), Name = "Ghost of Tsushima", Price = 59.99m, Developer = "Sucker Punch Productions" },
        new Game { Id = Guid.NewGuid(), Name = "Demon's Souls", Price = 59.99m, Developer = "Bluepoint Games" },
        new Game { Id = Guid.NewGuid(), Name = "Metro Exodus", Price = 39.99m, Developer = "4A Games" }
    };

    foreach (var game in games)
    {
        await gameRepository.AddGame(game);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();