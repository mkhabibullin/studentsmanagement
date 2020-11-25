using System;

namespace SM.Validation.Services
{
    public interface IValidationService
    {
        DateTime Validate(string publicId, string fullName);
    }
}
