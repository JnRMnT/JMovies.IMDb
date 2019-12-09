using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

/// <summary>
/// Class providing the extensions that are used on screen scraping operations
/// </summary>
public static class ScreenScrapingExtensions
{
    /// <summary>
    /// Prepares the screen text to be safely used by performing Html Decode and Trimming
    /// </summary>
    /// <param name="text">Text that was gathered using screen scraping</param>
    /// <returns>Clean Text</returns>
    public static string Prepare(this string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            return HttpUtility.HtmlDecode(Regex.Replace(Regex.Replace(text, "\\n", " "), @"\s+", " ").Trim());
        }
        else
        {
            return text;
        }
    }
}
