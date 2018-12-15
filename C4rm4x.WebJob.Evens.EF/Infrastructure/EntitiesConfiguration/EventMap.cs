using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace C4rm4x.WebJob.Evens.EF.Infrastructure.EntitiesConfiguration
{
    internal class EventMap : EntityTypeConfiguration<Event>
    {
        public EventMap()
        {
            ToTable("DomainEvents")
                .HasKey(e => e.EventID);

            Property(e => e.EventID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnName("EventID");
        }
    }
}
