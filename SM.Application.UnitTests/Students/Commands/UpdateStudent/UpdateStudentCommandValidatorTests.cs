using Shouldly;
using SM.Application.Groups.Commands.UpdateGroup;
using SM.Application.Students.Commands.UpdateStudent;
using SM.Domain.Entities;
using SM.Domain.Enums;
using Xunit;

namespace SM.Application.UnitTests.Groups.Commands.UpdateGroup
{
    public class UpdateStudentCommandValidatorTests : CommandTestBase
    {
        [Fact]
        public void IsValid_ShouldBeTrue_WhenPublicIdIsUnique()
        {
            var command = new UpdateStudentCommand
            {
                Id = 1,
                FirstName = "First",
                MiddleName = "First",
                PublicId = "Unique",
                Gender = Genders.Male
            };

            var validator = new UpdateStudentCommandValidator(Context);

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(true);
        }

        [Fact]
        public void IsValid_ShouldBeFalse_WhenPublicIdIsNotUnique()
        {
            Context.Students.Add(new Student {
                FirstName = "First",
                MiddleName = "First",
                PublicId = "Not unique",
                Gender = Genders.Male
            });
            Context.SaveChanges();

            var command = new UpdateStudentCommand
            {
                FirstName = "Second",
                MiddleName = "Second",
                PublicId = "Not unique",
                Gender = Genders.Male
            };

            var validator = new UpdateStudentCommandValidator(Context);

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_ShouldBeFalse_WhenPublicIdExcedes16characters()
        {
            var command = new UpdateStudentCommand
            {
                FirstName = "Second",
                MiddleName = "Second",
                PublicId = "Some Large Id that exceeds max length",
                Gender = Genders.Male
            };

            var validator = new UpdateStudentCommandValidator(Context);

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }
    }
}
