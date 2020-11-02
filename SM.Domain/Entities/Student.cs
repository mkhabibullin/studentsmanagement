using SM.Domain.Common;
using SM.Domain.Enums;
using System.Collections.Generic;

namespace SM.Domain.Entities
{
    public class Student : BaseEntity<long>
    {
        public Genders Gender { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string PublicId { get; set; }

        public ICollection<StudentGroup> StudentGroups { get; set; } = new List<StudentGroup>();
    }
}
