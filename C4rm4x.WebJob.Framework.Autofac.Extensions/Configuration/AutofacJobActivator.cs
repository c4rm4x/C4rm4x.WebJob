#region Using

using Autofac;
using C4rm4x.Tools.Utilities;
using Microsoft.Azure.WebJobs.Host;

#endregion

namespace C4rm4x.WebJob.Framework.Autofac.Configuration
{
    /// <summary>
    /// Defines an activator that creates an instance of a job type.
    /// </summary>
    public class AutofacJobActivator : IJobActivator
    {
        private readonly IComponentContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The Autofac context</param>
        public AutofacJobActivator(IComponentContext context)
        {
            context.NotNull(nameof(context));

            _context = context;
        }

        /// <summary>
        /// Creates a new instance of a job type.
        /// </summary>
        /// <typeparam name="T">The job type.</typeparam>
        /// <returns>A new instance of the job type.</returns>
        public T CreateInstance<T>()
        {
            return _context.Resolve<T>();
        }
    }
}