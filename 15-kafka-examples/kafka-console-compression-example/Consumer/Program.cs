using System;
using System.Text.Json;
using System.Threading;
using Confluent.Kafka;

public class KafkaConsumer
{
    public static void Main()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "demo-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true
        };

        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe("demo");

        Console.WriteLine("🎧 Чтение сообщений из топика 'demo'...");

        while (true)
        {
            try
            {
                var cr = consumer.Consume(CancellationToken.None);

                Console.WriteLine($"📥 Ключ: {cr.Message.Key}");
                Console.WriteLine($"📦 Значение: {cr.Message.Value}");

                try
                {
                    var obj = JsonSerializer.Deserialize<JsonElement>(cr.Message.Value);
                    Console.WriteLine($"🔍 Распознанный JSON: {obj}");
                }
                catch
                {
                    Console.WriteLine("ℹ️ Не JSON, просто строка");
                }

                Console.WriteLine($"📍 Offset: {cr.Offset}");
                Console.WriteLine("------");
            }
            catch (ConsumeException e)
            {
                Console.WriteLine($"❌ Ошибка при получении: {e.Error.Reason}");
            }
        }
    }
}
