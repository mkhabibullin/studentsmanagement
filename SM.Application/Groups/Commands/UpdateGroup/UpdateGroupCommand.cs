using MediatR;
using SM.Application.Common.Exceptions;
using SM.Application.Common.Interfaces;
using SM.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Application.Groups.Commands.UpdateGroup
{
    public class UpdateGroupCommand : IRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand>
        {
            private readonly IApplicationDbContext _context;

            public UpdateGroupCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
            {
                var entity = await _context.Groups.FindAsync(request.Id);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(Group), request.Id);
                }

                entity.Name = request.Name.Trim();

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
