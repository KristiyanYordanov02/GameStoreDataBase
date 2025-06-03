using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using GameStore.Core.Interfaces;
using GameStore.Core.Models;

namespace GameStore.Service
{
    public class KafkaCachePublisher
    {
        private readonly IGameRepository _repository;
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic = "game-cache";

        public KafkaCachePublisher(IGameRepository repository)
        {
            _repository = repository;
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task StartPublishingAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var games = await _repository.GetAllAsync();
                foreach (var game in games)
                {
                    var json = JsonSerializer.Serialize(game);
                    await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = json }, cancellationToken);
                }
                await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
            }
        }
    }
}