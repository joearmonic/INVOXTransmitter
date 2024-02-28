using System.IO;

namespace INVOXTransmitter.Business.Policies
{
    public interface IPoliciesValidator
    {
        bool IsValid(RecordedFile file);
    }
}