using Shouldly;
using SM.Application.Common.Exceptions;
using SM.Application.Groups.Commands.DeleteGroup;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SM.Application.UnitTests.Groups.Commands.CreateGroup
{
    public class DeleteGroupCommandTests : CommandTestBase
    {
        [Fact]
        public async Task Handle_GivenValidId_ShouldMarkPersistedGroupAsIsDeleted()
        {
            var command = new DeleteGroupCommand
            {
                Id = 1
            };

            var handler = new DeleteGroupCommand.DeleteGroupCommandHandler(Context);

            await handler.Handle(command, CancellationToken.None);

            var entity = Context.Groups.Find(command.Id);

            entity.ShouldNotBeNull();
            entity.IsDeleted.ShouldBeTrue();
        }

        [Fact]
        public void Handle_GivenInvalidId_ThrowsException()
        {
            var command = new DeleteGroupCommand
            {
                Id = 99
            };

            var handler = new DeleteGroupCommand.DeleteGroupCommandHandler(Context);

            Should.ThrowAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));
        }
    }
}
