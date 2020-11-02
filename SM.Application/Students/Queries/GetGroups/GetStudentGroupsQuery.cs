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
    public class GetStudentGroupsQuery : PagingQuery, IRequest<PagedResponse<IEnumerable<StudentGroupsListDto>>>
    {
        public long StudentId { internal get; set; }

        public class GetStudentGroupsQueryHandler : IRequestHandler<GetStudentGroupsQuery, PagedResponse<IEnumerable<StudentGroupsListDto>>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetStudentGroupsQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<PagedResponse<IEnumerable<StudentGroupsListDto>>> Handle(GetStudentGroupsQuery request, CancellationToken cancellationToken)
            {
                var query = _context.StudentGroup
                    .Where(sg => sg.StudentId == request.StudentId)
                    .ProjectTo<StudentGroupsListDto>(_mapper.ConfigurationProvider)
                    .OrderBy(s => s.Name);

                var data = await query
                    .Page(request)
                    .ToListAsync(cancellationToken);

                var total = await query.CountAsync(cancellationToken);

                return new PagedResponse<IEnumerable<StudentGroupsListDto>>(data, request.Page, request.PageSize, total);
            }
        }
    }
}
