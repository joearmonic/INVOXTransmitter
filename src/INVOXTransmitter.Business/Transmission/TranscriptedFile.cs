using System;

namespace INVOXTransmitter.Business.Transmission
{
    public class TranscriptedFile
    {
        public DateTime TranscriptedOn { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public byte[] Content { get; set; }
    }
}
