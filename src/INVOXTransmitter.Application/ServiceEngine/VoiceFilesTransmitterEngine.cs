using INVOXTransmitter.Business;
using INVOXTransmitter.Business.Policies;
using INVOXTransmitter.Business.Repositories;
using INVOXTransmitter.Business.Transmission;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INVOXTransmitter.Application.ServiceEngine
{
    public class VoiceFilesTransmitterEngine : ITransmitterEngine
    {
        private IPoliciesValidator _validator;
        private IFileRepository _fileRepository;
        private ITransmitter _transmitter;
        private readonly ILogger<VoiceFilesTransmitterEngine> _logger;

        public VoiceFilesTransmitterEngine(IPoliciesValidator policiesValidator, IFileRepository fileRepository, ITransmitter transmitter, ILogger<VoiceFilesTransmitterEngine> logger)
        {
            _validator = policiesValidator;
            _fileRepository = fileRepository;
            _transmitter = transmitter;
            _logger = logger;
        }

        public async Task RunAsync()
        {
            var validatedFiles = new List<RecordedFile>();

            var files = await _fileRepository.GetAllSinceAsync(System.DateTime.UtcNow.AddDays(-1));
            if (!files.Any())
                return;

            files.ToList().ForEach(f =>
            {
                if (_validator.IsValid(f))
                    validatedFiles.Add(f);
                else
                {
                    _logger.LogWarning($"{f.Name} from {f.UserName} is not valid, it doesn't complaint with size or format requirement");
                }
            });

            if (!validatedFiles.Any())
                return;

            await MakeRecursiveTransmissionAsync(validatedFiles);

            if (_transmitter.TransmissionResults.Any())
            {
                foreach (var transmissionResult in _transmitter.TransmissionResults)
                {
                    transmissionResult.Name = transmissionResult.Name.Replace("mp3", "txt");
                    await _fileRepository.SaveAsync(transmissionResult);
                }
            }
        }

        private async Task MakeRecursiveTransmissionAsync(List<RecordedFile> validatedFiles)
        {
            await MakeTransmissionAsync(validatedFiles.Take(Rules.TransmissionThreshold));

            if (validatedFiles.Count > Rules.TransmissionThreshold)
                await MakeTransmissionAsync(validatedFiles.Skip(Rules.TransmissionThreshold));
        }

        private async Task MakeTransmissionAsync(IEnumerable<RecordedFile> validatedFiles)
        {
            _transmitter.Set(validatedFiles.ToList());
            await _transmitter.DoAsync();
        }
    }
}