using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using SM.Application.Common.Extensions;
using SM.Application.Common.Interfaces;
using SM.Application.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Application.Students.Queries
{
    public class GetStudentsQuery : PagingQuery, IRequest<PagedResponse<IEnumerable<StudentDto>>>
    {
        public class GetStudentsQueryHandler : IRequestHandler<GetStudentsQuery, PagedResponse<IEnumerable<StudentDto>>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly ISieveProcessor _sieveProcessor;

            public GetStudentsQueryHandler(IApplicationDbContext context, IMapper mapper, ISieveProcessor sieveProcessor)
            {
                _context = context;
                _mapper = mapper;
                _sieveProcessor = sieveProcessor;
            }

            public async Task<PagedResponse<IEnumerable<StudentDto>>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
            {
                var query = _context.Students
                    .ProjectTo<StudentDto>(_mapper.ConfigurationProvider);

                var filteredQuery = _sieveProcessor.Apply(request, query, applyPagination: false);

                var data = await filteredQuery
                    .Page(request)
                    .ToListAsync(cancellationToken);

                var total = await query.CountAsync(cancellationToken);

                return new PagedResponse<IEnumerable<StudentDto>>(data, request.Page, request.PageSize, total);
            }
        }
    }
}
