using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Consumer.Services
{
    public class ConsumerService : BackgroundService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ConsumerService> _logger;

        public ConsumerService(IConfiguration configuration, ILogger<ConsumerService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = "InventoryConsumerGroup",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var topicName = "InventoryUpdate";

            // Subscribe to the topic
            _consumer.Subscribe(topicName);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    ProcessKafkaMessage(stoppingToken);
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); // Shorter delay
                }
            }
            finally
            {
                _consumer.Close();
            }
        }

        private async Task CreateTopicIfNotExists(string topicName)
        {
            var adminConfig = new AdminClientConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"]
            };

            using var adminClient = new AdminClientBuilder(adminConfig).Build();

            try
            {
                var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
                var topicExists = metadata.Topics.Exists(t => t.Topic == topicName);

                if (!topicExists)
                {
                    var topicSpecification = new TopicSpecification
                    {
                        Name = topicName,
                        NumPartitions = 1,
                        ReplicationFactor = 1
                    };

                    await adminClient.CreateTopicsAsync(new[] { topicSpecification });
                    _logger.LogInformation($"Topic '{topicName}' created.");
                }
                else
                {
                    _logger.LogInformation($"Topic '{topicName}' already exists.");
                }
            }
            catch (CreateTopicsException e)
            {
                _logger.LogError($"An error occurred while creating topic '{topicName}': {e.Results[0].Error.Reason}");
            }
        }

        private void ProcessKafkaMessage(CancellationToken stoppingToken)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                var message = consumeResult.Message.Value;
                _logger.LogInformation($"Received inventory update: {message}");
                Console.WriteLine($"Consumed message '{message}' from: '{consumeResult.TopicPartitionOffset}'");
            }
            catch (ConsumeException ex)
            {
                _logger.LogError($"Error consuming message: {ex.Message}");
                Console.WriteLine($"Error consuming message: {ex.Message}");
            }
        }
    }
}
