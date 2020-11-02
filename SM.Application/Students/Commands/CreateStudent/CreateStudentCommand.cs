using MediatR;
using SM.Application.Common.Interfaces;
using SM.Application.Common.Models;
using SM.Domain.Entities;
using SM.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Application.Students.Commands.CreateStudent
{
    public class CreateStudentCommand : IRequest<Response<long>>
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string PublicId { get; set; }

        public Genders? Gender { get; set; }

        public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, Response<long>>
        {
            private readonly IApplicationDbContext _context;

            public CreateStudentCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Response<long>> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
            {
                var entity = new Student
                {
                    FirstName = request.FirstName.Trim(),
                    MiddleName = request.MiddleName.Trim(),
                    LastName = request.LastName?.Trim(),
                    Gender = request.Gender.Value,
                    PublicId = request.PublicId
                };

                _context.Students.Add(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return new Response<long>(entity.Id);
            }
        }
    }
}
