using Shouldly;
using SM.Application.Common.Exceptions;
using SM.Application.Groups.Commands.DeleteGroup;
using SM.Application.Students.Commands.DeleteStudent;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SM.Application.UnitTests.Students.Commands.CreateStudent
{
    public class DeleteStudentCommandTests : CommandTestBase
    {
        [Fact]
        public async Task Handle_GivenValidId_ShouldMarkPersistedStudentAsIsDeleted()
        {
            var command = new DeleteStudentCommand
            {
                Id = 1
            };

            var handler = new DeleteStudentCommand.DeleteStudentCommandHandler(Context);

            await handler.Handle(command, CancellationToken.None);

            var entity = Context.Students.Find(command.Id);

            entity.ShouldNotBeNull();
            entity.IsDeleted.ShouldBeTrue();
        }

        [Fact]
        public void Handle_GivenInvalidId_ThrowsException()
        {
            var command = new DeleteStudentCommand
            {
                Id = 99
            };

            var handler = new DeleteStudentCommand.DeleteStudentCommandHandler(Context);

            Should.ThrowAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));
        }
    }
}
