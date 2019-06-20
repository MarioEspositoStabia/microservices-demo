using Microservices.Demo.Core.Enumerations;
using Microsoft.Extensions.Localization;
using System;
using System.Globalization;
using System.Reflection;

namespace Microservices.Demo.Core.MVC
{
    public class LocalizationService
    {
        private readonly IStringLocalizer _localizer;

        public LocalizationService(IStringLocalizerFactory factory, Type type)
        {
            AssemblyName assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            this._localizer = factory.Create(type.Name, assemblyName.Name);
        }

        public LocalizedString GetLocalizedHtmlString(string key)
        {
            return this._localizer[key];
        }

        public StatusCode GetLocalizedStatusCode(ResponseCode responseCode)
        {
            string responseCodeValue = responseCode.ToString("D");
            char statusTypeChar = responseCodeValue[0];

            StatusType statusType = StatusType.Success;

            if (char.IsDigit(statusTypeChar))
            {
                int statusTypeCode = int.Parse(char.ToString(statusTypeChar), CultureInfo.InvariantCulture);

                if (Enum.IsDefined(typeof(StatusType), statusTypeCode))
                {
                    statusType = (StatusType)statusTypeCode;
                }
            }

            return new StatusCode { Type = statusType, Code = responseCodeValue, Message = this._localizer[responseCodeValue] };
        }
    }
}
