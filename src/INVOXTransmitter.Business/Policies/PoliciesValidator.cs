using INVOXTransmitter.Application.Validation;
using INVOXTransmitter.Business.Validation;
using System.Collections.Generic;

namespace INVOXTransmitter.Business.Policies
{
    public class PoliciesValidator : IPoliciesValidator
    {
        private IList<IFilePolicy> _policies;

        public PoliciesValidator()
        {
            _policies = new List<IFilePolicy>
            {
                new FormatPolicy(),
                new SizePolicy()
            };
        }

        public bool IsValid(RecordedFile file)
        {
            foreach (var policy in _policies)
            {
                if (!policy.Validate(file))
                    return false;
            }

            return true;
        }
    }
}
