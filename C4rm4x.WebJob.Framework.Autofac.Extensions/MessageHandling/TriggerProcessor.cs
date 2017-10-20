#region Using

using Autofac;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebJob.Framework.MessageHandling;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebJob.Framework.Autofac.MessageHandling
{
    /// <summary>
    /// Service responsible to process each message that triggers the job
    /// </summary>
    public interface ITriggerProcessor
    {
        /// <summary>
        /// Process each message that triggers the job
        /// </summary>
        Task ProcessAsync<TMessage>(TMessage message);
    }

    /// <summary>
    /// Service responsible to process each message that triggers the job
    /// creating a new scope to isolate each message
    /// </summary>
    public class TriggerProcessor : ITriggerProcessor
    {
        private readonly ILifetimeScope _lifetimeScope;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lifetimeScope">The Lifetime Scope factory</param>
        public TriggerProcessor(ILifetimeScope lifetimeScope)
        {
            lifetimeScope.NotNull(nameof(lifetimeScope));

            _lifetimeScope = lifetimeScope;
        }

        /// <summary>
        /// Process each message that triggers the job 
        /// by creating a single scope for each of them
        /// </summary>
        public virtual async Task ProcessAsync<TMessage>(TMessage message)
        {
            message.NotNull(nameof(message));

            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var shell = scope.Resolve<IHandlerShell>();

                await shell.HandleAsync(message);
            }
        }
    }
}
