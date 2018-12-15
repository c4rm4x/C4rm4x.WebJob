using C4rm4x.WebJob.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace C4rm4x.WebJob.Events.EF
{
    internal class ApiEventDataSerializerContractResolver : DefaultContractResolver
    {
        private static IEnumerable<string> Properties = new[]
        {
            nameof(JobEventData.Id),
            nameof(JobEventData.Version),
            nameof(JobEventData.TimeStamp)
        };

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (IsApiEventDataProperty(property.PropertyName))
            {
                property.ShouldSerialize = i => false;
                property.Ignored = true;
            }

            return property;
        }

        private static bool IsApiEventDataProperty(string propertyName) => Properties.Contains(propertyName);
    }
}
