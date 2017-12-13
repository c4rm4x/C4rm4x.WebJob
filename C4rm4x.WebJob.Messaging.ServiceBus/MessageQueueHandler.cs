#region Using

using C4rm4x.Tools.ServiceBus;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebJob.Framework.Messaging;
using Microsoft.ServiceBus.Messaging;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebJob.Messaging.ServiceBus
{
    /// <summary>
    /// Implementation of IMessageQueueHandler using ServiceBus TopicClient
    /// </summary>
    public class MessageQueueHandler : IMessageQueueHandler
    {
        /// <summary>
        /// Gets the topic client
        /// </summary>
        public TopicClient TopicClient { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="topicClient">The topic client</param>
        public MessageQueueHandler(TopicClient topicClient)
        {
            topicClient.NotNull(nameof(topicClient));

            TopicClient = topicClient;
        }

        /// <summary>
        /// Sends a new item into the message queue
        /// </summary>
        /// <typeparam name="TItem">Type of the item</typeparam>
        /// <param name="item">The new item</param>
        public Task SendAsync<TItem>(TItem item) where TItem : class
        {
            return TopicClient.SendAsync(item.BuildBrokeredMessage());
        }
    }
}
