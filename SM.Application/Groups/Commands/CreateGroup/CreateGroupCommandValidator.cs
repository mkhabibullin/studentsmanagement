using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SM.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Application.Groups.Commands.CreateGroup
{
    public class CreateGroupCommandValidator : AbstractValidator<CreateGroupCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateGroupCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Name.Trim())
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(25).WithMessage("Name must not exceed 25 characters")
                .MustAsync(BeUniqueName).WithMessage("The specified name already exists");
        }

        public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            return await _context.Groups
                .AllAsync(l => l.Name.ToLower() != name.ToLower(), cancellationToken);
        }
    }
}
