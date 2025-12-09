using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using System.Text.Json;
using Payments.Application.Interfaces;

namespace Payments.Infrastructure.Messaging
{
    public class KafkaEventBus : IEventBus
    {
        private readonly IProducer<string, string> _producer;

        public KafkaEventBus(IProducer<string, string> producer)
        {
            _producer = producer;
        }

        public async Task PublishAsync(string topic, object message)
        {
            var json = JsonSerializer.Serialize(message);

            await _producer.ProduceAsync(topic, new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = json
            });
        }
    }
}
