using INVOXTransmitter.Application.ServiceEngine;
using INVOXTransmitter.Business.Policies;
using INVOXTransmitter.Business.Repositories;
using INVOXTransmitter.Business.Transmission;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace INVOXTransmitter.Applications.Tests
{
    public class VoiceFilesTransmitterTests
    {
        [Fact]
        public async Task Run_ReturnsSuccessfully()
        {
            // Arrange
            var validator = new Mock<IPoliciesValidator>();
            var fileRepository = new Mock<IFileRepository>();
            var transmitter = new Mock<ITransmitter>();
            var voiceFilesTransmitter = new VoiceFilesTransmitterEngine(validator.Object, fileRepository.Object, transmitter.Object);

            // Act
            await voiceFilesTransmitter.RunAsync();

            // Assert

        }
    }
}
