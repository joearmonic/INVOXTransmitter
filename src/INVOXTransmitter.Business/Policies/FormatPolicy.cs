using INVOXTransmitter.Business.Policies;
using System;

namespace INVOXTransmitter.Business.Validation
{
    public class FormatPolicy : IFilePolicy
    {
        public bool Validate(RecordedFile file)
        {
            // TODO: Look for the appropiate way of checking the mp3 audio format based on the content!
            return file.Name.EndsWith(".mp3");
        }
    }
}
