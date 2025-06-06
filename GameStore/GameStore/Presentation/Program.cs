using GameStore.Core.Interfaces;
using GameStore.Core.Models;
using GameStore.Infrastructure;
using GameStore.Infrastructure.Data;
using GameStore.Infrastructure.Repositories;
using GameStore.Presentation.HealthChecks;
using GameStore.Service;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using MongoDB.Driver;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        builder.Services.AddControllers();
        builder.Services.AddScoped<IGameRepository, GameRepository>();
        builder.Services.AddScoped<IGameService, GameService>();
        builder.Services.AddHealthChecks().AddCheck<HealthCheck>("GameStoreHealthCheck");
        builder.Services.AddSingleton<MongoDBContext>();

        // Add Swagger services
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        builder.Host.UseSerilog();

        var app = builder.Build();

        // Seed data
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<MongoDBContext>();
            var gameRepository = new GameRepository(context);

            var games = new List<Game>
            {
                new Game { Id = Guid.NewGuid(), Name = "The Witcher 3: Wild Hunt", Price = 49.99m, Developer = "CD Projekt Red" },
                new Game { Id = Guid.NewGuid(), Name = "Cyberpunk 2077", Price = 49.99m, Developer = "CD Projekt Red" },
                new Game { Id = Guid.NewGuid(), Name = "Elden Ring", Price = 29.99m, Developer = "FromSoftware" },
                new Game { Id = Guid.NewGuid(), Name = "Red Dead Redemption 2", Price = 59.99m, Developer = "Rockstar Games" },
                new Game { Id = Guid.NewGuid(), Name = "The Legend of Zelda: Breath of the Wild", Price = 59.99m, Developer = "Nintendo" },
                new Game { Id = Guid.NewGuid(), Name = "God of War", Price = 39.99m, Developer = "Santa Monica Studio" },
                new Game { Id = Guid.NewGuid(), Name = "Halo Infinite", Price = 59.99m, Developer = "343 Industries" },
                new Game { Id = Guid.NewGuid(), Name = "Horizon Forbidden West", Price = 59.99m, Developer = "Guerrilla Games" },
                new Game { Id = Guid.NewGuid(), Name = "Dark Souls II", Price = 49.99m, Developer = "FromSoftware" },
                new Game { Id = Guid.NewGuid(), Name = "Starfield", Price = 39.99m, Developer = "Bethesda Game Studios" },
                new Game { Id = Guid.NewGuid(), Name = "Assassin's Creed Valhalla", Price = 59.99m, Developer = "Ubisoft" },
                new Game { Id = Guid.NewGuid(), Name = "Call of Duty: Modern Warfare", Price = 69.99m, Developer = "Infinity Ward" },
                new Game { Id = Guid.NewGuid(), Name = "FIFA 24", Price = 49.99m, Developer = "EA Sports" },
                new Game { Id = Guid.NewGuid(), Name = "Resident Evil Village", Price = 49.99m, Developer = "Capcom" },
                new Game { Id = Guid.NewGuid(), Name = "Spider-Man: Miles Morales", Price = 49.99m, Developer = "Insomniac Games" },
                new Game { Id = Guid.NewGuid(), Name = "Final Fantasy VII Remake", Price = 49.99m, Developer = "Square Enix" },
                new Game { Id = Guid.NewGuid(), Name = "Death Stranding", Price = 39.99m, Developer = "Kojima Productions" },
                new Game { Id = Guid.NewGuid(), Name = "Ghost of Tsushima", Price = 39.99m, Developer = "Sucker Punch Productions" },
                new Game { Id = Guid.NewGuid(), Name = "Demon's Souls", Price = 69.99m, Developer = "Bluepoint Games" },
                new Game { Id = Guid.NewGuid(), Name = "Metro Exodus", Price = 49.99m, Developer = "4A Games" }
            };

            foreach (var game in games)
            {
                await gameRepository.AddAsync(game);
            }
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.MapControllers();
        app.MapHealthChecks("/health");

        await app.RunAsync("http://localhost:5000");

    }
}
