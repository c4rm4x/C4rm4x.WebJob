﻿using C4rm4x.WebJob.Framework;

namespace C4rm4x.WebJob.Events.EF.Configuration
{
    /// <summary>
    /// Configures the EventStoreConfiguration using a fluid interface
    /// </summary>
    public class EventStoreConfigurationBuilder
    {
        private readonly EventStoreConfiguration _configuration;

        private EventStoreConfigurationBuilder()
        {
            _configuration = EventStoreConfiguration.Create();
        }

        /// <summary>
        /// Creates a new instance of EventStoreConfigurationBuilder
        /// </summary>
        /// <remarks>
        /// This is the entry point to start configuring a new EventStoreConfiguration
        /// </remarks>
        /// <returns>A new instance of EventStoreConfigurationBuilder</returns>
        public static EventStoreConfigurationBuilder Configure() => new EventStoreConfigurationBuilder();

        /// <summary>
        /// Flags events of type T as sensitive
        /// </summary>
        /// <typeparam name="T">Type of the event to be flagged as sensitive</typeparam>
        /// <returns>This builder</returns>
        public EventStoreConfigurationBuilder SensitivePayload<T>() where T : JobEventData
        {
            _configuration.SensitivePayload<T>();

            return this;
        }

        /// <summary>
        /// Flags given type should be ignored every time an event contains any property of this type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public EventStoreConfigurationBuilder ShouldIgnore<T>()
        {
            _configuration.ShouldIgnore<T>();

            return this;
        }

        /// <summary>
        /// Creates a new instance of EventStoreConfiguration with the configuration specified at this moment
        /// </summary>
        /// <returns>A new instance of EventStoreConfiguration configured with the specified rules</returns>
        public IEventStoreConfiguration Build() => _configuration;
    }
}
