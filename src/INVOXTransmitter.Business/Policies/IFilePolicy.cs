namespace INVOXTransmitter.Business.Policies
{
    public interface IFilePolicy
    {
        bool Validate(RecordedFile file);
    }
}
