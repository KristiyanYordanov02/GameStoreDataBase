using System.Text.Json;
using Confluent.Kafka;
using GameStore.Core.Models;

public class KafkaCacheConsumer
{
    private readonly string _topic = "game-cache";
    private readonly IConsumer<Ignore, string> _consumer;

    public KafkaCacheConsumer()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "game-cache-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        _consumer.Subscribe(_topic);
    }

    public Task StartConsumingAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var result = _consumer.Consume(cancellationToken);
                    var game = JsonSerializer.Deserialize<Game>(result.Message.Value);
                    if (game != null)
                    {
                        InMemoryCache.Games.Add(game);
                        Console.WriteLine($"Cached game: {game.Title}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _consumer.Close();
            }
        }, cancellationToken);
    }
}