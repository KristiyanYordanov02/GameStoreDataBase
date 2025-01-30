using GameStore.Core.Interfaces;
using GameStore.Core.Models;
using GameStore.Service;
using Moq;
using Xunit;

namespace GameStore.Tests
{
    public class GameServiceTests
    {
        [Fact]
        public async Task AddGame_ShouldReturnGame()
        {
            // Arrange
            var mockRepo = new Mock<IGameRepository>();
            var gameService = new GameService(mockRepo.Object);
            var gameDto = new GameDto { Name = "Test Game", Price = 59.99m, Developer = "Test Dev" };

            // Act
            var result = await gameService.AddGame(gameDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(gameDto.Name, result.Name);
        }
    }
}