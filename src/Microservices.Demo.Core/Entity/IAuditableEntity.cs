using Microservices.Demo.Core.Enumerations;
using System;

namespace Microservices.Demo.Core.Entity
{
    public interface IAuditableEntity : IBaseEntity
    {
        DateTimeOffset CreatedDate { get; set; }

        Guid CreatedBy { get; set; }

        DateTimeOffset? LastModifiedDate { get; set; }

        Guid? LastModifiedBy { get; set; }

        long UpdateNumber { get; set; }

        EntityStatus Status { get; set; }

        byte[] RowVersion { get; set; }

        string Hmac { get; }
    }
}
