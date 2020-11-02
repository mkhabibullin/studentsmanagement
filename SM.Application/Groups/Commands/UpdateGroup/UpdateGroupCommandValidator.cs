using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SM.Application.Common.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Application.Groups.Commands.UpdateGroup
{
    public class UpdateGroupCommandValidator : AbstractValidator<UpdateGroupCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateGroupCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Name.Trim())
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(25).WithMessage("Name must not exceed 25 characters")
                .MustAsync(BeUniqueName).WithMessage("The specified name already exists");
        }

        public async Task<bool> BeUniqueName(UpdateGroupCommand model, string title, CancellationToken cancellationToken)
        {
            return await _context.Groups
                .Where(l => l.Id != model.Id)
                .AllAsync(l => l.Name.ToLower() != title.ToLower(), cancellationToken);
        }
    }
}
