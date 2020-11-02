using AutoMapper;
using Shouldly;
using Sieve.Services;
using SM.Application.Common.Models;
using SM.Application.Students.Queries;
using SM.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SM.Application.UnitTests.Groups.Queries
{
    [Collection("QueryTests")]
    public class GetStudentsQueryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISieveProcessor _sieveProcessor;

        public GetStudentsQueryTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
            _sieveProcessor = fixture.SieveProcessor;
        }

        [Fact]
        public async Task Handle_ReturnsCorrectResponseAndListCount()
        {
            var query = new GetStudentsQuery();

            var handler = new GetStudentsQuery.GetStudentsQueryHandler(_context, _mapper, _sieveProcessor);

            var result = await handler.Handle(query, CancellationToken.None);

            result.ShouldBeOfType<PagedResponse<IEnumerable<StudentDto>>>();
            result.Data.Count().ShouldBe(2);

            var first = result.Data.First();

            first.Groups.Count().ShouldBe(2);
        }
    }
}
