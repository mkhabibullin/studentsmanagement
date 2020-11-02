using System;

namespace SM.Domain.Interfaces
{
    public interface ISoftDeletable
    {
        string DeletedBy { get; set; }

        DateTime? Deleted { get; set; }

        bool IsDeleted { get; set; }
    }
}
