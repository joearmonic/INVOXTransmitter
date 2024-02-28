using System;

namespace INVOXMedicalTranscriptor
{
    public class RecordedFile
    {
        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }
        
        public string UserName { get; set; }

        public byte[]  Content { get; set; }        
    }
}
