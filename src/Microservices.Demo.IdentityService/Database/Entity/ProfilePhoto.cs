using Microservices.Demo.Core.Entity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservices.Demo.IdentityService.Database.Entity
{
    public class ProfilePhoto : AuditableEntity
    {
        [Column(Order = 9)]
        public Guid UserId { get; set; }

        [Column(Order = 10)]
        public byte[] Photo { get; set; }

        [Column(Order = 11)]
        public string FileExtension { get; set; }

        public virtual User User { get; protected set; }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
            {
                return false;
            }

            var otherProfilePhoto = obj as ProfilePhoto;

            return this.UserId == otherProfilePhoto.UserId &&
                   this.Photo == otherProfilePhoto.Photo;
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(base.GetHashCode(), this.UserId, this.Photo);
        }
    }
}
