using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Demo.Core.Enumerations
{
    public enum ResponseCode
    {
        Success = 200,

        Authentication = 300,
        UnknownUser = 301,
        SubscriptionNotFound = 302,
        SecurityTokenExpired = 303,
        InvalidSecurityToken = 304,
        EmailNotConfirmed = 305,
        UserNameExists = 306,
        EmailExists = 307,
        FamilyLineNotFound = 308,
        IndividualNotFound = 309,
        UserLocked = 310,

        Authorization = 400,
        UnauthorizedAccess = 401,
        MissingHeader = 402
    }
}
