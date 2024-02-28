using FluentAssertions;
using INVOXTransmitter.Business.Validation;
using System.Threading.Tasks;
using Xunit;

namespace INVOXTransmitter.Business.Tests.Policies
{
    public class VoiceFilesTransmitterTests
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

            var formatPolicy = new FormatPolicy();

            // Act
            var result = formatPolicy.Validate(recordedFile);

            // Assert
            result.Should().BeTrue();
        }
    }
}
