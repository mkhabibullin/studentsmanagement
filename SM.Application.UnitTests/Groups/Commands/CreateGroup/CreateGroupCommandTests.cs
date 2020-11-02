using Shouldly;
using SM.Application.Groups.Commands.CreateGroup;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SM.Application.UnitTests.Groups.Commands.CreateGroup
{
    public class CreateGroupCommandTests : CommandTestBase
    {
        [Fact]
        public async Task Handle_ShouldPersistGroup()
        {
            var command = new CreateGroupCommand
            {
                Name = "Special"
            };

            var handler = new CreateGroupCommand.CreateGroupCommandHandler(Context);

            var result = await handler.Handle(command, CancellationToken.None);

            var entity = Context.Groups.Find(result.Data);

            entity.ShouldNotBeNull();
            entity.Name.ShouldBe(command.Name);
        }
    }
}
