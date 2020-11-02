using SM.Domain.Interfaces;
using System;

namespace SM.Domain.Common
{
    public abstract class BaseEntity<TId> : IAuditable, ISoftDeletable
    {
        public TId Id { get; set; }

        #region IAuditable

        public string CreatedBy { get; set; }

        public DateTime Created { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }

        #endregion

        #region ISoftDeletable

        public string DeletedBy { get; set; }

        public DateTime? Deleted { get; set; }

        public bool IsDeleted { get; set; }

        #endregion
    }
}
