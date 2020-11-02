using Shouldly;
using SM.Application.Common.Exceptions;
using SM.Application.Groups.Commands.UpdateGroup;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SM.Application.UnitTests.Groups.Commands.UpdateGroup
{
    public class UpdateGroupCommandTests : CommandTestBase
    {
        [Fact]
        public async Task Handle_GivenValidId_ShouldUpdatePersistedGroup()
        {
            var command = new UpdateGroupCommand
            {
                Id = 1,
                Name = "New Name"
            };

            var handler = new UpdateGroupCommand.UpdateGroupCommandHandler(Context);

            await handler.Handle(command, CancellationToken.None);

            var entity = Context.Groups.Find(command.Id);

            entity.ShouldNotBeNull();
            entity.Name.ShouldBe(command.Name);
        }

        [Fact]
        public void Handle_GivenInvalidId_ThrowsException()
        {
            var command = new UpdateGroupCommand
            {
                Id = 99,
                Name = "This item doesn't exist."
            };

            var sut = new UpdateGroupCommand.UpdateGroupCommandHandler(Context);

            Should.ThrowAsync<NotFoundException>(() =>
                sut.Handle(command, CancellationToken.None));
        }
    }
}
