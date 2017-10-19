#region Using

using C4rm4x.Tools.Utilities;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Reflection;

#endregion

namespace C4rm4x.WebJob.TestUtilities.Acceptance.Internals
{
    /// <summary>
    /// Service responsible to manage all the interaction between the acceptance tests and in-memory job host
    /// </summary>
    internal class InMemoryJobHost : IDisposable
    {
        private JobHost _host;

        /// <summary>
        /// Gets whether or not this instance has already been disposed
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Configure the in memory job host with the given configuration
        /// </summary>
        public void Configure(Action<JobHostConfiguration> configurator)
        {
            configurator.NotNull(nameof(configurator));

            var config = new JobHostConfiguration();

            configurator(config);

            _host = new JobHost(config);

            _host.Start();
        }

        /// <summary>Calls a job method.</summary>
        /// <param name="method">The job method to call.</param>
        /// <param name="arguments">
        /// An object with public properties representing argument names and values to bind to parameters in the job
        /// method. In addition to parameter values, these may also include binding data values.
        /// </param>
        public void Call(MethodInfo method, object arguments)
        {
            _host.Call(method, arguments);
        }

        /// <summary>Calls a job method.</summary>
        /// <param name="method">The job method to call.</param>
        /// <param name="arguments">The argument names and values to bind to parameters in the job method.
        /// In addition to parameter values, these may also include binding data values. </param>
        public void Call(MethodInfo method, IDictionary<string, object> arguments)
        {
            _host.Call(method, arguments);
        }

        /// <summary>
        /// Releases all the managed resources
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed) return;

            if (_host.IsNotNull())
            {
                _host.Stop();
                _host.Dispose();
            }

            GC.SuppressFinalize(this);

            IsDisposed = true;
        }
    }
}
