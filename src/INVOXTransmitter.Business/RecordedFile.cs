using System;

namespace INVOXTransmitter.Business
{
    public class RecordedFile
    {
        public string Name { get; set; }

        public byte[] Content { get; set; }

        public string UserName { get; set; }
        
        public DateTimeOffset? CreatedOn { get; set; }
    }
}
