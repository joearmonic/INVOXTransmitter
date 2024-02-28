using INVOXTransmitter.Business.Transmission;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace INVOXTransmitter.Business.Repositories
{
    public interface IFileRepository
    {
        Task<IList<RecordedFile>> GetAllSinceAsync(DateTime dateTime);

        Task SaveAsync(TranscriptedFile transcriptedFile);
    }
}