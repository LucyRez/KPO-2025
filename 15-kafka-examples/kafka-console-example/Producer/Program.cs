using Confluent.Kafka;
using System;
using System.Threading.Tasks;

class KafkaBasicProducer
{
    public static async Task Main()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092"
        };

        using var producer = new ProducerBuilder<Null, string>(config).Build();

        try
        {
            var result = await producer.ProduceAsync(
                topic: "demo",
                message: new Message<Null, string> { Value = "Привет, Kafka!" });

            Console.WriteLine($"✅ Отправлено сообщение в {result.TopicPartitionOffset}");
        }
        catch (ProduceException<Null, string> e)
        {
            Console.WriteLine($"❌ Ошибка при отправке: {e.Error.Reason}");
        }
    }
}
