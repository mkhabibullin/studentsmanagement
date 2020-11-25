using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.EntityFrameworkCoreIntegration.Mappings;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace SM.Infrastructure.StatePersistence
{
    public class StudentStateDbContext : SagaDbContext
    {
        public StudentStateDbContext(DbContextOptions<StudentStateDbContext> options)
            : base(options)
        {
        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get { yield return new StudentStateMap(); }
        }
    }
}
