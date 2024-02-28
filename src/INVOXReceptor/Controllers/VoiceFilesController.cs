using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace INVOXReceptor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VoiceFilesController : ControllerBase
    {
        private readonly ILogger<VoiceFilesController> _logger;
        private readonly string _storageConnectionString;

        public VoiceFilesController(IConfiguration configuration, ILogger<VoiceFilesController> logger)
        {
            _logger = logger;
            _storageConnectionString = configuration.GetConnectionString("INVOXStorageConnectionString");
        }

        [HttpPost("Upstream")]
        public async Task Upstream([FromForm] List<IFormFile> files, [FromForm] string UserName)
        {
            var containerClient = GetContainerClient(UserName.ToLower(), _storageConnectionString);
            if(!containerClient.Exists())                   
            {
                var recordingTag = new Dictionary<string, string>
                {
                    { "invoxrecording", "true" }
                };

                await containerClient.CreateAsync();
                await containerClient.SetMetadataAsync(recordingTag);
            }

            foreach(var file in files)
            {
                var client = containerClient.GetBlobClient($"{file.FileName}");
                await client.UploadAsync(file.OpenReadStream(), overwrite: true);
            }
        }

        private BlobContainerClient GetContainerClient(string accountName, string connectionString)
        {
            return new BlobContainerClient(
                connectionString,
                accountName);
        }
    }
}
