using System;

namespace INVOXReceptor
{
    public class VoiceFile
    {
        public DateTime UploadOn { get; set; }

        public string UserName { get; set; }

        public byte[] Content {  get; set; }

        public string FileName { get; set; }
    }
}