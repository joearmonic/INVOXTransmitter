using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using INVOXTransmitter.Business;
using INVOXTransmitter.Business.Repositories;
using INVOXTransmitter.Business.Transmission;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace INVOXTransmitter.Infraestructure.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly string _storageConnectionString;

        public FileRepository(string storageConnectionString)
        {
            _storageConnectionString = storageConnectionString;
        }

        public async Task<IList<RecordedFile>> GetAllSinceAsync(DateTime dateTime)
        {
            var recordings = new List<RecordedFile>();
            var blobServiceClient = new BlobServiceClient(_storageConnectionString);

            var containers = blobServiceClient.GetBlobContainers(BlobContainerTraits.Metadata);
            foreach (var container in containers)
            {
                if (container.Properties.Metadata?.ContainsKey("invoxrecording") == true)
                {
                    var containerClient = blobServiceClient.GetBlobContainerClient(container.Name);
                    var userBlobs = containerClient.GetBlobs();
                    foreach (var userBlob in userBlobs)
                    {
                        if (userBlob.Properties.CreatedOn < dateTime)
                            continue;

                        var blobClient = containerClient.GetBlobClient(userBlob.Name);
                        byte[] content = null;
                        using (var reader = new StreamReader(await blobClient.OpenReadAsync()))
                        {
                            content = System.Text.Encoding.UTF8.GetBytes(reader.ReadToEnd());
                        }

                        recordings.Add(new RecordedFile { Name = userBlob.Name, Content = content, UserName = container.Name, CreatedOn = userBlob.Properties.CreatedOn });
                    }
                }
            }

            return recordings;
        }

        public async Task SaveAsync(TranscriptedFile transcriptedFile)
        {
            var containerClient = new BlobContainerClient(_storageConnectionString, transcriptedFile.UserName);
            var client = containerClient.GetBlobClient(transcriptedFile.Name);
            await client.UploadAsync(new MemoryStream(transcriptedFile.Content), overwrite: true);
        }
    }
}
