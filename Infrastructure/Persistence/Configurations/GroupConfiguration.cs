using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SM.Domain.Entities;

namespace SM.Infrastructure.Persistence.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.Property(t => t.Name)
                .HasMaxLength(25)
                .IsRequired();

            builder
                .HasIndex(t => t.Name)
                .IsUnique();
        }
    }
}
