using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Producer.Services
{
    public class ProducerService
    {
        private readonly IConfiguration _configuration;
        private readonly IProducer<Null, string> _producer;

        public ProducerService(IConfiguration configuration)
        {
            _configuration = configuration;

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"]
            };

            _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        }

        public async Task ProduceAsync(string topic, string message)
        {
            try
            {
                var kafkaMessage = new Message<Null, string> { Value = message };
                var result = await _producer.ProduceAsync(topic, kafkaMessage);
                Console.WriteLine($"Produced message to: {result.TopicPartitionOffset}");
            }
            catch (ProduceException<Null, string> ex)
            {
                Console.WriteLine($"Error producing message: {ex.Message}");
            }
        }
    }
}
