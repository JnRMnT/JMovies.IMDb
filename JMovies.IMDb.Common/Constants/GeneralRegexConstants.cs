using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JMovies.IMDb.Common.Constants
{
    public class GeneralRegexConstants
    {
        public static readonly Regex AmountStringRegex = new Regex(@"^([^a-zA-Z0-9_\s\n]*\s?|[a-zA-Z]{2,3}\s?)([\d\.,]+)(\s?[^a-zA-Z0-9_\s\n]*|\s?[a-zA-Z]{2,3})$", RegexOptions.Multiline);
        public static readonly Regex PharantesisRegex = new Regex(@"\((.+)\)");
        public static readonly Regex HtmlDatetimeTagRegex = new Regex(@"PT(\d*D)?(\d*H)?(\d*M)?(\d*S)?");
    }
}
