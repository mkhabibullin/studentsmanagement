using MediatR;
using SM.Application.Common.Exceptions;
using SM.Application.Common.Interfaces;
using SM.Domain.Entities;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Application.Students.Commands.AddGroups
{
    public class AddGroupsCommand : IRequest
    {
        public long StudentId { get; set; }

        public int[] GroupIds { get; set; }

        public class AddStudentCommandHandler : IRequestHandler<AddGroupsCommand>
        {
            private readonly IApplicationDbContext _context;

            public AddStudentCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(AddGroupsCommand request, CancellationToken cancellationToken)
            {
                var student = await _context.Students.FindAsync(request.StudentId);

                if (student == null)
                {
                    throw new NotFoundException(nameof(Student), request.StudentId);
                }

                foreach (var groupId in request.GroupIds)
                {
                    var group = await _context.Groups.FindAsync(groupId);

                    if (group == null)
                    {
                        throw new NotFoundException(nameof(Group), groupId);
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
