using System;
using System.Collections.Generic;
using System.Linq;

namespace SM.Registration.Services
{
    public class RegistrationService : IRegistrationService
    {
        private static IDictionary<long, StudentModel> Data { get; set; } = new Dictionary<long, StudentModel>();

        public string Register(long studentId, string fullName)
        {
            if (Data.Any(d => d.Value.FullName.Equals(fullName, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new Exception("The student ID had already beed registered");
            }

            var publicId = GeneratePublicId(studentId, fullName);

            Data.Add(studentId, new StudentModel { Id = studentId, FullName = fullName });

            return publicId;
        }

        private string GeneratePublicId(long studentId, string fullName)
        {
            return $"{studentId}{fullName}";
        }
    }

    class StudentModel
    {
        public long Id { get; set; }

        public string FullName { get; set; }
    }
}
