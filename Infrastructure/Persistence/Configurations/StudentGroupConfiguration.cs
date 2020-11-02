using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SM.Domain.Entities;

namespace SM.Infrastructure.Persistence.Configurations
{
    public class StudentGroupConfiguration : IEntityTypeConfiguration<StudentGroup>
    {
        public void Configure(EntityTypeBuilder<StudentGroup> builder)
        {
            builder.HasKey(t => new { t.StudentId, t.GroupId });

            builder.HasOne(sc => sc.Student)
                .WithMany(s => s.StudentGroups)
                .HasForeignKey(sc => sc.StudentId);

            builder.HasOne(sc => sc.Group)
                .WithMany(g => g.StudentGroups)
                .HasForeignKey(sc => sc.GroupId);
        }
    }
}
