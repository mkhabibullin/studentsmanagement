using MediatR;
using Microsoft.EntityFrameworkCore;
using SM.Application.Common.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Application.Groups.Commands.AddStudents
{
    public class DeleteStudentsCommand : IRequest
    {
        public int GroupId { get; set; }

        public long[] StudentIds { get; set; }

        public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentsCommand>
        {
            private readonly IApplicationDbContext _context;

            public DeleteStudentCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteStudentsCommand request, CancellationToken cancellationToken)
            {
                var students = await _context.StudentGroup
                    .Where(sg => sg.GroupId == request.GroupId)
                    .Where(sg => request.StudentIds.Contains(sg.StudentId))
                    .ToArrayAsync();

                _context.StudentGroup.RemoveRange(students);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
