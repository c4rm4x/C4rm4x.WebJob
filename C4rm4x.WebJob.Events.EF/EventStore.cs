using C4rm4x.Tools.Utilities;
using C4rm4x.WebJob.Events.EF.Infrastructure;
using C4rm4x.WebJob.Framework;
using C4rm4x.WebJob.Framework.Events;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace C4rm4x.WebJob.Events.EF
{
    /// <summary>
    /// Implementation of IEventStore using EF
    /// </summary>
    public class EventStore : IEventStore
    {
        protected EventStoreContext Context { get; private set; }

        /// <summary>
        /// Gets the serializer settings
        /// </summary>
        public static JsonSerializerSettings SerializerSettings { get; private set; } =
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new ApiEventDataSerializerContractResolver(EventStorePolicy.TypesToIgnore),
            };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The event store context</param>
        public EventStore(EventStoreContext context)
        {
            context.NotNull(nameof(context));

            Context = context;
        }

        /// <summary>
        /// Saves all the events
        /// </summary>
        /// <param name="events">The events raised to be saved</param>     
        public Task SaveAllAsync(IEnumerable<JobEventData> events)
        {
            Context.Events.AddRange(events.Select(CreateEvent));

            return Context.SaveChangesAsync();
        }

        private Event CreateEvent(JobEventData @event)
        {
            return Event.Create(@event, GetPayload(@event));
        }

        private static string GetPayload(JobEventData eventData)
        {
            return EventStorePolicy.IsSensitive(eventData)
                ? null
                : JsonConvert.SerializeObject(eventData, Formatting.Indented, SerializerSettings);
        }
    }
}
