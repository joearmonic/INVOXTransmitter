
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace INVOXMedicalTranscriptor.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("[controller]")]
    public class TranscriptorController : ControllerBase
    {
        private static Random _rnd = new Random();

        private readonly ILogger<TranscriptorController> _logger;

        public TranscriptorController(ILogger<TranscriptorController> logger)
        {
            _logger = logger;
        }

        [HttpPost("Transcript")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TranscriptedFile))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status412PreconditionFailed, Type = typeof(ValidationProblemDetails))]
        public ActionResult Transcript(RecordedFile recordedFile)
        {
            _logger.LogInformation($"File to Transcript {recordedFile.Name}");

            TranscriptedFile transcripted = MakeTranscription(recordedFile);

            _logger.LogInformation($"Transcripted file to {recordedFile.Name}");

            return Ok(transcripted);
        }

        private TranscriptedFile MakeTranscription(RecordedFile recordedFile)
        {
            if(RandomSuccess(0.95))
            {
                var selectedTranscriptedFilePosition = _rnd.Next(1, 4);
                using (var sw = System.IO.File.OpenText($"TranscriptedFiles/Sample{selectedTranscriptedFilePosition}.txt"))
                    return new TranscriptedFile
                    {
                        Content = UTF8Encoding.UTF8.GetBytes(sw.ReadToEnd()),
                        Name = recordedFile.Name,
                        TranscriptedOn = DateTime.UtcNow,
                        UserName = recordedFile.UserName
                    };
            }

            throw new System.Web.Http.HttpResponseException(HttpStatusCode.PreconditionFailed);
        }

        public bool RandomSuccess(double probability)
        {
            return _rnd.NextDouble() < probability;
        }
    }
}
