using System.Collections.Generic;
using System.Globalization;

namespace Microservices.Demo.Core.MVC
{
    public class LocalizationOptions
    {
       public List<CultureInfo> SupportedCultures { get; set; }
    }
}
