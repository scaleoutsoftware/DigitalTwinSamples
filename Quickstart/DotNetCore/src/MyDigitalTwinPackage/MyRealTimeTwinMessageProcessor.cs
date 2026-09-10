using Microsoft.Extensions.Logging.Abstractions;
using Scaleout.Modules.DigitalTwin.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDigitalTwinPackage
{
    internal class MyRealTimeTwinMessageProcessor : MessageProcessor<MyRealTimeTwinModel>
    {
        ILogger<MyRealTimeTwinMessageProcessor> _logger;

        /// <summary>
        /// Message handler constructor. Parameters are supplied via Dependency Injection
        /// and can be modified as needed.
        /// </summary>
        /// <param name="logger">ILogger instance.</param>
        public MyRealTimeTwinMessageProcessor(ILogger<MyRealTimeTwinMessageProcessor> logger)
        {
            _logger = logger ?? NullLogger<MyRealTimeTwinMessageProcessor>.Instance;
        }


        /// <summary>
        /// Processes a message sent to the specified digital twin instance.
        /// </summary>
        /// <param name="context">The processing context for the operation.</param>
        /// <param name="digitalTwin">The digital twin instance receiving the message.</param>
        /// <param name="msgBytes">The raw message data as a byte array.</param>
        /// <returns>A <see cref="ProcessingResult"/> representing the outcome of the message processing.</returns>
        /// <remarks>
        /// <para>
        /// A <see cref="ProcessingResult.NoUpdate"/> value can be returned to
        /// indicate that the <paramref name="digitalTwin"/> instance was not modified 
        /// and does not need to be updated in the ScaleOut service.
        /// <see cref="ProcessingResult.NoUpdate"/> should not be returned if the object
        /// was modified in any way.
        /// If you are unsure of whether the <paramref name="digitalTwin"/> was modified, 
        /// return <see cref="ProcessingResult.DoUpdate"/>.
        /// </remarks>
        public override Task<ProcessingResult> ProcessMessageAsync(ProcessingContext<MyRealTimeTwinModel> context, MyRealTimeTwinModel digitalTwin, byte[] msgBytes)
        {
            // Deserialize the message bytes into our specific message type.
            MyRealTimeTwinMessage? message = System.Text.Json.JsonSerializer.Deserialize<MyRealTimeTwinMessage>(msgBytes);
            if (message is null)
                return Task.FromResult(ProcessingResult.NoUpdate);

            // Modify the instance's state based on the message content.
            digitalTwin.CurrentValue = message.StringPayload;

            // The SOSS object was modified, so return DoUpdate to indicate that
            // it should be updated in the ScaleOut service.
            return Task.FromResult(ProcessingResult.DoUpdate);
        }
    }
}
