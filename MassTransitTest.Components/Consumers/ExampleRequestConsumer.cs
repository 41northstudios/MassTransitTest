using System.Threading.Tasks;
using MassTransit;
using MassTransitTest.Components.Models;

namespace MassTransitTest.Components.Consumers
{
    public class ExampleRequestConsumer : IConsumer<IExampleRequest>
    {

        public async Task Consume(ConsumeContext<IExampleRequest> context)
        {
            var payload = context.Message.Payload;

            var customClass = new CustomClass {Payload = payload};

            await context.RespondAsync<IExampleResponse>(new
            {
                context.Message.RequestId,
                InVar.Timestamp,
                CustomClass = customClass
            });

            // The above throws an error: Error MCA0001 Anonymous type does not map to message contract 'IExampleResponse'.The following properties of the anonymous type are incompatible: CustomClass.
            // If I change to CustomClass = (ICustomClass)customClass; it works
            // Why is this, CustomClass implements ICustomClass

        }
    }
}
