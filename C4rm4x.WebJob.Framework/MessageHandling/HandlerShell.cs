#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebJob.Framework.Logging;
using System;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebJob.Framework.MessageHandling
{
    /// <summary>
    /// Service responsible to shell every message and perform common operations
    /// </summary>
    public interface IHandlerShell
    {
        /// <summary>
        /// Runs the code of the actual handler after all the common operations that are 
        /// performed for each message regardless of its type
        /// </summary>
        /// <typeparam name="TMessage">The message type</typeparam>
        /// <param name="message">The message to be handled</param>
        Task HandleAsync<TMessage>(TMessage message);
    }

    /// <summary>
    /// Implementation of IHandlerShell which performs nothing but executing the actual handler
    /// and handling possible exceptions
    /// </summary>
    public class HandlerShell : IHandlerShell
    {
        private readonly ILog _logger;

        private readonly IHandlerFactory _handlerFactory;

        public bool RethrowExceptions { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="handlerFactory">The message handler factory</param>
        /// <param name="rethrowExceptions">Indicates whether or not the exception should be rethrown</param>
        public HandlerShell(
            ILog logger,
            IHandlerFactory handlerFactory,
            bool rethrowExceptions = true)
        {
            logger.NotNull(nameof(logger));
            handlerFactory.NotNull(nameof(handlerFactory));

            _logger = logger;
            _handlerFactory = handlerFactory;

            RethrowExceptions = rethrowExceptions;
        }

        /// <summary>
        /// Runs the code of the actual handler after all the common operations that are 
        /// performed for each message regardless of its type
        /// </summary>
        /// <param name="message">The message to be handled</param>
        public async Task HandleAsync<TMessage>(TMessage message)
        {
            try
            {
                // Handles the message 
                await _handlerFactory.GetHandler<TMessage>().HandleAsync(message);
            }
            catch (AggregateException e)
            {
                HandleException(e.Flatten());
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        private void HandleException(Exception exceptionToHandle)
        {
            _logger.Error("Unknown exception", exceptionToHandle);

            if (RethrowExceptions) throw exceptionToHandle;
        }
    }
}
