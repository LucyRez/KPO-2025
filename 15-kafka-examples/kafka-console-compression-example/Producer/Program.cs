using System;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;

public class KafkaProducer
{
    public static async Task Main()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
            CompressionType = CompressionType.Gzip 
        };

        using var producer = new ProducerBuilder<string, string>(config).Build();

        // Пример: простая строка
        await producer.ProduceAsync("demo", new Message<string, string>
        {
            Key = "simple",
            Value = "Привет, Kafka!"
        });

        // Пример: JSON-сообщение
        var person = new { name = "Alice", age = 30 };
        string json = JsonSerializer.Serialize(person);

        await producer.ProduceAsync("demo", new Message<string, string>
        {
            Key = "json",
            Value = json
        });

        Console.WriteLine("✅ Сообщения отправлены");
    }
}
