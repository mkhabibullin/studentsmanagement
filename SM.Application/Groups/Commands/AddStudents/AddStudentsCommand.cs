using MediatR;
using SM.Application.Common.Exceptions;
using SM.Application.Common.Interfaces;
using SM.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Application.Groups.Commands.AddStudents
{
    public class AddStudentsCommand : IRequest
    {
        public int GroupId { get; set; }

        public long[] StudentIds { get; set; }

        public class AddStudentCommandHandler : IRequestHandler<AddStudentsCommand>
        {
            private readonly IApplicationDbContext _context;

            public AddStudentCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(AddStudentsCommand request, CancellationToken cancellationToken)
            {
                var group = await _context.Groups.FindAsync(request.GroupId);

                if (group == null)
                {
                    throw new NotFoundException(nameof(Group), request.GroupId);
                }

                foreach (var studentId in request.StudentIds)
                {
                    var student = await _context.Students.FindAsync(studentId);

                    if (student == null)
                    {
                        throw new NotFoundException(nameof(Student), studentId);
                    }

                    var entity = new StudentGroup
                    {
                        Group = group,
                        Student = student
                    };

                    _context.StudentGroup.Add(entity);
                }

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
