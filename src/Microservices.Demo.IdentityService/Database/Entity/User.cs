using Microservices.Demo.Core.Cryptography;
using Microservices.Demo.Core.Entity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservices.Demo.IdentityService.Database.Entity
{
    public class User : AuditableEntity
    {
        public User()
        {

        }

        public User(
            string userName,
            string password,
            string eMail)
        {
            this.UserName = userName;
            this.Email = eMail;
            string salt = SaltGenerator.Generate();
            this.Password = password.ComputeHashSHA256(salt);
            this.Salt = salt;
        }

        [Column(Order = 9)]
        public string UserName { get; set; }

        [Column(Order = 10)]
        public DateTimeOffset? LastActivityDate { get; set; }

        [Column(Order = 11)]
        public string Password { get; set; }

        [Column(Order = 12)]
        public string Salt { get; set; }

        [Column(Order = 13)]
        public string PasswordQuestion { get; set; }

        [Column(Order = 14)]
        public string PasswordAnswer { get; set; }

        [Column(Order = 15)]
        public string Email { get; set; }

        [Column(Order = 16)]
        public bool EmailConfirmed { get; set; }

        [Column(Order = 17)]
        public bool IsLockedOut { get; set; }

        [Column(Order = 18)]
        public DateTimeOffset? LastPasswordChangedDate { get; set; }

        [Column(Order = 19)]
        public DateTimeOffset? LastLockOutDate { get; set; }

        [Column(Order = 20)]
        public int? FailedPasswordAttempt { get; set; }

        [Column(Order = 21)]
        public DateTimeOffset? FailedPasswordAttemptDate { get; set; }

        [Column(Order = 22)]
        public int? FailedPasswordAnswerAttempt { get; set; }

        [Column(Order = 23)]
        public DateTimeOffset? FailedPasswordAnswerAttemptDate { get; set; }

        [Column(Order = 24)]
        public string Settings { get; set; }

        [Column(Order = 25)]
        public string VerificationCode { get; set; }

        public virtual ProfilePhoto ProfilePhoto { get; protected set; }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
            {
                return false;
            }

            User otherUser = obj as User;

            return this.UserName == otherUser.UserName &&
                   this.LastActivityDate == otherUser.LastActivityDate &&
                   this.LastActivityDate == otherUser.LastActivityDate &&
                   this.Password == otherUser.Password &&
                   this.Salt == otherUser.Salt &&
                   this.PasswordQuestion == otherUser.PasswordQuestion &&
                   this.PasswordAnswer == otherUser.PasswordAnswer &&
                   this.Email == otherUser.Email &&
                   this.EmailConfirmed == otherUser.EmailConfirmed &&
                   this.IsLockedOut == otherUser.IsLockedOut &&
                   this.LastPasswordChangedDate == otherUser.LastPasswordChangedDate &&
                   this.LastLockOutDate == otherUser.LastLockOutDate &&
                   this.FailedPasswordAttempt == otherUser.FailedPasswordAttempt &&
                   this.FailedPasswordAttemptDate == otherUser.FailedPasswordAttemptDate &&
                   this.FailedPasswordAnswerAttempt == otherUser.FailedPasswordAnswerAttempt &&
                   this.FailedPasswordAnswerAttemptDate == otherUser.FailedPasswordAnswerAttemptDate;
        }

        public override int GetHashCode()
        {
            HashCode hashCode = default(HashCode);
            hashCode.Add(base.GetHashCode());
            hashCode.Add(this.UserName);
            hashCode.Add(this.LastActivityDate);
            hashCode.Add(this.Password);
            hashCode.Add(this.Salt);
            hashCode.Add(this.PasswordQuestion);
            hashCode.Add(this.PasswordAnswer);
            hashCode.Add(this.Email);
            hashCode.Add(this.EmailConfirmed);
            hashCode.Add(this.IsLockedOut);
            hashCode.Add(this.LastPasswordChangedDate);
            hashCode.Add(this.FailedPasswordAttempt);
            hashCode.Add(this.LastLockOutDate);
            hashCode.Add(this.FailedPasswordAttemptDate);
            hashCode.Add(this.FailedPasswordAnswerAttempt);
            hashCode.Add(this.FailedPasswordAnswerAttemptDate);
            return hashCode.ToHashCode();
        }


        public bool ValidateUser(string password)
        {
            return this.Password == password.ComputeHashSHA256(this.Salt);
        }
    }
}
