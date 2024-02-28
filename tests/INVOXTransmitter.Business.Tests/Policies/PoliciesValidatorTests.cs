using FluentAssertions;
using INVOXTransmitter.Business.Policies;
using System.Threading.Tasks;
using Xunit;

namespace INVOXTransmitter.Business.Tests.Policies
{
    public class PoliciesValidatorTests
    {
        [Fact]
        public async Task Validate_ReturnsTrue()
        {
            // Arrange
            var recordedFile = new RecordedFile
            {
                Content = new byte[] { },
                Name = "Test.mp3"
            };

            var policiesValidator = new PoliciesValidator();

            // Act
            var result = policiesValidator.IsValid(recordedFile);

            // Assert
            result.Should().BeTrue();
        }
    }
}
