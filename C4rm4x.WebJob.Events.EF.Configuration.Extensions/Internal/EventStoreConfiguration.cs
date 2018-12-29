using C4rm4x.Tools.Utilities;
using C4rm4x.WebJob.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace C4rm4x.WebJob.Events.EF.Configuration
{
    internal class EventStoreConfiguration : IEventStoreConfiguration
    {
        public ICollection<Type> SensitiveEvents { get; private set; }

        public ICollection<Type> TypesToIgnore { get; private set; }

        private EventStoreConfiguration()
        {
            SensitiveEvents = new HashSet<Type>();
            TypesToIgnore = new HashSet<Type>();
        }

        public static EventStoreConfiguration Create() => new EventStoreConfiguration();

        public bool IsSensitive<T>(T eventData) where T : JobEventData
        {
            return IsSensitive(eventData.GetType());
        }

        public bool IsSensitive(Type eventDataType)
        {
            eventDataType.Is<JobEventData>();

            return SensitiveEvents.Contains(eventDataType);
        }

        IEnumerable<Type> IEventStoreConfiguration.TypesToIgnore => this.TypesToIgnore.AsEnumerable();


        internal EventStoreConfiguration SensitivePayload<T>() where T : JobEventData
        {
            return SensitivePayload(typeof(T));
        }

        internal EventStoreConfiguration SensitivePayload(Type eventDataType)
        {
            eventDataType.Is<JobEventData>();

            SensitiveEvents.Add(eventDataType);

            return this;
        }

        internal EventStoreConfiguration ShouldIgnore<T>()
        {
            return ShouldIgnore(typeof(T));
        }

        internal EventStoreConfiguration ShouldIgnore(Type typeToIgnore)
        {
            typeToIgnore.Must(t => !typeof(JobEventData).IsAssignableFrom(t),
                "{0} must not be compatible with {1}".AsFormat(
                    typeToIgnore.FullName, typeof(JobEventData).FullName));

            TypesToIgnore.Add(typeToIgnore);

            return this;
        }
    }
}
