using System;


namespace MassTransitTest
{
    public interface IExampleRequest
    {
        Guid RequestId { get; }
        DateTime Timestamp { get; }
        int Payload { get; }
    }
}
