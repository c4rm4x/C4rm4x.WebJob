#region Using

using Autofac;
using C4rm4x.Tools.Utilities;
using Microsoft.Azure.WebJobs;

#endregion

namespace C4rm4x.WebJob.Framework.Autofac.Configuration
{
    /// <summary>
    /// JobHostConfiguration extensions
    /// </summary>
    public static class JobHostConfigurationExtensions
    {
        /// <summary>
        /// Sets config JobActivator to use AutofacJobActivator instance
        /// </summary>
        /// <param name="config">The job host configuration</param>
        /// <param name="context">The Autofac context</param>
        public static void UseAutofacActivator(
            this JobHostConfiguration config, IComponentContext context)
        {
            config.NotNull(nameof(config));
            context.NotNull(nameof(context));

            config.JobActivator = new AutofacJobActivator(context);
        }
    }
}
