using System;

namespace SM.Validation.Services
{
    public class ValidationService : IValidationService
    {
        public DateTime Validate(string publicId, string fullName)
        {
            return DateTime.Now.AddDays(1);
        }
    }
}
