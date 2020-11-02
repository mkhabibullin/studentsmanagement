using Shouldly;
using SM.Application.Common.Exceptions;
using SM.Application.Students.Commands.UpdateStudent;
using SM.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SM.Application.UnitTests.Groups.Commands.UpdateGroup
{
    public class UpdateStudentCommandTests : CommandTestBase
    {
        [Fact]
        public async Task Handle_GivenValidId_ShouldUpdatePersistedStudent()
        {
            var command = new UpdateStudentCommand
            {
                Id = 1,
                FirstName = "NewName",
                MiddleName = "NewMiddleName",
                Gender = Genders.Male
            };

            var handler = new UpdateStudentCommand.UpdateStudentCommandHandler(Context);

            await handler.Handle(command, CancellationToken.None);

            var entity = Context.Students.Find(command.Id);

            entity.ShouldNotBeNull();
            entity.FirstName.ShouldBe(command.FirstName);
        }

        [Fact]
        public void Handle_GivenInvalidId_ThrowsException()
        {
            var command = new UpdateStudentCommand
            {
                Id = 99,
                FirstName = "NewName",
                MiddleName = "NewMiddleName",
                Gender = Genders.Male
            };

            var sut = new UpdateStudentCommand.UpdateStudentCommandHandler(Context);

            Should.ThrowAsync<NotFoundException>(() =>
                sut.Handle(command, CancellationToken.None));
        }
    }
}
