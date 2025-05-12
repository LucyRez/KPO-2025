using Confluent.Kafka;
using System;
using System.Threading;

class KafkaBasicConsumer
{
    public static void Main()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "demo-group-3",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Null, string>(config).Build();
        consumer.Subscribe("demo");

        Console.WriteLine("🎧 Ожидание сообщений... Нажмите Ctrl+C для выхода.");

        try
        {
            while (true)
            {
                var cr = consumer.Consume(CancellationToken.None);
                Console.WriteLine($"📥 Получено сообщение: '{cr.Message.Value}' с offset {cr.Offset}");
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("🛑 Остановка по Ctrl+C");
        }
        finally
        {
            consumer.Close();
        }
    }
}
