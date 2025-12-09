using Payments.Application.Interfaces;

namespace Payments.Infrastructure.Messaging
{
    public class NullEventBus : IEventBus
    {
        public Task PublishAsync(string topic, object message)
        {
            Console.WriteLine($"[NullEventBus] Event '{topic}' ignored (Kafka not configured).");
            return Task.CompletedTask;
        }
    }
}
