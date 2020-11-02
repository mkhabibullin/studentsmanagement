using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SM.Application.Common.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Application.Groups.Commands.AddStudents
{
    public class AddStudentsCommandValidator : AbstractValidator<AddStudentsCommand>
    {
        private readonly IApplicationDbContext _context;

        public AddStudentsCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.StudentIds.Distinct().ToArray())
                .NotEmpty().WithMessage("Students list is required");

            RuleForEach(v => v.StudentIds)
                .MustAsync(ShouldExist)
                .WithMessage((gs, studentId) => $"The student with ID {studentId} doesn't exist");

            RuleForEach(v => v.StudentIds)
                .MustAsync(BeUnique)
                .WithMessage((gs, studentId) => $"The student with ID {studentId} is already in the group");
        }

        public async Task<bool> BeUnique(AddStudentsCommand cmd, long studentId, CancellationToken cancellationToken)
        {
            return await _context.StudentGroup
                .Where(sg => sg.GroupId == cmd.GroupId)
                .AllAsync(sg => sg.StudentId != studentId, cancellationToken);
        }

        public async Task<bool> ShouldExist(long studentId, CancellationToken cancellationToken)
        {
            return await _context.Students.AnyAsync(s => s.Id == studentId, cancellationToken);
        }
    }
}
