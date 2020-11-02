using Shouldly;
using SM.Application.Students.Commands.CreateStudent;
using SM.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SM.Application.UnitTests.Students.Commands.CreateStudent
{
    public class CreateStudentCommandTests : CommandTestBase
    {
        [Fact]
        public async Task Handle_ShouldPersistGroup()
        {
            var command = new CreateStudentCommand
            {
                FirstName = "Special",
                MiddleName = "Special",
                Gender = Genders.Male
            };

            var handler = new CreateStudentCommand.CreateStudentCommandHandler(Context);

            var result = await handler.Handle(command, CancellationToken.None);

            var entity = Context.Students.Find(result.Data);

            entity.ShouldNotBeNull();
            entity.FirstName.ShouldBe(command.FirstName);
        }
    }
}
