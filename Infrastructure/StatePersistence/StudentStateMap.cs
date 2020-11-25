using MassTransit.EntityFrameworkCoreIntegration.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SM.Application.Saga.StudentRegistration;

namespace SM.Infrastructure.StatePersistence
{
    public class StudentStateMap : SagaClassMap<StudentRegistrationState>
    {
        protected override void Configure(EntityTypeBuilder<StudentRegistrationState> entity, ModelBuilder model)
        {
            entity.Property(x => x.CurrentState).HasMaxLength(64);
            entity.Property(x => x.StudentId);
            entity.Property(x => x.FullName);

            //// If using Optimistic concurrency, otherwise remove this property
            //entity.Property(x => x.RowVersion).IsRowVersion();
        }
    }
}
