using SM.Domain.Common;
using System.Collections.Generic;

namespace SM.Domain.Entities
{
    public class Group : BaseEntity<int>
    {
        public string Name { get; set; }

        public ICollection<StudentGroup> StudentGroups { get; set; } = new List<StudentGroup>();
    }
}
