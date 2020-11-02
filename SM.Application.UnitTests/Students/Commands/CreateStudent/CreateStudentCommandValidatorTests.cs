using Shouldly;
using SM.Application.Students.Commands.CreateStudent;
using SM.Domain.Entities;
using SM.Domain.Enums;
using Xunit;

namespace SM.Application.UnitTests.Students.Commands.CreateStudent
{
    public class CreateStudentCommandValidatorTests : CommandTestBase
    {
        [Fact]
        public void IsValid_ShouldBeTrue_WhenPublicIsUnique()
        {
            var command = new CreateStudentCommand
            {
                FirstName = "First",
                MiddleName = "First",
                Gender = Genders.Male,
                PublicId = "Unique"
            };

            var validator = new CreateStudentCommandValidator(Context);

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(true);
        }

        [Fact]
        public void IsValid_ShouldBeFalse_WhenPublicIdIsNotUnique()
        {
            Context.Students.Add(new Student
            {
                FirstName = "First",
                MiddleName = "First",
                Gender = Genders.Male,
                PublicId = "Not unique"
            });
            Context.SaveChanges();

            var command = new CreateStudentCommand
            {
                FirstName = "Second",
                MiddleName = "Second",
                Gender = Genders.Male,
                PublicId = "Not unique"
            };

            var validator = new CreateStudentCommandValidator(Context);

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_ShouldBeFalse_WhenPublicIdExcedes16characters()
        {
            var command = new CreateStudentCommand
            {
                FirstName = "Second",
                MiddleName = "Second",
                Gender = Genders.Male,
                PublicId = "Some Large Id that exceeds max length"
            };

            var validator = new CreateStudentCommandValidator(Context);

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_ShouldBeFalse_WhenPublicIdLessThen6characters()
        {
            var command = new CreateStudentCommand
            {
                FirstName = "Second",
                MiddleName = "Second",
                Gender = Genders.Male,
                PublicId = "small"
            };

            var validator = new CreateStudentCommandValidator(Context);

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_ShouldBeFalse_WhenFirstNameExceeds40characters()
        {
            var command = new CreateStudentCommand
            {
                FirstName = "Large Fisrt Name Large Fisrt Name Large Fisrt Name2",
                MiddleName = "Second",
                Gender = Genders.Male,
                PublicId = "Second"
            };

            var validator = new CreateStudentCommandValidator(Context);

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }
    }
}
