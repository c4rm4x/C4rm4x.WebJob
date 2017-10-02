#region Using

using Autofac;
using C4rm4x.Tools.Utilities;
using System;
using System.Linq;
using System.Reflection;

#endregion

namespace C4rm4x.WebJob.Framework.Autofac
{
    /// <summary>
    /// Utilities methods to auto-register all the API objects for both,
    /// Autofac container builder and Multi tenant container
    /// </summary>
    public static class RegistrationExtensions
    {
        private static void RegisterTypeByAttribute<TAttr>(
               this ContainerBuilder container,
               params Assembly[] assemblies)
               where TAttr : Attribute
        {
            container.NotNull(nameof(container));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            container.RegisterAssemblyTypes(assemblies)
                .Where(t => t.GetCustomAttributes(false).Any(a => a.GetType() == typeof(TAttr)))
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();
        }

        /// <summary>
        /// Registers all public classes decorated with attribute DomainService within all specified assemblies
        /// </summary>
        /// <param name="container">The container</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterAllDomainServices(
            this ContainerBuilder container,
            params Assembly[] assemblies)
        {
            container.RegisterTypeByAttribute<DomainServiceAttribute>(assemblies);
        }

        /// <summary>
        /// Registers all public classes decorated with attribute Repository within all specified assemblies
        /// </summary>
        /// <param name="container">The container</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterAllRepositories(
            this ContainerBuilder container,
            params Assembly[] assemblies)
        {
            container.RegisterTypeByAttribute<RepositoryAttribute>(assemblies);
        }

        /// <summary>
        /// Registers all public classes decorated with attribute MessageHandler within all specified assemblies
        /// </summary>
        /// <param name="container">The container</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterAllMessageHandlers(
            this ContainerBuilder container,
            params Assembly[] assemblies)
        {
            container.RegisterTypeByAttribute<MessageHandlerAttribute>(assemblies);
        }

        /// <summary>
        /// Registers all public classes decorated with attribute EventHandler within all specified assemblies
        /// </summary>
        /// <param name="container">The container</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterAllEventHandlers(
            this ContainerBuilder container,
            params Assembly[] assemblies)
        {
            container.RegisterTypeByAttribute<EventHandlerAttribute>(assemblies);
        }

        /// <summary>
        /// Registers all public classes decorated with attributes DomainService, Repository, 
        /// MessageHandler and EventHandler within all specified assemblies
        /// </summary>
        /// <param name="container">The container</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterAll(
            this ContainerBuilder container,
            params Assembly[] assemblies)
        {
            container.NotNull(nameof(container));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            container.RegisterAllDomainServices(assemblies);
            container.RegisterAllRepositories(assemblies);
            container.RegisterAllMessageHandlers(assemblies);
            container.RegisterAllEventHandlers(assemblies);
        }

        /// <summary>
        /// Registers all modules that inherit from JobModule within all specified assemblies
        /// </summary>
        /// <param name="container">Autofac container builder</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterModules(
            this ContainerBuilder container,
            params Assembly[] assemblies)
        {
            container.NotNull(nameof(container));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            container.RegisterAssemblyModules<JobModule>(assemblies);
        }
    }
}
