using INVOXTransmitter.Business;
using INVOXTransmitter.Business.Transmission;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace INVOXTransmitter.Infraestructure.Transmission
{
    public class Transmitter : ITransmitter
    {
        private readonly HttpClient _client;
        private readonly ILogger<Transmitter> _logger;
        private IList<Transmission> _transmissions;

        public Transmitter(HttpClient client, ILogger<Transmitter> logger)
        {
            _transmissions = new List<Transmission>();
            _client = client;
            _logger = logger;
        }


        public IList<TranscriptedFile> TransmissionResults { get; private set; } =new List<TranscriptedFile>();

        public async Task DoAsync()
        {
            await TransmitAsync(_transmissions);
        }

        private async Task<IList<Transmission>> TransmitAsync(IList<Transmission> transmissions)
        {
            if (!transmissions.Any())
                return transmissions;

            var retryTransmissions = new List<Transmission>();
            foreach (var transmission in transmissions)
            {
                var serializedFile = System.Text.Json.JsonSerializer.Serialize(transmission.File);
                var postContent = new StringContent(serializedFile, Encoding.UTF8, "application/json");
                var postResponse = await _client.PostAsync("transcriptor/transcript", postContent);
                if (postResponse.IsSuccessStatusCode)
                {
                    var content = await postResponse.Content.ReadAsStringAsync();
                    var transcriptedResponse = System.Text.Json.JsonSerializer.Deserialize<TranscriptedFile>(content, new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true});
                    TransmissionResults.Add(transcriptedResponse);
                }
                else
                {
                    transmission.Attempts++;
                    _logger.LogError($"Failed to send file {transmission.File.Name}. Pending atempts {Rules.Retries - transmission.Attempts}");
                    if (transmission.Attempts < Rules.Retries)
                        retryTransmissions.Add(transmission);
                }                
            }

            return await TransmitAsync(retryTransmissions);
        }

        public void Set(IList<RecordedFile> validatedFiles)
        {
            _transmissions = validatedFiles.Select(f => new Transmission { Attempts = 0, File = f }).ToList();
        }

        internal class Transmission
        {
            public int Attempts { get; set; }

            public RecordedFile File { get; set; }
        }        
    }
}
