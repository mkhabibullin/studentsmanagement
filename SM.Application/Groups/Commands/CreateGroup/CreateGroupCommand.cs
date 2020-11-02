using MediatR;
using SM.Application.Common.Interfaces;
using SM.Application.Common.Models;
using SM.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Application.Groups.Commands.CreateGroup
{
    public class CreateGroupCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }

        public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Response<int>>
        {
            private readonly IApplicationDbContext _context;

            public CreateGroupCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Response<int>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
            {
                var entity = new Group
                { 
                    Name = request.Name
                };

                _context.Groups.Add(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return new Response<int>(entity.Id);
            }
        }
    }
}
