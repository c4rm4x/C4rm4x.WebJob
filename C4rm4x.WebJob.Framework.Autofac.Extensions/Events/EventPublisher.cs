#region Using

using Autofac;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebJob.Framework.Events;
using System;
using System.Collections;

#endregion

namespace C4rm4x.WebJob.Framework.Autofac.Events
{
    /// <summary>
    /// Implementation of IEventPublisher using Autofac container
    /// </summary>
    public class EventPublisher :
        AbstractEventPublisher, IEventPublisher
    {
        private readonly IComponentContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The Autofac context</param>
        public EventPublisher(IComponentContext context)
        {
            context.NotNull(nameof(context));

            _context = context;
        }

        /// <summary>
        /// Get all the registered handlers for the given type
        /// </summary>
        /// <param name="type">The type of the event to be processed</param>
        /// <returns>The collection of registered handlers for the given type</returns>
        protected override IEnumerable GetHandlers(Type type)
        {
            return _context.ResolveAll(typeof(IEventHandler<>).MakeGenericType(type)) ?? new object[] { };
        }
    }
}
