using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SM.Domain.Entities;

namespace SM.Infrastructure.Persistence.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.Property(t => t.FirstName)
                .HasMaxLength(40)
                .IsRequired();

            builder.Property(t => t.MiddleName)
                .HasMaxLength(40)
                .IsRequired();

            builder.Property(t => t.LastName)
                .HasMaxLength(60);

            builder.Property(t => t.PublicId)
                .HasMaxLength(16);

            builder
                .HasIndex(t => t.PublicId)
                .IsUnique()
                .HasFilter("[PublicId] IS NOT NULL");
        }
    }
}
