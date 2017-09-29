#region Using

using Autofac;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebJob.Framework.MessageHandling;

#endregion

namespace C4rm4x.WebJob.Framework.Autofac.MessageHandling
{
    /// <summary>
    /// Implementation of IHandlerFactory using Autofac container
    /// </summary>
    public class HandlerFactory : IHandlerFactory
    {
        private readonly IComponentContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The Autofac context</param>
        public HandlerFactory(IComponentContext context)
        {
            context.NotNull(nameof(context));

            _context = context;
        }

        /// <summary>
        /// Retrieves the message handler for the specified type
        /// </summary>
        /// <typeparam name="TMessage">Type of the message</typeparam>
        /// <returns>The instance that implement IHandler for ths specified type</returns>
        public IHandler<TMessage> GetHandler<TMessage>()
        {
            return _context.Resolve<IHandler<TMessage>>();
        }
    }
}
