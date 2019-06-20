using Microservices.Demo.Core.Enumerations;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Microservices.Demo.Core.Entity
{
    public abstract class AuditableEntity : BaseEntity, IAuditableEntity
    {
        private StringBuilder _hmacStringBuilder;

        public AuditableEntity()
        {
            this._hmacStringBuilder = new StringBuilder();
        }

        [Column(Order = 1)]
        public DateTimeOffset CreatedDate { get; set; }

        [Column(Order = 2)]
        public Guid CreatedBy { get; set; }

        [Column(Order = 3)]
        public DateTimeOffset? LastModifiedDate { get; set; }

        [Column(Order = 4)]
        public Guid? LastModifiedBy { get; set; }

        [Column(Order = 5)]
        public long UpdateNumber { get; set; }

        [Column(Order = 6)]
        public EntityStatus Status { get; set; }

        [Column(Order = 7)]
        public byte[] RowVersion { get; set; }

        [Column(Order = 8)]
        public string Hmac
        {
            get
            {
                this._hmacStringBuilder.Clear();
                Type type = this.GetType();
                PropertyInfo[] properties = type.GetProperties();
                foreach (var property in properties)
                {
                    if (property.Name != "RowVersion" &&
                        property.Name != "Hmac" &&
                        !typeof(IEnumerable).IsAssignableFrom(property.PropertyType) &&
                        !property.PropertyType.IsClass)
                    {
                        object value = property.GetValue(this);
                        this._hmacStringBuilder.Append(value);
                    }
                }

                byte[] key = Encoding.UTF8.GetBytes("A2?3R_tp*Olq/aX-1EE[2qa^lgH1rGF1z>ta!s-DR4%)");
                using (var hmac = new HMACSHA256(key))
                {
                    byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(this._hmacStringBuilder.ToString()));
                    return Convert.ToBase64String(hash);
                }
            }

            set
            {
                // Empty setter needed for Entity Framework Core in order to map the auto computed value.
            }
        }
    }
}
