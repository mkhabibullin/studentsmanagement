using MediatR;
using Microsoft.EntityFrameworkCore;
using SM.Application.Common.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Application.Students.Commands.AddGroups
{
    public class DeleteGroupsCommand : IRequest
    {
        public long StudentId { get; set; }

        public int[] GroupIds { get; set; }

        public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupsCommand>
        {
            private readonly IApplicationDbContext _context;

            public DeleteGroupCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteGroupsCommand request, CancellationToken cancellationToken)
            {
                var groups = await _context.StudentGroup
                    .Where(sg => sg.StudentId == request.StudentId)
                    .Where(sg => request.GroupIds.Contains(sg.GroupId))
                    .ToArrayAsync();

                _context.StudentGroup.RemoveRange(groups);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
