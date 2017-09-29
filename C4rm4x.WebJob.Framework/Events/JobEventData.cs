#region Using

using System;

#endregion

namespace C4rm4x.WebJob.Framework
{
    /// <summary>
    /// Base class for all EventData related to IEventHandler
    /// </summary>
    public abstract class JobEventData
    {
        /// <summary>
        /// Gets the id of aggregate related root
        /// </summary>
        public Guid Id { get; internal set; }

        /// <summary>
        /// Gets the version
        /// </summary>
        public int Version { get; internal set; }

        /// <summary>
        /// Gets the timestamp 
        /// </summary>
        public DateTime TimeStamp { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public JobEventData()
        {
            TimeStamp = DateTime.UtcNow;
        }
    }
}
