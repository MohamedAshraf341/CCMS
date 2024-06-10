using Microsoft.AspNetCore.Components;
using System.Collections.Specialized;
using System.Web;
using System;
using System.Linq;
using Microsoft.AspNetCore.WebUtilities;

namespace CCMS.FE.UI.Extensions
{
    static class NavigationManagerExtensions
    {
        /// <summary>
        /// Gets the section part of the documentation page
        /// Ex: /components/button;  "components" is the section
        /// </summary>
        public static string GetSection(this NavigationManager navMan)
        {
            // get the absolute path with out the base path
            var currentUri = navMan.Uri.Remove(0, navMan.BaseUri.Length - 1);
            var firstElement = currentUri
                .Split("/", StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault();
            return firstElement;
        }

        public static NameValueCollection QueryString(this NavigationManager navigationManager)
        {
            return HttpUtility.ParseQueryString(new Uri(navigationManager.Uri).Query);
        }

        public static string QueryString(this NavigationManager navigationManager, string key)
        {
            return navigationManager.QueryString()[key];
        }
        public static bool TryGetQueryString<T>(this NavigationManager navManager, string key, out T value)
        {
            var uri = navManager.ToAbsoluteUri(navManager.Uri);

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(key, out var valueFromQueryString))
            {
                if (typeof(T) == typeof(int) && int.TryParse(valueFromQueryString, out var valueAsInt))
                {
                    value = (T)(object)valueAsInt;
                    return true;
                }

                if (typeof(T) == typeof(string))
                {
                    value = (T)(object)valueFromQueryString.ToString();
                    return true;
                }

                if (typeof(T) == typeof(decimal) && decimal.TryParse(valueFromQueryString, out var valueAsDecimal))
                {
                    value = (T)(object)valueAsDecimal;
                    return true;
                }
                if (typeof(T).IsEnum)
                {
                    var val = Enum.Parse(typeof(T), valueFromQueryString);
                    value = (T)val;
                    return true;
                }
            }

            value = default;
            return false;
        }
    }

}
