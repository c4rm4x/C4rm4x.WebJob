#region Using

using Autofac;
using C4rm4x.Tools.ServiceBus;
using C4rm4x.Tools.Utilities;
using Microsoft.ServiceBus.Messaging;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebJob.Framework.Autofac.MessageHandling
{
    /// <summary>
    /// Service responsible to process each message that triggers the job
    /// creating a new scope to isolate each brokered message received
    /// </summary>
    public class ServiceBusTriggerProcessor
        : TriggerProcessor, ITriggerProcessor
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lifetimeScope">The Lifetime Scope factory</param>
        public ServiceBusTriggerProcessor(ILifetimeScope lifetimeScope)
            : base(lifetimeScope)
        {
        }

        /// <summary>
        /// Process each message that triggers the job 
        /// by creating a single scope for each of them
        /// </summary>
        public override Task ProcessAsync<TMessage>(TMessage message)
        {
            message.NotNull(nameof(message));
            message.GetType().Is<BrokeredMessage>();

            var brokeredMessage = message as BrokeredMessage;

            return base.ProcessAsync(brokeredMessage.ExtractContent());
        }
    }
}
