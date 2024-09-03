using CCMS.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Common.Helpers
{
    public static class LanguageCodeExtensions
    {
        public static string ToCultureString(LanguageCode languageCode)
        {
            return languageCode switch
            {
                LanguageCode.English_US => "en-US",
                LanguageCode.Arabic_EG => "ar-EG",
                _ => "en-US", // Default fallback to English
            };
        }
    }

}
