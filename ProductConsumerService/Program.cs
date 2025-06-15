using Confluent.Kafka;
using System.Text.Json;
namespace ProductConsumerService
{

    public class Program
    {
        static void Main(string[] args)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "product-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe("product-created");

            Console.WriteLine("Listening for product-created events...");

            while (true)
            {
                var result = consumer.Consume();
                var @event = JsonSerializer.Deserialize<ProductCreatedEvent>(result.Message.Value);
                Console.WriteLine($"[Kafka Consumer] New product created: {@event?.Name} (ID: {@event?.Id})");
            }

          
        }
        public record ProductCreatedEvent(Guid Id, string Name);
    }
}
