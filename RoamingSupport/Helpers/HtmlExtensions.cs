using System;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MvcExtensionsLibrary
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString EnumDisplayNameFor(this HtmlHelper html, Enum item)
        {
            var type = item.GetType();
            var member = type.GetMember(item.ToString());
            DisplayAttribute displayname = (DisplayAttribute)member[0].GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();

            if (displayname != null)
            {
                return new MvcHtmlString(displayname.Name);
            }

            return new MvcHtmlString(item.ToString());
        }
    }
}