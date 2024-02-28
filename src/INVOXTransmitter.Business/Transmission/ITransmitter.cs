using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace INVOXTransmitter.Business.Transmission
{
    public interface ITransmitter
    {
        IList<TranscriptedFile> TransmissionResults { get; }

        Task DoAsync();
        
        void Set(IList<RecordedFile> validatedFiles);
    }
}
