using SM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Student> Students { get; set; }

        DbSet<Group> Groups { get; set; }

        DbSet<StudentGroup> StudentGroup { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
