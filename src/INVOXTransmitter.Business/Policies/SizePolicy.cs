using INVOXTransmitter.Business;
using INVOXTransmitter.Business.Policies;

namespace INVOXTransmitter.Application.Validation
{
    public class SizePolicy : IFilePolicy
    {
        public bool Validate(RecordedFile file)
        {
            return file.Content.Length >= 51200 && file.Content.Length < 3000000;
        }
    }
}
