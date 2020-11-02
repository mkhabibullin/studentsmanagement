using MediatR;
using Microsoft.EntityFrameworkCore;
using SM.Application.Common.Exceptions;
using SM.Application.Common.Interfaces;
using SM.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Application.Groups.Commands.DeleteGroup
{
    public class DeleteGroupCommand : IRequest
    {
        public int Id { get; set; }

        public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand>
        {
            private readonly IApplicationDbContext _context;

            public DeleteGroupCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
            {
                var entity = await _context.Groups
                    .Where(g => g.Id == request.Id)
                    .SingleOrDefaultAsync(cancellationToken);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(Group), request.Id);
                }

                _context.Groups.Remove(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
