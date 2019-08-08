using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

public static class ScreenScrapingExtensions
{
    public static string Prepare(this string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            return HttpUtility.HtmlDecode(text.Trim());
        }
        else
        {
            return text;
        }
    }
}
