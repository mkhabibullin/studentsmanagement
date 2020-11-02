using Shouldly;
using SM.Application.Groups.Commands.UpdateGroup;
using SM.Domain.Entities;
using Xunit;

namespace SM.Application.UnitTests.Groups.Commands.UpdateGroup
{
    public class UpdateGroupCommandValidatorTests : CommandTestBase
    {
        [Fact]
        public void IsValid_ShouldBeTrue_WhenNameIsUnique()
        {
            var command = new UpdateGroupCommand
            {
                Id = 1,
                Name = "Unique"
            };

            var validator = new UpdateGroupCommandValidator(Context);

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(true);
        }

        [Fact]
        public void IsValid_ShouldBeFalse_WhenNameIsNotUnique()
        {
            Context.Groups.Add(new Group { Name = "Not unique" });
            Context.SaveChanges();

            var command = new UpdateGroupCommand
            {
                Name = "Not unique"
            };

            var validator = new UpdateGroupCommandValidator(Context);

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_ShouldBeFalse_WhenNameExcedes25characters()
        {
            var command = new UpdateGroupCommand
            {
                Name = "Some Large Name that exceeds max length"
            };

            var validator = new UpdateGroupCommandValidator(Context);

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }
    }
}
