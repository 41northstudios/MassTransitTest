using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransitTest
{
    public interface IExampleResponse
    {
        Guid RequestId { get; }
        DateTime Timestamp { get; }
        ICustomClass CustomClass { get; }
    }
}
