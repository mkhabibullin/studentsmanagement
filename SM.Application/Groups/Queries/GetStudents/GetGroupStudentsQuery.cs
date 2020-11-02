using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SM.Application.Common.Extensions;
using SM.Application.Common.Interfaces;
using SM.Application.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Application.Groups.Queries
{
    public class GetGroupStudentsQuery : PagingQuery, IRequest<PagedResponse<IEnumerable<GroupStudentsListDto>>>
    {
        public int GroupId { internal get; set; }

        public class GetGroupStudentsQueryHandler : IRequestHandler<GetGroupStudentsQuery, PagedResponse<IEnumerable<GroupStudentsListDto>>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetGroupStudentsQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<PagedResponse<IEnumerable<GroupStudentsListDto>>> Handle(GetGroupStudentsQuery request, CancellationToken cancellationToken)
            {
                var query = _context.StudentGroup
                    .Where(sg => sg.GroupId == request.GroupId)
                    .ProjectTo<GroupStudentsListDto>(_mapper.ConfigurationProvider)
                    .OrderBy(s => s.MiddleName)
                    .ThenBy(s => s.FirstName);

                var data = await query
                    .Page(request)
                    .ToListAsync(cancellationToken);

                var total = await query.CountAsync(cancellationToken);

                return new PagedResponse<IEnumerable<GroupStudentsListDto>>(data, request.Page, request.PageSize, total);
            }
        }
    }
}
