using AutoMapper;
using MediatR;
using SM.Application.Common.Exceptions;
using SM.Application.Common.Interfaces;
using SM.Application.Common.Models;
using SM.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Application.Groups.Queries.GetGroup
{
    public class GetGroupByIdQuery : IRequest<Response<GroupDto>>
    {
        public int Id { get; set; }

        public class GetGroupByIdQueryHandler : IRequestHandler<GetGroupByIdQuery, Response<GroupDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetGroupByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<GroupDto>> Handle(GetGroupByIdQuery query, CancellationToken cancellationToken)
            {
                var group = await _context.Groups.FindAsync(query.Id);

                if (group == null)
                {
                    throw new NotFoundException(nameof(Group), query.Id);
                }

                return new Response<GroupDto>(_mapper.Map<GroupDto>(group));
            }
        }
    }
}
