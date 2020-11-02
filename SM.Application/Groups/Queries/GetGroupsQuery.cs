using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using SM.Application.Common.Extensions;
using SM.Application.Common.Interfaces;
using SM.Application.Common.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Application.Groups.Queries
{
    public class GetGroupsQuery : PagingQuery, IRequest<PagedResponse<IEnumerable<GroupDto>>>
    {
        public class GetGroupsQueryHandler : IRequestHandler<GetGroupsQuery, PagedResponse<IEnumerable<GroupDto>>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly ISieveProcessor _sieveProcessor;

            public GetGroupsQueryHandler(IApplicationDbContext context, IMapper mapper, ISieveProcessor sieveProcessor)
            {
                _context = context;
                _mapper = mapper;
                _sieveProcessor = sieveProcessor;
            }

            public async Task<PagedResponse<IEnumerable<GroupDto>>> Handle(GetGroupsQuery request, CancellationToken cancellationToken)
            {
                var query = _context.Groups
                    .ProjectTo<GroupDto>(_mapper.ConfigurationProvider);

                var filteredQuery = _sieveProcessor.Apply(request, query, applyPagination: false);

                var data = await filteredQuery
                    .Page(request)
                    .ToListAsync(cancellationToken);

                var total = await filteredQuery.CountAsync(cancellationToken);

                return new PagedResponse<IEnumerable<GroupDto>>(data, request.Page, request.PageSize, total);
            }
        }
    }
}
