#region Using

using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebJob.Framework.Messaging
{
    /// <summary>
    /// Service responsible to handler messaging between modules
    /// </summary>
    public interface IMessageQueueHandler
    {
        /// <summary>
        /// Sends a new item into the message queue
        /// </summary>
        /// <param name="item">The new item</param>
        /// <returns>The task</returns>
        Task SendAsync<TItem>(TItem item)
            where TItem : class;

        /// <summary>
        /// Sends a new batch of items into the message queue
        /// </summary>
        /// <param name="items">The new items</param>
        /// <returns>The task</returns>
        Task SendAsync<TItem>(IEnumerable<TItem> items)
            where TItem : class;
    }
}
