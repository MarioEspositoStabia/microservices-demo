using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservices.Demo.Core.Entity
{
    public abstract class BaseEntity : IBaseEntity
    {
        [Column(Order = 0)]
        public Guid Id { get; set; }

        public static bool operator ==(BaseEntity entity, BaseEntity otherEntity)
        {
            if (entity is null && otherEntity is null)
            {
                return true;
            }

            if (entity is null || otherEntity is null)
            {
                return false;
            }

            return entity.Equals(otherEntity);
        }

        public static bool operator !=(BaseEntity entity, BaseEntity otherEntity)
        {
            return !(entity == otherEntity);
        }

        public override bool Equals(object obj)
        {
            var otherEntity = obj as BaseEntity;

            if (otherEntity is null)
            {
                return false;
            }

            if (ReferenceEquals(this, otherEntity))
            {
                return true;
            }

            return this.Id == otherEntity.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Id);
        }

        public override string ToString()
        {
            return this.Id.ToString();
        }
    }
}
