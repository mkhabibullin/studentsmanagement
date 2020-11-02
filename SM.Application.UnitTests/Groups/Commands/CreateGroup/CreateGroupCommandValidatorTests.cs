using Shouldly;
using SM.Application.Groups.Commands.CreateGroup;
using SM.Domain.Entities;
using Xunit;

namespace SM.Application.UnitTests.Groups.Commands.CreateGroup
{
    public class CreateGroupCommandValidatorTests : CommandTestBase
    {
        [Fact]
        public void IsValid_ShouldBeTrue_WhenNameIsUnique()
        {
            var command = new CreateGroupCommand
            {
                Name = "Unique"
            };

            var validator = new CreateGroupCommandValidator(Context);

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(true);
        }

        [Fact]
        public void IsValid_ShouldBeFalse_WhenNameIsNotUnique()
        {
            Context.Groups.Add(new Group { Name = "Not unique" });
            Context.SaveChanges();

            var command = new CreateGroupCommand
            {
                Name = "Not unique"
            };

            var validator = new CreateGroupCommandValidator(Context);

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_ShouldBeFalse_WhenNameExcedes25characters()
        {
            var command = new CreateGroupCommand
            {
                Name = "Some Large Name that exceeds max length"
            };

            var validator = new CreateGroupCommandValidator(Context);

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }
    }
}
