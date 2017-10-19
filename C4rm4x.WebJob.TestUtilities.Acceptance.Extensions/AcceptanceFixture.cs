#region Using

using C4rm4x.Tools.TestUtilities.Bdd;
using C4rm4x.Tools.TestUtilities.EF;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebJob.TestUtilities.Acceptance.Internals;
using Microsoft.Azure.WebJobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using SimpleInjector.Extensions.LifetimeScoping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Reflection;
using TestContext = C4rm4x.Tools.TestUtilities.TestContext;

#endregion

namespace C4rm4x.WebJob.TestUtilities.Acceptance
{
    /// <summary>
    /// Base test class for acceptance tests
    /// </summary>
    [TestClass]
    public abstract class AcceptanceFixture
    {
        private Container _container;

        private Scope _scope;

        private GivenWhenThen _givenWhenThen;

        private readonly Action<JobHostConfiguration> _configurator;

        /// <summary>
        /// The test context
        /// </summary>
        protected TestContext Context { get; private set; } = new TestContext();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configurator">JobHost configurator</param>
        public AcceptanceFixture(Action<JobHostConfiguration> configurator)
        {
            configurator.NotNull(nameof(configurator));

            _configurator = configurator;
        }

        /// <summary>
        /// Initialises the test class
        /// </summary>
        [TestInitialize]
        public virtual void Setup()
        {
            SetupContainer();
            SetupJobHost();
        }

        private void SetupContainer()
        {
            _container = new Container();

            RegisterDependencies(_container, new LifetimeScopeLifestyle()); // Registers the rest

            _container.Verify();

            _scope = _container.BeginLifetimeScope(); // Starts container life time scope
        }


        private void SetupJobHost()
        {
            JobHost.Configure(_configurator);
        }

        /// <summary>
        /// Finalises the test class
        /// </summary>
        [TestCleanup]
        public virtual void Cleanup()
        {
            _givenWhenThen = null;

            _scope.Dispose(); // Enforce to dispose all the components

            Context.Cleanup();
        }

        /// <summary>
        /// Registers all the dependencies for this acceptance test
        /// </summary>
        /// <param name="container">The container</param>
        /// <param name="lifeStyle">The life style that specifies how the return instance will be cached</param>
        protected virtual void RegisterDependencies(
            Container container,
            Lifestyle lifeStyle)
        {
            container.Register<InMemoryJobHost>(lifeStyle);
        }

        /// <summary>
        /// Starts a new test plan with the initial given step
        /// </summary>
        /// <param name="step">The initial given step</param>
        protected IGivenDefinition Given(GivenHandler step)
        {
            _givenWhenThen = GivenWhenThen.StartWith(step);

            return _givenWhenThen;
        }

        /// <summary>
        /// Starts a new test plan with the initial when step
        /// </summary>
        /// <param name="step">Then initial when step</param>
        protected IWhenDefinition When(WhenHandler step)
        {
            _givenWhenThen = GivenWhenThen.StartWith(step);

            return _givenWhenThen;
        }

        /// <summary>Calls a job method.</summary>
        /// <param name="method">The job method to call.</param>
        /// <param name="arguments">
        /// An object with public properties representing argument names and values to bind to parameters in the job
        /// method. In addition to parameter values, these may also include binding data values.
        /// </param>
        protected void Invoke(MethodInfo method, object arguments)
        {
            JobHost.Call(method, arguments);
        }

        /// <summary>Calls a job method.</summary>
        /// <param name="method">The job method to call.</param>
        /// <param name="arguments">The argument names and values to bind to parameters in the job method.
        /// In addition to parameter values, these may also include binding data values. </param>
        protected void Invoke(MethodInfo method, IDictionary<string, object> arguments)
        {
            JobHost.Call(method, arguments);
        }

        /// <summary>
        /// Gets an instance of the specified type
        /// </summary>
        /// <typeparam name="TService">Type of instance</typeparam>
        /// <returns>An instance of specified type</returns>
        /// <exception cref="SimpleInjector.ActivationException">Thrown when there are errors resolving the service instance</exception>
        protected TService GetInstance<TService>()
            where TService : class
        {
            return _container.GetInstance<TService>();
        }

        private InMemoryJobHost JobHost
        {
            get { return GetInstance<InMemoryJobHost>(); }
        }
    }


    /// <summary>
    /// Base test class for acceptance tests using Entity Framework for access to the database
    /// </summary>
    /// <typeparam name="TContext">Type of DbContext</typeparam>
    [TestClass]
    public abstract class AcceptanceFixture<TContext> :
        AcceptanceFixture
        where TContext : DbContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configurator">Http configurator</param>
        public AcceptanceFixture(Action<JobHostConfiguration> configurator)
            : base(configurator)
        { }

        /// <summary>
        /// Finalises the test class
        /// </summary>
        [TestCleanup]
        public override void Cleanup()
        {
            EntityManager.Restore();

            base.Cleanup();
        }

        /// <summary>
        /// Registers all the dependencies for this acceptance test
        /// </summary>
        /// <param name="container">The container</param>
        /// <param name="lifeStyle">The life style that specifies how the return instance will be cached</param>
        protected override void RegisterDependencies(
            Container container,
            Lifestyle lifeStyle)
        {
            container.Register<EntityManager<TContext>>(lifeStyle);
            container.Register<TContext>(lifeStyle);

            base.RegisterDependencies(container, lifeStyle);
        }

        /// <summary>
        /// Adds a new entity to the db context
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity</typeparam>
        /// <param name="entity">Entity to add</param>
        /// <param name="saveContext">Indicates whether the entity must be saved in the context</param>
        protected void AddEntityToContext<TEntity>(
            TEntity entity,
            bool saveContext = false)
            where TEntity : class
        {
            EntityManager.Add(entity);

            if (saveContext)
                SaveContext();
        }

        /// <summary>
        /// Saves all the entities within the context into the database
        /// </summary>
        protected void SaveContext()
        {
            EntityManager.SaveAllChanges();
        }

        private EntityManager<TContext> EntityManager
        {
            get { return GetInstance<EntityManager<TContext>>(); }
        }
    }
}
