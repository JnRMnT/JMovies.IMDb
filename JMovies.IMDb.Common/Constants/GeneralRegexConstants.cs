using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JMovies.IMDb.Common.Constants
{
    /// <summary>
    /// Class responsible for holding the Regex definitions that are commonly used
    /// </summary>
    public class GeneralRegexConstants
    {
        /// <summary>
        /// Regex that captures any money string
        /// </summary>
        public static readonly Regex AmountStringRegex = new Regex(@"^([^a-zA-Z0-9_\s\n]*\s?|[a-zA-Z]{2,3}\s?)([\d\.,]+)(\s?[^a-zA-Z0-9_\s\n]*|\s?[a-zA-Z]{2,3})$", RegexOptions.Multiline);

        /// <summary>
        /// Basic Regex that captures text inside pharantesis
        /// </summary>
        public static readonly Regex PharantesisRegex = new Regex(@"\((.+)\)");

        /// <summary>
        /// Regex that captures and groups the HTML Datetime attribute details
        /// </summary>
        public static readonly Regex HtmlDatetimeTagRegex = new Regex(@"PT(\d*D)?(\d*H)?(\d*M)?(\d*S)?");
    }
}
