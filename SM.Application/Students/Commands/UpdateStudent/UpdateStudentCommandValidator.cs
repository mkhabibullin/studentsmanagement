using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SM.Application.Common.Interfaces;
using SM.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Application.Students.Commands.UpdateStudent
{
    public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateStudentCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.FirstName.Trim())
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(40).WithMessage("First name must not exceed 40 characters");

            RuleFor(v => v.MiddleName.Trim())
                .NotEmpty().WithMessage("Middle name is required")
                .MaximumLength(40).WithMessage("Middle name must not exceed 40 characters");

            RuleFor(v => v.Gender.HasValue ? v.Gender.Value : (Genders?)null)
                .NotNull().WithMessage("Gender is required");

            RuleFor(v => v.LastName != null ? v.LastName.Trim() : null)
                .MaximumLength(60).WithMessage("Last name must not exceed 60 characters");

            RuleFor(v => v.PublicId != null ? v.PublicId.Trim() : null)
                .MinimumLength(6).WithMessage("Public ID must exceed 6 characters")
                .MaximumLength(16).WithMessage("Public ID must not exceed 16 characters")
                .MustAsync(BeUniquePublicId).WithMessage("The specified public ID already exists"); ;
        }

        public async Task<bool> BeUniquePublicId(string publicId, CancellationToken cancellationToken)
        {
            if (publicId == null) return true;

            return await _context.Students
                .AllAsync(l => l.PublicId.ToLower() != publicId.ToLower(), cancellationToken);
        }
    }
}
