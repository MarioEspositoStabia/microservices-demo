using Microservices.Demo.Core.Enumerations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservices.Demo.Core.Entity
{
    public class ChangeLog : BaseEntity
    {
        [Column(Order = 1)]
        public string EntityName { get; set; }

        [Column(Order = 2)]
        public EntityOperation Operation { get; set; }

        [Column(Order = 3)]
        public string PrimaryKey { get; set; }

        [Column(Order = 4)]
        public string PropertyName { get; set; }

        [Column(Order = 5)]
        public string OldValue { get; set; }

        [Column(Order = 6)]
        public string NewValue { get; set; }

        [Column(Order = 7)]
        public string ChangedBy { get; set; }

        [Column(Order = 8)]
        public DateTimeOffset ChangedDate { get; set; }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
            {
                return false;
            }

            var otherChangeLog = obj as ChangeLog;

            return this.EntityName == otherChangeLog.EntityName &&
                   this.Operation == otherChangeLog.Operation &&
                   this.PrimaryKey == otherChangeLog.PrimaryKey &&
                   this.PropertyName == otherChangeLog.PropertyName &&
                   this.OldValue == otherChangeLog.OldValue &&
                   this.NewValue == otherChangeLog.NewValue &&
                   this.ChangedBy == otherChangeLog.ChangedBy &&
                   this.ChangedDate == otherChangeLog.ChangedDate;
        }

        public override int GetHashCode()
        {
            HashCode hashCode = default(HashCode);
            hashCode.Add(this.Id);
            hashCode.Add(this.EntityName);
            hashCode.Add(this.Operation);
            hashCode.Add(this.PrimaryKey);
            hashCode.Add(this.PropertyName);
            hashCode.Add(this.OldValue);
            hashCode.Add(this.NewValue);
            hashCode.Add(this.ChangedBy);
            hashCode.Add(this.ChangedDate);
            return hashCode.ToHashCode();
        }
    }
}
