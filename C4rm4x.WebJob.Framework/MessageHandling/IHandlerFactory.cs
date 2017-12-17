#region Using


#endregion

namespace C4rm4x.WebJob.Framework.MessageHandling
{
    /// <summary>
    /// Service responsible to retrieve the instance that implemements
    /// IHandler for the specified type of message
    /// </summary>
    public interface IHandlerFactory
    {
        /// <summary>
        /// Gets the handler for the specified message type.
        /// </summary>
        IHandler<TMessage> GetHandler<TMessage>();
    }
}
