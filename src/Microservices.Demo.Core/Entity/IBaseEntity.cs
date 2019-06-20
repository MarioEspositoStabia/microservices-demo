using System;

namespace Microservices.Demo.Core.Entity
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }
    }
}
