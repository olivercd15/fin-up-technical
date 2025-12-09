using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Application.Interfaces
{
    public interface IEventBus
    {
        Task PublishAsync(string topic, object message);
    }
}
