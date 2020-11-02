using AutoMapper;
using MediatR;
using SM.Application.Common.Exceptions;
using SM.Application.Common.Interfaces;
using SM.Application.Common.Models;
using SM.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Application.Students.Queries.GetStudent
{
    public class GetStudentByIdQuery : IRequest<Response<StudentDto>>
    {
        public long Id { get; set; }

        public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, Response<StudentDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetStudentByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<StudentDto>> Handle(GetStudentByIdQuery query, CancellationToken cancellationToken)
            {
                var student = await _context.Students.FindAsync(query.Id);

                if (student == null)
                {
                    throw new NotFoundException(nameof(Student), query.Id);
                }

                return new Response<StudentDto>(_mapper.Map<StudentDto>(student));
            }
        }
    }
}
