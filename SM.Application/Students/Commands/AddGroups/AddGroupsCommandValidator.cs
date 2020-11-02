using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SM.Application.Common.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Application.Students.Commands.AddGroups
{
    public class AddGroupsCommandValidator : AbstractValidator<AddGroupsCommand>
    {
        private readonly IApplicationDbContext _context;

        public AddGroupsCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.GroupIds.Distinct())
                .NotEmpty().WithMessage("Groups list is required");

            RuleForEach(v => v.GroupIds)
                .MustAsync(ShouldExist)
                .WithMessage((gs, groupId) => $"The group with ID {groupId} doesn't exist");

            RuleForEach(v => v.GroupIds)
                .MustAsync(BeUnique).WithMessage((c, groupId) => $"The student is already included in the group with ID {groupId}");
        }

        public async Task<bool> BeUnique(AddGroupsCommand cmd, int groupId, CancellationToken cancellationToken)
        {
            return await _context.StudentGroup
                .Where(sg => sg.StudentId == cmd.StudentId)
                .AllAsync(sg => sg.GroupId != groupId, cancellationToken);
        }

        public async Task<bool> ShouldExist(int groupId, CancellationToken cancellationToken)
        {
            return await _context.Groups.AnyAsync(g => g.Id == groupId, cancellationToken);
        }
    }
}
