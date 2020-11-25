using Shouldly;
using SM.Application.Students.Commands.CreateStudent;
using SM.Domain.Enums;
using Xunit;

namespace SM.Application.UnitTests.Students.Commands.CreateStudent
{
    public class CreateStudentCommandValidatorTests : CommandTestBase
    {
        [Fact]
        public void IsValid_ShouldBeFalse_WhenFirstNameExceeds40characters()
        {
            var command = new CreateStudentCommand
            {
                FirstName = "Large Fisrt Name Large Fisrt Name Large Fisrt Name2",
                MiddleName = "Second",
                Gender = Genders.Male
            };

            var validator = new CreateStudentCommandValidator(Context);

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }
    }
}
