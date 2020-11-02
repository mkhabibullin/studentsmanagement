using MediatR;
using SM.Application.Common.Exceptions;
using SM.Application.Common.Interfaces;
using SM.Domain.Entities;
using SM.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Application.Students.Commands.UpdateStudent
{
    public class UpdateStudentCommand : IRequest
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string PublicId { get; set; }

        public Genders? Gender { get; set; }

        public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand>
        {
            private readonly IApplicationDbContext _context;

            public UpdateStudentCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
            {
                var entity = await _context.Students.FindAsync(request.Id);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(Student), request.Id);
                }

                entity.FirstName = request.FirstName.Trim();
                entity.MiddleName = request.MiddleName.Trim();
                entity.LastName = request.LastName?.Trim();
                entity.PublicId = request.PublicId?.Trim();
                entity.Gender = request.Gender.Value;

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
